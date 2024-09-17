using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZadElealam.Core.Dto
{
    public class QuestionDto
    {
        public string Text { get; set; }
        public List<AnswerDto> Answers { get; set; }
        public int CorrectAnswerId { get; set; }
    }
}
