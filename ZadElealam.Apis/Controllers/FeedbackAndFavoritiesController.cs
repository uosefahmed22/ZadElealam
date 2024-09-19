using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.SharePoint.Client;
using System.Security.Claims;
using ZadElealam.Core.Dto;
using ZadElealam.Core.Errors;
using ZadElealam.Core.IRepository;
using ZadElealam.Core.Models;
using ZadElealam.Core.Models.Auth;

namespace ZadElealam.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackAndFavoritiesController : ControllerBase
    {
        private readonly IFeedbackAndFavorities _feedbackAndFavorities;
        private readonly UserManager<AppUser> _userManager;

        public FeedbackAndFavoritiesController(IFeedbackAndFavorities feedbackAndFavorities,
            UserManager<AppUser> userManager)
        {
            _feedbackAndFavorities = feedbackAndFavorities;
            _userManager = userManager;
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPost("addfeedback")]
        public async Task<IActionResult> AddFeedback(FeedbackDto feedback)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null)
            {
                return BadRequest(new ApiResponse(400, "Invalid user"));
            }
            var user = await _userManager.FindByEmailAsync(email);
            var result = await _feedbackAndFavorities.AddFeedback(user.Id, feedback);
            return Ok(result);
        }

        [HttpGet("getallfeedbackbyplaylist")]
        public async Task<IActionResult> GetAllFeedback(int YouTubePlaylistId)
        {
            var result = await _feedbackAndFavorities.GetAllFeedbackByPlaylist(YouTubePlaylistId);
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpDelete("deletefeedback")]
        public async Task<IActionResult> DeleteFeedback(int feedbackId)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            var user = await _userManager.FindByEmailAsync(email);
            if (email == null)
            {
                return BadRequest(new ApiResponse(400, "Invalid user"));
            }
            var result = await _feedbackAndFavorities.DeleteFeedback(feedbackId, user.Id);
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPut("updatefeedback")]
        public async Task<IActionResult> UpdateFeedback(int feedbackId, UpdateFeedbackDto feedback)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null)
            {
                return BadRequest(new ApiResponse(400, "Invalid user"));
            }
            var user = await _userManager.FindByEmailAsync(email);
            var result = await _feedbackAndFavorities.UpdateFeedback(feedback, user.Id, feedbackId);
            return Ok(result);
        }

        //Favorities
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPost("addfavorities")]
        public async Task<IActionResult> AddFavorities(int PlayListId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null)
            {
                return BadRequest(new ApiResponse(400, "Invalid user"));
            }
            var user = await _userManager.FindByEmailAsync(email);
            var result = await _feedbackAndFavorities.AddFavorities(user.Id, PlayListId);
            return Ok(result);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpGet("getallfavorities")]
        public async Task<IActionResult> GetAllFavorities()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null)
            {
                return BadRequest(new ApiResponse(400, "Invalid user"));
            }
            var user = await _userManager.FindByEmailAsync(email);
            var result = await _feedbackAndFavorities.GetAllFavoritiesForUser(user.Id);
            return Ok(result);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpDelete("deletefavorities")]
        public async Task<IActionResult> DeleteFavorities(int FavoriteId)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null)
            {
                return BadRequest(new ApiResponse(400, "Invalid user"));
            }
            var user = await _userManager.FindByEmailAsync(email);
            var result = await _feedbackAndFavorities.DeleteFavorities(FavoriteId, user.Id);
            return Ok(result);
        }
    }
}
