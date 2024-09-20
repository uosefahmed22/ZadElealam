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
    public class NotificationRepository : INotificationRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly IMapper _mapper;

        public NotificationRepository(AppDbContext dbContext,IMapper mapper)
        {
            _dbContext = dbContext;
            _mapper = mapper;
        }

        public async Task<ApiResponse> CreateNotificationAsync(NotificationDto model)
        {
            try { 
                var notification = _mapper.Map<Notification>(model);
                await _dbContext.Notifications.AddAsync(notification);
                await _dbContext.SaveChangesAsync();
                return new ApiResponse(200, "Notification created successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }
        public async Task<ApiResponse> DeleteNotificationAsync(int notificationId)
        {
            try
            {
                var notification = await _dbContext.Notifications.FindAsync(notificationId);
                if (notification == null)
                {
                    return new ApiResponse(404, "Notification not found");
                }
                _dbContext.Notifications.Remove(notification);
                await _dbContext.SaveChangesAsync();
                return new ApiResponse(200, "Notification deleted successfully");
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }
        public async Task<ApiResponse> GetNotificationsForUserAsync()
        {
            try
            {
                var notifications =await _dbContext
                    .Notifications
                    .Select(n => new Notification
                    {
                        Id = n.Id,
                        Title = n.Title,
                        Message = n.Message,
                        Date = n.Date,
                        IsRead = n.IsRead
                    })
                    .ToListAsync();
                return new ApiResponse(200, "Notifications retrieved successfully", notifications);
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }
        public async Task<ApiResponse> MarkNotificationAsReadAsync(int notificationId)
        {
            try
            {
                var notification = await _dbContext
                    .Notifications
                    .FindAsync(notificationId);
                if (notification == null)
                    return new ApiResponse(404, "Notification not found");
                
                notification.MarkAsRead();
                await _dbContext.SaveChangesAsync();
                return new ApiResponse(200, "Notification marked as read successfully", notification);
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }
        public async Task<ApiResponse> MarkNotificationAsUnreadAsync(int notificationId)
        {
            try
            {
                var notification = await _dbContext
                    .Notifications
                    .FindAsync(notificationId);
                if (notification == null)
                {
                    return new ApiResponse(404, "Notification not found");
                }
                if(!notification.IsRead)
                {
                    return new ApiResponse(400, "Notification is already unread");
                }
                notification.IsRead = false;
                await _dbContext.SaveChangesAsync();
                return new ApiResponse(200, "Notification marked as unread successfully", notification);
            }
            catch (Exception ex)
            {
                return new ApiResponse(500, ex.Message);
            }
        }
    }
}
