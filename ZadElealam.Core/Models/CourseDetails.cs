using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZadElealam.Core.Models
{
    public class CourseDetails
    {
        public int Id { get; set; }
        public int VideosCount { get; set; }
        public int EnrollmentsCount { get; set; }
        public float Rating { get; set; }
        public YouTubePlaylist YouTubePlaylist{ get; set; }
        public int YouTubePlaylistId { get; set; }
    }
}
