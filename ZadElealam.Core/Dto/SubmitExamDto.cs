using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZadElealam.Core.Dto
{
    public class SubmitExamDto
    {
        public string? StudentId { get; set; }
        public Dictionary<int, int> StudentAnswers { get; set; }
    }
}
