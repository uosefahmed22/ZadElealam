using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZadElealam.Core.Dto
{
    public class CreateExamDto
    {
        public string Title { get; set; }
        public int PlaylistId { get; set; }
        public List<QuestionDto> Questions { get; set; }
    }

}
