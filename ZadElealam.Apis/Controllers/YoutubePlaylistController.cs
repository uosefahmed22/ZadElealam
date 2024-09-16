using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZadElealam.Core.Dto;
using ZadElealam.Core.IRepository;
using ZadElealam.Core.Models;
using ZadElealam.Repository;
using ZadElealam.Repository.Repository;

namespace ZadElealam.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YoutubePlaylistController : ControllerBase
    {
        private readonly IPlaylistRepository _playlistRepository;

        public YoutubePlaylistController(IPlaylistRepository playlistRepository)
        {
            _playlistRepository = playlistRepository;
        }
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

        [HttpDelete("deleteplaylist")]
        public async Task<IActionResult> DeletePlaylist(int playlistId)
        {
            var response = await _playlistRepository.DeletePlaylistAsync(playlistId);
            return Ok(response);
        }

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

    }
}