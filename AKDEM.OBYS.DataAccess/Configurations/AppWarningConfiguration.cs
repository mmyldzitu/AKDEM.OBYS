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
    public class AppWarningConfiguration : IEntityTypeConfiguration<AppWarning>
    {
        public void Configure(EntityTypeBuilder<AppWarning> builder)
        {
            builder.Property(x => x.WarningTime).HasDefaultValueSql("getdate()");
            builder.HasOne(x => x.AppUserSession).WithMany(x => x.AppWarnings).HasForeignKey(x => x.UserSessionId);
        }
    }
}
