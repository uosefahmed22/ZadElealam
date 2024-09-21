using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using ZadElealam.Core.Dto;
using ZadElealam.Core.Errors;
using ZadElealam.Core.IRepository;
using ZadElealam.Core.Models;
using ZadElealam.Core.Models.Auth;
using ZadElealam.Repository;
using ZadElealam.Repository.Repository;

namespace ZadElealam.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YoutubePlaylistController : ControllerBase
    {
        private readonly IPlaylistRepository _playlistRepository;
        private readonly UserManager<AppUser> _userManager;

        public YoutubePlaylistController(IPlaylistRepository playlistRepository,UserManager<AppUser> userManager)
        {
            _playlistRepository = playlistRepository;
            _userManager = userManager;
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost("addplaylist")]
        public async Task<IActionResult> AddPlaylist([FromBody] AddPlaylistRequest addPlaylistRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var response = await _playlistRepository.AddPlaylistFromYouTubeAsync(addPlaylistRequest.PlaylistUrl, addPlaylistRequest.CategoryId);
            return Ok(response);
        }

        [HttpGet("getplaylistsbycategory")]
        public async Task<IActionResult> GetPlaylistsByCategory(int categoryId)
        {
            var playlists = await _playlistRepository.GetPlaylistsByCategoryIdAsync(categoryId);
            return Ok(playlists);
        }

        [HttpGet("getvideosbyplaylist")]
        public async Task<IActionResult> GetVideosByPlaylist(int playlistId)
        {
            var videos = await _playlistRepository.GetVideosByPlaylistIdAsync(playlistId);
            return Ok(videos);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpDelete("deleteplaylist")]
        public async Task<IActionResult> DeletePlaylist(int playlistId)
        {
            var response = await _playlistRepository.DeletePlaylistAsync(playlistId);
            return Ok(response);
        }

        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        [HttpPost("updatevideoprogress")]
        public async Task<IActionResult> UpdateVideoProgress([FromBody] UpdateVideoProgressRequest updateVideoProgressRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
            var response = await _playlistRepository.UpdateVideoProgressAsync(updateVideoProgressRequest.StudentId, updateVideoProgressRequest.VideoId, updateVideoProgressRequest.WatchedDuration);
            return Ok(response);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpGet("getenrollmentcourses")]
        public async Task<IActionResult> GetEnrollmentCourses()
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null)
            {
                return BadRequest(new ApiResponse(400, "Invalid user"));
            }
            var user = await _userManager.FindByEmailAsync(email);
            var response = await _playlistRepository.GetErollmentCourses(user.Id);
            return Ok(response);
        }
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "User")]
        [HttpPost("enrolltocourse")]
        public async Task<IActionResult> EnrollToCourse(int PlaylistId)
        {
            var email = User.FindFirst(ClaimTypes.Email)?.Value;
            if (email == null)
            {
                return BadRequest(new ApiResponse(400, "Invalid user"));
            }
            var user = await _userManager.FindByEmailAsync(email);
            var response = await _playlistRepository.EnrollToCourse(user.Id ,PlaylistId);
            return Ok(response);
        }
        [HttpGet("getcoursebyid")]
        public async Task<IActionResult> GetCourseById(int PlaylistId)
        {
            var response = await _playlistRepository.GetCourseById(PlaylistId);
            return Ok(response);
        }
    }
}