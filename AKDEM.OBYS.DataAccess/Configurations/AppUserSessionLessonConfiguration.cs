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
    public class AppUserSessionLessonConfiguration : IEntityTypeConfiguration<AppUserSessionLesson>
    {
        public void Configure(EntityTypeBuilder<AppUserSessionLesson> builder)
        {
            builder.HasIndex(x => new
            {
                x.LessonId,
                x.UserSessionId
            }).IsUnique();
            builder.HasOne(x => x.AppUserSession).WithMany(x => x.AppUserSessionLessons).HasForeignKey(x => x.UserSessionId).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(x => x.AppLesson).WithMany(x => x.AppUserSessionLessons).HasForeignKey(x => x.LessonId).OnDelete(DeleteBehavior.ClientSetNull);
        }
    }
}
