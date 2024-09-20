using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ZadElealam.Core.Dto;
using ZadElealam.Core.Errors;
using ZadElealam.Core.IRepository;
using ZadElealam.Core.Models.Auth;

namespace ZadElealam.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly UserManager<AppUser> _userManager;

        public NotificationController(INotificationRepository notificationRepository,UserManager<AppUser> userManager)
        {
            _notificationRepository = notificationRepository;
            _userManager = userManager;
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpGet("GetNotificationsForUserAsync")]
        public async Task<IActionResult> GetNotificationsForUserAsync()
        {
            var response = await _notificationRepository.GetNotificationsForUserAsync();
            return Ok(response);
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPost("markNotificationAsRead")]
        public async Task<IActionResult> MarkNotificationAsReadAsync(int notificationId)
        {
            var response = await _notificationRepository.MarkNotificationAsReadAsync(notificationId);
            return Ok(response);
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPost("markNotificationAsUnread")]
        public async Task<IActionResult> MarkNotificationAsUnreadAsync(int notificationId)
        {
            var response = await _notificationRepository.MarkNotificationAsUnreadAsync(notificationId);
            return Ok(response);
        }
        
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("deleteNotification")]
        public async Task<IActionResult> DeleteNotificationAsync(int notificationId)
        {
            var response = await _notificationRepository.DeleteNotificationAsync(notificationId);
            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost("createNotification")]
        public async Task<IActionResult> CreateNotificationAsync(NotificationDto model)
        {
            var response = await _notificationRepository.CreateNotificationAsync(model);
            return Ok(response);
        }
    }
}
