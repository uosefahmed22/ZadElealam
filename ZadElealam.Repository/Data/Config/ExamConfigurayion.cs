using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ZadElealam.Core.Models;

namespace ZadElealam.Repository.Data.Config
{
    public class ExamConfiguration : IEntityTypeConfiguration<Exam>
    {
        public void Configure(EntityTypeBuilder<Exam> builder)
        {
            builder.HasMany(e => e.Questions)
                   .WithOne()
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
