using AKDEM.OBYS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.DataAccess.Configurations
{
    public class AppLessonConfiguration : IEntityTypeConfiguration<AppLesson>
    {
        public void Configure(EntityTypeBuilder<AppLesson> builder)
        {
            builder.Property(x => x.Definition).HasMaxLength(200).IsRequired();
            builder.HasOne(x => x.AppUser).WithMany(x => x.AppLessons).HasForeignKey(x => x.UserId);
        }
    }
}
