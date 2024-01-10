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

            builder.HasOne(x => x.AppSessionBranch).WithMany(x => x.AppSchedules).HasForeignKey(x => x.SessionBranchId);
        }
    }
}
