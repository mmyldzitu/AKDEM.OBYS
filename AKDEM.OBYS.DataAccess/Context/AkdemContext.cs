using AKDEM.OBYS.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AKDEM.OBYS.DataAccess.Context
{
    public class AkdemContext:DbContext
    {
        public AkdemContext(DbContextOptions<AkdemContext> options) :base(options)
        {

        }
        public DbSet<AppBranch> AppBranches { get; set; }
        public DbSet<AppClass> AppClasses { get; set; }
        public DbSet<AppLesson> AppLessons { get; set; }
        public DbSet<AppRole> AppRoles { get; set; }
        public DbSet<AppSession> AppSessions { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<AppUserRole> AppUserRoles { get; set; }
        public DbSet<AppUserSession> AppUserSessions { get; set; }
        public DbSet<AppUserSessionLesson> AppUserSessionLessons { get; set; }
        public DbSet<AppWarning> AppWarnings { get; set; }
        
    }
}
