using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZadElealam.Core.Dto
{
    public class UpdateVideoProgressRequest
    {
        public string StudentId { get; set; }
        public int VideoId { get; set; }
        public TimeSpan WatchedDuration { get; set; }
    }
}
