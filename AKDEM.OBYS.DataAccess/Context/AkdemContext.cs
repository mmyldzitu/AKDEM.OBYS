using AKDEM.OBYS.DataAccess.Configurations;
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
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AppBranchConfiguration());
            modelBuilder.ApplyConfiguration(new AppClassConfiguration());
            modelBuilder.ApplyConfiguration(new AppLessonConfiguration());
            modelBuilder.ApplyConfiguration(new AppRoleConfiguration());
            modelBuilder.ApplyConfiguration(new AppSessionConfiguration());
            modelBuilder.ApplyConfiguration(new AppUserConfiguration());
            modelBuilder.ApplyConfiguration(new AppUserRoleConfiguration());
            modelBuilder.ApplyConfiguration(new AppUserSessionConfiguration());
            modelBuilder.ApplyConfiguration(new AppUserSessionLessonConfiguration());
            modelBuilder.ApplyConfiguration(new AppWarningConfiguration());
            modelBuilder.ApplyConfiguration(new AppScheduleConfiguration());
            modelBuilder.ApplyConfiguration(new AppScheduleDetailConfiguration());
            modelBuilder.ApplyConfiguration(new AppSessionBranchConfiguration());
            modelBuilder.ApplyConfiguration(new AppGraduatedConfiguration());
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
        public DbSet<AppSchedule> AppSchedules { get; set; }
        public DbSet<AppScheduleDetail> AppScheduleDetails { get; set; }
        public DbSet<AppSessionBranch> AppSessionBranches { get; set; }
        public DbSet<AppGraduated> AppGraduateds { get; set; }

    }
}
