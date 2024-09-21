using Google.Apis.Services;
using Google.Apis.YouTube.v3.Data;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Channels;
using System.Threading.Tasks;
using System.Xml;
using ZadElealam.Core.Models;

namespace ZadElealam.Repository.Repository
{
    public class YouTubeService
    {
        private readonly Google.Apis.YouTube.v3.YouTubeService _youtubeService;

        public YouTubeService(string apiKey)
        {
            _youtubeService = new Google.Apis.YouTube.v3.YouTubeService(new Google.Apis.Services.BaseClientService.Initializer()
            {
                ApiKey = apiKey,
                ApplicationName = "ZadElealam"
            });
        }
        public async Task<YouTubePlaylist> GetPlaylistAsync(string playlistId)
        {
            var playlistRequest = _youtubeService.Playlists.List("snippet");
            playlistRequest.Id = playlistId;

            var playlistResponse = await playlistRequest.ExecuteAsync();
            var playlist = playlistResponse.Items.FirstOrDefault();

            if (playlist == null)
            {
                return null;
            }

            return new YouTubePlaylist
            {
                Title = playlist.Snippet.Title,
                Description = playlist.Snippet.Description,
                YouTubePlaylistId = playlist.Id,
            };
        }
        public async Task<List<YouTubeVideo>> GetVideosFromPlaylistAsync(string playlistId)
        {
            var videos = new List<YouTubeVideo>();
            var nextPageToken = "";

            while (nextPageToken != null)
            {
                var playlistItemsRequest = _youtubeService.PlaylistItems.List("snippet,contentDetails");//_youtubeService doesn't exist in the current context
                playlistItemsRequest.PlaylistId = playlistId;
                playlistItemsRequest.MaxResults = 50;
                playlistItemsRequest.PageToken = nextPageToken;

                var playlistItemsResponse = await playlistItemsRequest.ExecuteAsync();

                foreach (var playlistItem in playlistItemsResponse.Items)
                {
                    videos.Add(new YouTubeVideo
                    {
                        Title = playlistItem.Snippet.Title,
                        Description = playlistItem.Snippet.Description,
                        YouTubeVideoId = playlistItem.ContentDetails.VideoId,
                        Url = $"https://www.youtube.com/watch?v={playlistItem.ContentDetails.VideoId}",
                        ThumbnailUrl = playlistItem.Snippet.Thumbnails.Default__.Url,
                        Duration = await GetVideoDurationAsync(playlistItem.ContentDetails.VideoId)
                    });
                }

                nextPageToken = playlistItemsResponse.NextPageToken;
            }

            return videos;
        }
        private async Task<TimeSpan> GetVideoDurationAsync(string videoId)
        {
            var videoRequest = _youtubeService.Videos.List("contentDetails");
            videoRequest.Id = videoId;

            var videoResponse = await videoRequest.ExecuteAsync();
            var video = videoResponse.Items.FirstOrDefault();

            if (video == null)
            {
                return TimeSpan.Zero;
            }

            var duration = XmlConvert.ToTimeSpan(video.ContentDetails.Duration);
            return duration;
        }
    }
}
