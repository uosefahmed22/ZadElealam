using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZadElealam.Core.Dto;
using ZadElealam.Core.Errors;
using ZadElealam.Core.IRepository;
using ZadElealam.Core.Models;
using ZadElealam.Repository.Data;

namespace ZadElealam.Repository.Repository
{
    public class FeedbackAndFavorities : IFeedbackAndFavorities
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public FeedbackAndFavorities(AppDbContext dbContext,
            IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        //Feedback
        public async Task<ApiResponse> AddFeedback(string UserId, FeedbackDto feedback)
        {
            try
            {
                var mappedFeedback = _mapper.Map<FeedbackDto, Feedback>(feedback);
                mappedFeedback.UserId = UserId;
                await _dbContext.Feedbacks.AddAsync(mappedFeedback);
                await _dbContext.SaveChangesAsync();
                return new ApiResponse(200, "تم إضافة التقييم بنجاح");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }
        public async Task<ApiResponse> DeleteFeedback(int id, string UserId)
        {
            try
            {
                var feedback = await _dbContext
                    .Feedbacks
                    .FirstOrDefaultAsync(f => f.Id == id && f.UserId == UserId);
                if (feedback == null)
                {
                    return new ApiResponse(404, "هذا التقييم غير موجود");
                }
                _dbContext.Feedbacks.Remove(feedback);
                await _dbContext.SaveChangesAsync();
                return new ApiResponse(200, "تم حذف التقييم بنجاح");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }
        public async Task<ApiResponse> GetAllFeedbackByPlaylist(int YouTubePlaylistId)
        {
            try
            {
                var feedbacks = await _dbContext
                    .Feedbacks
                    .Where(f => f.YouTubePlaylistId == YouTubePlaylistId)
                    .Select(f => new
                    {
                        f.Id,
                        f.FeedbackMessage,
                        f.Rating,
                        f.Date,
                    })
                    .ToListAsync();
                if (!feedbacks.Any())
                {
                    return new ApiResponse(404, "لا يوجد تقييمات");
                }
                return new ApiResponse(200, feedbacks);
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }
        public async Task<ApiResponse> UpdateFeedback(UpdateFeedbackDto model,string userId, int id)
        {
            try
            {
                var feedbackExist = await _dbContext.Feedbacks
                    .FirstOrDefaultAsync(f => f.Id == id && f.UserId == userId);
                if (feedbackExist == null)
                {
                    return new ApiResponse(404, "لا يوجد تقييم");
                }
                _mapper.Map(model, feedbackExist);
                await _dbContext.SaveChangesAsync();
                return new ApiResponse(200, "تم تحديث التقييم بنجاح");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }

        //Favorities
        public async Task<ApiResponse> AddFavorities(string UserId, int PlayListId)
        {
            try
            {
                var favorities = new Favorities
                {
                    UserId = UserId,
                    PlayListId = PlayListId
                };
                if (await _dbContext.Favorities.AnyAsync(f => f.UserId == UserId && f.PlayListId == PlayListId))
                {
                    return new ApiResponse(400, "هذا العنصر موجود بالفعل في القائمة المفضلة");
                }
                await _dbContext.Favorities.AddAsync(favorities);
                await _dbContext.SaveChangesAsync();
                return new ApiResponse(200, "تم إالاضافة الي القائمة المفضلة بنجاح");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }
        public async Task<ApiResponse> DeleteFavorities(int id, string UserId)
        {
            try
            {
                var favorities = await _dbContext
                    .Favorities
                    .FirstOrDefaultAsync(f => f.Id == id && f.UserId == UserId);
                if (favorities == null)
                {
                    return new ApiResponse(404, "لا يوجد في القائمة المفضلة");
                }
                _dbContext.Favorities.Remove(favorities);
                await _dbContext.SaveChangesAsync();
                return new ApiResponse(200, "Favorities Deleted Successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }
        public async Task<ApiResponse> GetAllFavoritiesForUser(string UserId)
        {
            try
            {
                var favorities = await _dbContext
                    .Favorities
                    .Where(f => f.UserId == UserId)
                    .Select(f => new
                    {
                        f.Id,
                        f.PlayListId
                    })
                    .ToListAsync();
                if (!favorities.Any())
                {
                    return new ApiResponse(404, "لا يوجد عناصر في القائمة المفضلة");
                }
                return new ApiResponse(200, favorities);
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }
    }
}