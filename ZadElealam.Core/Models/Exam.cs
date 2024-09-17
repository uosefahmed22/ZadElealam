using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZadElealam.Core.Models
{
    public class Exam
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int PlaylistId { get; set; }

        [System.Text.Json.Serialization.JsonIgnore]
        public YouTubePlaylist Playlist { get; set; }

        public ICollection<Question> Questions { get; set; }
    }

}
