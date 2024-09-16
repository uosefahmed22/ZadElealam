using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZadElealam.Core.Models;
using ZadElealam.Core.Models.Auth;

namespace ZadElealam.Repository
{
    public class AppDbContext : IdentityDbContext<AppUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> dbContextOptions) : base(dbContextOptions)
        {
        }
        public DbSet<RefreshToken> RefreshTokens { get; set; }
        public DbSet<YouTubePlaylist> YouTubePlaylists { get; set; }
        public DbSet<YouTubeVideo> YouTubeVideos { get; set; }
        public DbSet<StudentVideoProgress> StudentVideoProgresses { get; set; }
        public DbSet<Category> Categories { get; set; }
    }
}
