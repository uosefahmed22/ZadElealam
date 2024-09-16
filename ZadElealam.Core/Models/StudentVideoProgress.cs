using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZadElealam.Core.Models
{
    public class StudentVideoProgress
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public int VideoId { get; set; }
        public YouTubeVideo Video { get; set; }
        public TimeSpan WatchedDuration { get; set; }
        public bool IsCompleted { get; set; }
        public DateTime? CompletionDate { get; set; }
    }

}
