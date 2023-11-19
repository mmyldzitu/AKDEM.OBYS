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
    public class AppUserConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.Property(x => x.FirstName).HasMaxLength(200).IsRequired();
            builder.Property(x => x.SecondName).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Email).HasMaxLength(200).IsRequired();
            builder.Property(x => x.Password).HasMaxLength(200).IsRequired();
            // builder.Property(x => x.ImagePath).HasMaxLength(500).IsRequired();
            builder.HasOne(x => x.AppBranch).WithMany(x => x.AppUsers).HasForeignKey(x => x.BranchId).OnDelete(DeleteBehavior.ClientSetNull);
            builder.HasOne(x => x.AppClass).WithMany(x => x.AppUsers).HasForeignKey(x => x.ClassId).OnDelete(DeleteBehavior.ClientSetNull);

        }
    }
}
