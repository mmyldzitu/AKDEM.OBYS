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
    public class AppScheduleConfiguration : IEntityTypeConfiguration<AppSchedule>
    {
        public void Configure(EntityTypeBuilder<AppSchedule> builder)
        {
            builder.Property(x => x.Definition).HasMaxLength(200).IsRequired();
            builder.HasOne(x => x.AppBranch).WithOne(x => x.AppSchedule).HasForeignKey<AppSchedule>(x => x.BranchId);
            builder.HasOne(x => x.AppSession).WithMany(x => x.AppSchedules).HasForeignKey(x => x.SessionId);
        }
    }
}
