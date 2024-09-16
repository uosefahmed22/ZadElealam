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
        public string VideoUrl { get; set; }
        public string Title { get; set; }
        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        [ForeignKey("YouTubePlaylistId")]
        public int YouTubePlaylistId { get; set; }
        public YouTubePlaylist Playlist { get; set; }
    }

}
