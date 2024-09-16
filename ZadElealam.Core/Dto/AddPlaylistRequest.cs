using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZadElealam.Core.Dto
{

    public class AddPlaylistRequest
    {
        public string PlaylistUrl { get; set; }
        public int CategoryId { get; set; }
    }
}
