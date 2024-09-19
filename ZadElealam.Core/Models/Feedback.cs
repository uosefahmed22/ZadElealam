using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZadElealam.Core.Models
{
    public class Feedback
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string FeedbackMessage { get; set; }
        public int Rating { get; set; }
        public int YouTubePlaylistId { get; set; }
        public YouTubePlaylist YouTubePlaylist { get; set; }
        public DateTime Date { get; set; } = DateTime.Now;
    }
}
