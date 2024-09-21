using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZadElealam.Core.Models.Auth;

namespace ZadElealam.Core.Models
{
    public class Enrollment
    {
        public int Id { get; set; }
        public int PlayListId { get; set; }
        public YouTubePlaylist PlayList { get; set; }
        public string UserId { get; set; }
        public AppUser User { get; set; }
    }
}
