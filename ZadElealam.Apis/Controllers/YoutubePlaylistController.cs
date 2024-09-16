using Google.Apis.Services;
using Google.Apis.YouTube.v3;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ZadElealam.Core.Dto;
using ZadElealam.Core.IServices;
using ZadElealam.Core.Models;
using ZadElealam.Repository;

namespace ZadElealam.Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class YoutubePlaylistController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly IYoutubePlaylistsServices _youtubePlaylistsServices;
        private readonly AppDbContext _context;

        public YoutubePlaylistController(IConfiguration configuration,
            IYoutubePlaylistsServices youtubePlaylistsServices,
            AppDbContext context)
        {
            _configuration = configuration;
            _youtubePlaylistsServices = youtubePlaylistsServices;
            _context = context;
        }

        [HttpPost("add-playlist")]
        public async Task<IActionResult> AddPlaylist([FromBody] AddPlaylistRequest request)
        {
            var playlistId = ExtractPlaylistIdFromUrl(request.PlaylistUrl);

            // Fetch videos from the playlist
            var videos = await _youtubePlaylistsServices.GetPlaylistVideosAsync(playlistId);

            // Create playlist and associate it with the category
            var playlist = new YouTubePlaylist
            {
                PlaylistId = playlistId,
                Title = "Playlist Title", // Fetch title if needed
                Description = "Playlist Description", // Fetch description if needed
                PublishedAt = DateTime.Now, // Set proper date
                CategoryId = request.CategoryId,
                Videos = videos
            };

            _context.YouTubePlaylists.Add(playlist);
            await _context.SaveChangesAsync();

            return Ok(playlist);
        }

        // Get videos by PlaylistId
        [HttpGet("playlists/{id}/videos")]
        public IActionResult GetVideosByPlaylistId(int id)
        {
            var videos = _context.YouTubeVideos.Where(v => v.YouTubePlaylistId == id).ToList();
            return Ok(videos);
        }
        //create category
        [HttpPost("add-category")]
        public async Task<IActionResult> AddCategory([FromBody] CategoryDto category)
        {
            var response = await _youtubePlaylistsServices.AddCategoryAsync(category);
            return Ok(response);
        }
        //get all categories
        [HttpGet("categories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _youtubePlaylistsServices.GetCategoriesAsync();
            return Ok(categories);
        }
        //get GetVideosByPlaylistIdAsync
        [HttpGet("get-VideosByPlaylistId")]
        public async Task<IActionResult> GetVideosByPlaylistIdAsync(int id)
        {
            var videos = await _youtubePlaylistsServices.GetVideosByPlaylistIdAsync(id);
            return Ok(videos);
        }







        private string ExtractPlaylistIdFromUrl(string url)
        {
            var uri = new Uri(url);
            var query = System.Web.HttpUtility.ParseQueryString(uri.Query);
            return query["list"];
        }


    }
}
