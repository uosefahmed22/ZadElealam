using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZadElealam.Core.Dto;
using ZadElealam.Core.Errors;

namespace ZadElealam.Core.IRepository
{
    public interface INotificationRepository
    {
        Task<ApiResponse> GetNotificationsForUserAsync();
        Task<ApiResponse> MarkNotificationAsReadAsync(int notificationId);
        Task<ApiResponse> MarkNotificationAsUnreadAsync(int notificationId);
        Task<ApiResponse> DeleteNotificationAsync(int notificationId);
        Task<ApiResponse> CreateNotificationAsync(NotificationDto model);
    }
}
