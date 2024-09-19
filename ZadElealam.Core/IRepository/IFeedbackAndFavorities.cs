using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZadElealam.Core.Dto;
using ZadElealam.Core.Errors;
using ZadElealam.Core.Models;

namespace ZadElealam.Core.IRepository
{
    public interface IFeedbackAndFavorities
    {
        //Feedback
        Task<ApiResponse> AddFeedback(string UserId,FeedbackDto feedback);
        Task<ApiResponse> GetAllFeedbackByPlaylist(int YouTubePlaylistId);
        Task<ApiResponse> DeleteFeedback(int id, string UserId);
        Task<ApiResponse> UpdateFeedback(UpdateFeedbackDto model, string userId, int id);
        //Favorities
        Task<ApiResponse> AddFavorities(string UserId, int PlayListId);
        Task<ApiResponse> GetAllFavoritiesForUser(string UserId);
        Task<ApiResponse> DeleteFavorities(int id, string UserId);
    }
}
