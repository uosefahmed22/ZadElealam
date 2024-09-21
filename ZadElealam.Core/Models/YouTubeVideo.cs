using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZadElealam.Core.Models
{
    public class YouTubeVideo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string YouTubeVideoId { get; set; }
        public string ThumbnailUrl { get; set; }
        public TimeSpan Duration { get; set; }
        public int PlaylistId { get; set; }
        public YouTubePlaylist Playlist { get; set; }
        public ICollection<StudentVideoProgress> StudentProgresses { get; set; }
    }

}
