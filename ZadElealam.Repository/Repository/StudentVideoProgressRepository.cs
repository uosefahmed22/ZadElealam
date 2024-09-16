using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZadElealam.Core.Models;
using ZadElealam.Repository;

namespace ZadElealam.Core.IRepository
{
    public class StudentVideoProgressRepository
    {
        private readonly AppDbContext _context;

        public StudentVideoProgressRepository(AppDbContext context)
        {
            _context = context;
        }

        
    }
}
