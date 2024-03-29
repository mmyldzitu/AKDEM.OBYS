﻿using AKDEM.OBYS.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.DataAccess.Configurations
{
    public class AppGraduatedConfiguration : IEntityTypeConfiguration<AppGraduated>
    {
        public void Configure(EntityTypeBuilder<AppGraduated> builder)
        {
            builder.HasOne(x => x.AppUser).WithMany(x => x.AppGraduateds).HasForeignKey(x => x.UserId);
        }
    }
}
