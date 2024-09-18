using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZadElealam.Core.Dto
{
    public class FeedbackDto
    {
        public string FeedbackMessage { get; set; }
        public int Rating { get; set; }
        public DateTime Date { get; set; }
    }
}
