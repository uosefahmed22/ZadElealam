using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZadElealam.Core.Models
{
    public class Question
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public ICollection<Answer> Answers { get; set; }
        public int CorrectAnswerId { get; set; }
    }
}
