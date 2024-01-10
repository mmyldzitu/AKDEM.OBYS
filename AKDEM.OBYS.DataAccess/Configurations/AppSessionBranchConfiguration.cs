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
    public class AppSessionBranchConfiguration : IEntityTypeConfiguration<AppSessionBranch>
    {
        public void Configure(EntityTypeBuilder<AppSessionBranch> builder)
        {
            builder.HasIndex(x => new
            {
                x.SessionId,
                x.BranchId
               
            }).IsUnique();

          
            builder.HasOne(x => x.AppSession).WithMany(x => x.AppSessionBranches).HasForeignKey(x => x.SessionId);
            builder.HasOne(x => x.AppBranch).WithMany(x => x.AppSessionBranches).HasForeignKey(x => x.BranchId);
        }
    }
}
