using Google;
using Google.Apis.YouTube.v3.Data;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using ZadElealam.Core.Dto;
using ZadElealam.Core.Errors;
using ZadElealam.Core.IRepository;
using ZadElealam.Core.Models;
using ZadElealam.Repository.Data;

namespace ZadElealam.Repository.Repository
{
    public class PlaylistRepository : IPlaylistRepository
    {
        private readonly AppDbContext _context;

        public PlaylistRepository(AppDbContext context)
        {
            _context = context;
        }
        public async Task<ApiResponse> AddPlaylistFromYouTubeAsync(string playlistUrl, int categoryId)
        {
            var playlistId = ExtractYouTubePlaylistId(playlistUrl);

            if (string.IsNullOrEmpty(playlistId))
            {
                return new ApiResponse(400, "رابط قائمة التشغيل على اليوتيوب غير صالح.");
            }

            if (await _context.YouTubePlaylists.AnyAsync(p => p.YouTubePlaylistId == playlistId))
            {
                return new ApiResponse(409, "قائمة التشغيل موجودة بالفعل.");
            }

            if (!await _context.Categories.AnyAsync(c => c.Id == categoryId))
            {
                return new ApiResponse(404, "الفئة المحددة غير موجودة.");
            }

            var youtubePlaylist = await FetchPlaylistFromYouTubeAsync(playlistId);

            if (youtubePlaylist == null)
            {
                return new ApiResponse(404, "تعذر جلب قائمة التشغيل من اليوتيوب.");
            }

            var playlist = new YouTubePlaylist
            {
                Title = youtubePlaylist.Title,
                Description = youtubePlaylist.Description,
                YouTubePlaylistId = playlistId,
                CategoryId = categoryId,
                Url = $"https://www.youtube.com/playlist?list={playlistId}",
                VideosCount = youtubePlaylist.Videos.Count,
                ThumbnailUrl = youtubePlaylist.ThumbnailUrl,
                CourseHours = (float)youtubePlaylist.Videos.Sum(v => v.Duration.TotalHours),
                Videos = youtubePlaylist.Videos.Select(video => new YouTubeVideo
                {
                    Title = video.Title,
                    Description = video.Description,
                    YouTubeVideoId = video.YouTubeVideoId,
                    ThumbnailUrl = video.ThumbnailUrl,
                    Duration = video.Duration,
                    Url = video.Url
                }).ToList()
            };


            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.YouTubePlaylists.Add(playlist);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ApiResponse(200, "تمت إضافة قائمة التشغيل بنجاح.");
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                return new ApiResponse(500, "حدث خطأ أثناء حفظ قائمة التشغيل.");
            }
        }
        public async Task<ApiResponse> GetPlaylistsByCategoryIdAsync(int categoryId)
        {
            var playlists = await _context.YouTubePlaylists
                                          .Where(p => p.CategoryId == categoryId)
                                          .Select(p => new
                                          {
                                              Id = p.Id,
                                              Title = p.Title,
                                              Description = p.Description,
                                              ThumbnailUrl = p.Videos.FirstOrDefault().ThumbnailUrl,
                                              Url = p.Url,
                                              VideosCount = p.VideosCount,
                                              CourseHours = p.CourseHours,
                                              Rating = GetAverageRating(p.Id).Result
                                          })
                                          .ToListAsync();
            
            if (playlists == null || !playlists.Any())
            {
                return new ApiResponse(404, "لم يتم العثور على قوائم تشغيل لهذه الفئة.");
            }

            return new ApiResponse(200, playlists);
        }
        public async Task<ApiResponse> GetVideosByPlaylistIdAsync(int playlistId)
        {
            var videos = await _context.YouTubeVideos
                                       .Where(v => v.PlaylistId == playlistId)
                                       .Select(v => new
                                       {
                                           Id = v.Id,
                                           Title = v.Title,
                                           Duration = v.Duration,
                                           ThumbnailUrl = v.ThumbnailUrl,
                                           Url = v.Url
                                       })
                                       .ToListAsync();

            if (videos == null || !videos.Any())
            {
                return new ApiResponse(404, "لم يتم العثور على فيديوهات لهذه القائمة.");
            }

            return new ApiResponse(200, videos);
        }
        public async Task<ApiResponse> DeletePlaylistAsync(int playlistId)
        {
            var playlist = await _context.YouTubePlaylists.FindAsync(playlistId);
            if (playlist == null)
            {
                return new ApiResponse(404, "قائمة التشغيل غير موجودة.");
            }
            _context.YouTubePlaylists.Remove(playlist);
            await _context.SaveChangesAsync();
            return new ApiResponse(200, "تم حذف قائمة التشغيل بنجاح.");
        }
        
        public async Task<ApiResponse> UpdateVideoProgressAsync(string studentId, int videoId, TimeSpan watchedDuration)
        {
            var video = await _context.YouTubeVideos.FindAsync(videoId);
            if (video == null)
            {
                return new ApiResponse(404, "الفيديو غير موجود.");
            }

            if (watchedDuration > video.Duration)
            {
                return new ApiResponse(400, "المدة المشاهدة أكبر من مدة الفيديو.");
            }
            await UpdateProgress(studentId, videoId, watchedDuration);
            return new ApiResponse(200, "تم تحديث التقدم بنجاح.");
        }
        
        public async Task<ApiResponse> GetErollmentCourses(string studentId)
        {
            var courses = await _context.Enrollments
                                       .Where(e => e.UserId == studentId)
                                       .Select(e => new
                                       {
                                           Id = e.PlayListId,
                                           Title = e.PlayList.Title,
                                           Description = e.PlayList.Description,
                                           ThumbnailUrl = e.PlayList.Videos.FirstOrDefault().ThumbnailUrl,
                                           Url = e.PlayList.Url
                                       })
                                       .ToListAsync();

            if (courses == null || !courses.Any())
            {
                return new ApiResponse(200, "لم يتم العثور على دورات لهذا المستخدم.");
            }

            return new ApiResponse(200, courses);
        }
        public async Task<ApiResponse> EnrollToCourse(string studentId, int playlistId)
        {
            try
            {
                if (await _context.Enrollments.AnyAsync(e => e.UserId == studentId && e.PlayListId == playlistId))
                {
                    return new ApiResponse(409, "أنت مسجل بالفعل في هذه الدورة.");
                }

                if (!await _context.YouTubePlaylists.AnyAsync(p => p.Id == playlistId))
                {
                    return new ApiResponse(404, "قائمة التشغيل غير موجودة.");
                }

                var enrollment = new Enrollment
                {
                    PlayListId = playlistId,
                    UserId = studentId
                };

                _context.Enrollments.Add(enrollment);
                await _context.SaveChangesAsync();
                return new ApiResponse(200, "تم التسجيل بنجاح.");
            }
            catch (Exception)
            {
                return new ApiResponse(500, "حدث خطأ أثناء التسجيل.");
            }
        }
        
        public async Task<ApiResponse> GetCourseById(int playlistId)
        {
            try
            {
                var playlist = await _context.YouTubePlaylists
                    .Where(p => p.Id == playlistId)
                    .Select(p => new
                    {
                        Title = p.Title,
                        Description = p.Description,
                        ThumbnailUrl = p.Videos.FirstOrDefault().ThumbnailUrl,
                        Url = p.Url,
                        VideosCount = p.VideosCount,
                        CourseHours = p.CourseHours
                    })
                    .FirstOrDefaultAsync();

                if (playlist == null)
                {
                    return new ApiResponse
                    {
                        Message = "Playlist not found."
                    };
                }

                var rating = await GetAverageRating(playlistId);

                return new ApiResponse(200, new { playlist, rating });
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }

        private async Task<double> GetAverageRating(int playlistId)
        {
            var ratings = await _context.Feedbacks
                .Where(f => f.YouTubePlaylistId == playlistId)
                .Select(f => f.Rating)
                .ToListAsync();

            int numberOfRatings = ratings.Count;

            if (numberOfRatings == 0)
            {
                return 0; 
            }

            double totalRatings = ratings.Sum();
            double averageRating = totalRatings / numberOfRatings;

            return Math.Round(averageRating, 1);
        }
        private string ExtractYouTubePlaylistId(string playlistUrl)
        {
            var uri = new Uri(playlistUrl);
            var query = HttpUtility.ParseQueryString(uri.Query);
            return query["list"];
        }
        private async Task<YouTubePlaylist> FetchPlaylistFromYouTubeAsync(string playlistId)
        {
            var youtubeService = new YouTubeService("AIzaSyDfFkZh-tgi65A_fngn6ye1Md9knVg_oVU");
            var playlist = youtubeService.GetPlaylistAsync(playlistId);
            var videos = youtubeService.GetVideosFromPlaylistAsync(playlistId);

            await Task.WhenAll(playlist, videos);
            return new YouTubePlaylist
            {
                YouTubePlaylistId = playlistId,
                Title = playlist.Result.Title,
                Description = playlist.Result.Description,
                Url = $"https://www.youtube.com/playlist?list={playlistId}",
                Videos = videos.Result,
                ThumbnailUrl = videos.Result.FirstOrDefault()?.ThumbnailUrl
            };
        }
        private async Task UpdateProgress(string studentId, int videoId, TimeSpan watchedDuration)
        {
            var progress = await _context.StudentVideoProgresses
                .FirstOrDefaultAsync(p => p.StudentId == studentId && p.VideoId == videoId);

            if (progress == null)
            {
                progress = new StudentVideoProgress
                {
                    StudentId = studentId,
                    VideoId = videoId,
                    WatchedDuration = watchedDuration,
                    IsCompleted = false
                };
                _context.StudentVideoProgresses.Add(progress);
            }
            else
            {
                progress.WatchedDuration = watchedDuration;
                if (watchedDuration >= progress.Video.Duration * 0.9)
                {
                    progress.IsCompleted = true;
                    progress.CompletionDate = DateTime.Now;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
