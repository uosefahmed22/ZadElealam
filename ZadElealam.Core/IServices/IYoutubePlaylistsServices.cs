using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZadElealam.Core.Dto;
using ZadElealam.Core.Errors;
using ZadElealam.Core.Models;

namespace ZadElealam.Core.IServices
{
    public interface IYoutubePlaylistsServices
    {
        Task<List<YouTubeVideo>> GetPlaylistVideosAsync(string playlistId);
        Task<ApiResponse> AddCategoryAsync(CategoryDto category);
        Task<List<Category>> GetCategoriesAsync();
        Task<List<YouTubeVideoDto>> GetVideosByPlaylistIdAsync(int id);
    }
}
