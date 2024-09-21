using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
        public string YouTubePlaylistId { get; set; }
        public string? ThumbnailUrl { get; set; }
        public int VideosCount { get; set; }
        public float CourseHours { get; set; }
        public ICollection<YouTubeVideo> Videos { get; set; }
        public int CategoryId { get; set; }
        public Category Category { get; set; }
        public ICollection<Exam> Exams { get; set; }
    }
}
