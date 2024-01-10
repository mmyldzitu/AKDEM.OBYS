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
    public class AppUserSessionConfiguration : IEntityTypeConfiguration<AppUserSession>
    {
        public void Configure(EntityTypeBuilder<AppUserSession> builder)
        {
            builder.HasIndex(x => new
            {
                x.UserId,
                x.SessionId
            }).IsUnique();
            builder.HasOne(x => x.AppUser).WithMany(x => x.AppUserSessions).HasForeignKey(x => x.UserId);
            builder.HasOne(x => x.AppSession).WithMany(x => x.AppUserSessions).HasForeignKey(x => x.SessionId);
            builder.HasOne(x => x.AppBranch).WithMany(x => x.AppUserSessions).HasForeignKey(x => x.BranchId);
        }
    }
}
