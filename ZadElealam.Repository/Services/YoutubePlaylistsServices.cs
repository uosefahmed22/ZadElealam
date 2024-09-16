using AutoMapper;
using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZadElealam.Core.Dto;
using ZadElealam.Core.Errors;
using ZadElealam.Core.IServices;
using ZadElealam.Core.Models;

namespace ZadElealam.Repository.Services
{
    public class YoutubePlaylistsServices : IYoutubePlaylistsServices
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public YoutubePlaylistsServices(AppDbContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }
        public async Task<List<YouTubeVideo>> GetPlaylistVideosAsync(string playlistId)
        {
            var youtubeService = new YouTubeService(new BaseClientService.Initializer()
            {
                ApiKey = "AIzaSyDfFkZh-tgi65A_fngn6ye1Md9knVg_oVU",
                ApplicationName = "ZadElealem"
            });

            var playlistItemsRequest = youtubeService.PlaylistItems.List("snippet");
            playlistItemsRequest.PlaylistId = playlistId;
            playlistItemsRequest.MaxResults = 50;

            var playlistItemsResponse = await playlistItemsRequest.ExecuteAsync();

            return playlistItemsResponse.Items.Select(item => new YouTubeVideo
            {
                VideoUrl = $"https://www.youtube.com/watch?v={item.Snippet.ResourceId.VideoId}",
                Title = item.Snippet.Title
            }).ToList();
        }
        public async Task<ApiResponse> AddCategoryAsync(CategoryDto category)
        {
            var categoryExist = _dbContext.Categories.FirstOrDefault(c => c.Name == category.Name);
            if (!(categoryExist is null))
            {
                return new ApiResponse(400, "Category already exists");
            }

            var newCategory = _mapper.Map<Category>(category);

            _dbContext.Categories.Add(newCategory);
            await _dbContext.SaveChangesAsync();

            return new ApiResponse(200, "Category added successfully");
        }
        public async Task<List<Category>> GetCategoriesAsync()
        {
            var categories = await _dbContext.Categories.ToListAsync();
            return categories;
        }
        public async Task<List<YouTubeVideoDto>> GetVideosByPlaylistIdAsync(int id)
        {
            var videos = await _dbContext
                .YouTubeVideos
                .Where(v => v.YouTubePlaylistId == id)
                .Select(v => new YouTubeVideo
                {
                    Title = v.Title,
                    VideoUrl = v.VideoUrl,
                    YouTubePlaylistId = v.YouTubePlaylistId
                })
                .ToListAsync();
           var mapped = _mapper.Map<List<YouTubeVideoDto>>(videos);
            return mapped;
        }

    }
}
