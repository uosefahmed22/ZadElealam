using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZadElealam.Core.Models.Auth;

namespace ZadElealam.Core.Models
{
    public class Notification
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Message { get; set; }
        public DateTime Date { get; set; } = DateTime.UtcNow;
        public bool IsRead { get; set; }
        public void MarkAsRead()
        {
            IsRead = true;
        }
    }

}
