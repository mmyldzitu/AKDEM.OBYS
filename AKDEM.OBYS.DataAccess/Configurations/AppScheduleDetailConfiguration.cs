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
    public class AppScheduleDetailConfiguration : IEntityTypeConfiguration<AppScheduleDetail>
    {
        public void Configure(EntityTypeBuilder<AppScheduleDetail> builder)
        {
            builder.HasOne(x => x.AppLesson).WithMany(x => x.AppScheduleDetails).HasForeignKey(x => x.LessonId);
            builder.HasOne(x => x.AppSchedule).WithMany(x => x.AppScheduleDetails).HasForeignKey(x => x.ApScheduleId);
        }
    }
}
