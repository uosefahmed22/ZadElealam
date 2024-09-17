using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZadElealam.Core.Models
{
    public class StudentExam
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public string? StudentName { get; set; }
        public int Score { get; set; }
        public int ExamId { get; set; }
        public Exam Exam { get; set; }
        public bool IsPassed { get; set; }
        public DateTime CompletionDate { get; set; }
    }

}
