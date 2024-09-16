using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZadElealam.Core.Models
{
    public class YouTubePlaylist
    {
        public int YouTubePlaylistId { get; set; }
        public string PlaylistId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime PublishedAt { get; set; }
        public string? ImageUrl { get; set; }
        [NotMapped]
        public IFormFile Image { get; set; }
        public List<YouTubeVideo> Videos { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
    }
}
