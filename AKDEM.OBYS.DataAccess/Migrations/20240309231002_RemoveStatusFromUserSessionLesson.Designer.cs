﻿// <auto-generated />
using System;
using AKDEM.OBYS.DataAccess.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AKDEM.OBYS.DataAccess.Migrations
{
    [DbContext(typeof(AkdemContext))]
    [Migration("20240309231002_RemoveStatusFromUserSessionLesson")]
    partial class RemoveStatusFromUserSessionLesson
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.7")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppBranch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ClassId")
                        .HasColumnType("int");

                    b.Property<string>("Definition")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.HasIndex("ClassId");

                    b.ToTable("AppBranches");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppClass", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Definition")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("AppClasses");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppGraduated", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Date")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("President")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.Property<string>("belgeNo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("studentName")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("year")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AppGraduateds");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppLesson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Definition")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AppLessons");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Definition")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.HasKey("Id");

                    b.ToTable("AppRoles");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppSchedule", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Definition")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("SessionBranchId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("SessionBranchId");

                    b.ToTable("AppSchedules");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppScheduleDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ApScheduleId")
                        .HasColumnType("int");

                    b.Property<string>("Day")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Hours")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("LessonId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("ApScheduleId");

                    b.HasIndex("LessonId");

                    b.ToTable("AppScheduleDetails");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppSession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Definition")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<int>("MinAbsenteeism")
                        .HasColumnType("int");

                    b.Property<double>("MinAverageNote")
                        .HasColumnType("float");

                    b.Property<double>("MinLessonNote")
                        .HasColumnType("float");

                    b.Property<string>("SessionPresident")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<bool>("Status2")
                        .HasColumnType("bit");

                    b.HasKey("Id");

                    b.ToTable("AppSessions");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppSessionBranch", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("BranchId")
                        .HasColumnType("int");

                    b.Property<int>("SessionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BranchId");

                    b.HasIndex("SessionId", "BranchId")
                        .IsUnique();

                    b.ToTable("AppSessionBranches");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("BranchId")
                        .HasColumnType("int");

                    b.Property<int?>("ClassId")
                        .HasColumnType("int");

                    b.Property<string>("DepartReason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("ImagePath")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("SecondName")
                        .IsRequired()
                        .HasMaxLength(200)
                        .HasColumnType("nvarchar(200)");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<int>("SıraNo")
                        .HasColumnType("int");

                    b.Property<double>("TotalAverage")
                        .HasColumnType("float");

                    b.Property<double>("TotalWarningCount")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("BranchId");

                    b.HasIndex("ClassId");

                    b.ToTable("AppUsers");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppUserRole", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("RoleId")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("RoleId");

                    b.HasIndex("UserId", "RoleId")
                        .IsUnique();

                    b.ToTable("AppUserRoles");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppUserSession", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<double>("Average")
                        .HasColumnType("float");

                    b.Property<int>("BranchId")
                        .HasColumnType("int");

                    b.Property<int>("ClassId")
                        .HasColumnType("int");

                    b.Property<double>("SessionAbsentWarningCount")
                        .HasColumnType("float");

                    b.Property<int>("SessionId")
                        .HasColumnType("int");

                    b.Property<double>("SessionLessonWarningCount")
                        .HasColumnType("float");

                    b.Property<double>("SessionWarningCount")
                        .HasColumnType("float");

                    b.Property<bool>("Status")
                        .HasColumnType("bit");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BranchId");

                    b.HasIndex("SessionId");

                    b.HasIndex("UserId", "SessionId")
                        .IsUnique();

                    b.ToTable("AppUserSessions");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppUserSessionLesson", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Devamsızlık")
                        .HasColumnType("int");

                    b.Property<int>("LessonId")
                        .HasColumnType("int");

                    b.Property<int>("Not")
                        .HasColumnType("int");

                    b.Property<int>("UserSessionId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserSessionId");

                    b.HasIndex("LessonId", "UserSessionId")
                        .IsUnique();

                    b.ToTable("AppUserSessionLessons");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppWarning", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("UserSessionId")
                        .HasColumnType("int");

                    b.Property<double>("WarningCount")
                        .HasColumnType("float");

                    b.Property<string>("WarningReason")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("WarningTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime2")
                        .HasDefaultValueSql("getdate()");

                    b.HasKey("Id");

                    b.HasIndex("UserSessionId");

                    b.ToTable("AppWarnings");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppBranch", b =>
                {
                    b.HasOne("AKDEM.OBYS.Entities.AppClass", "AppClass")
                        .WithMany("AppBranches")
                        .HasForeignKey("ClassId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppClass");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppGraduated", b =>
                {
                    b.HasOne("AKDEM.OBYS.Entities.AppUser", "AppUser")
                        .WithMany("AppGraduateds")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppLesson", b =>
                {
                    b.HasOne("AKDEM.OBYS.Entities.AppUser", "AppUser")
                        .WithMany("AppLessons")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppSchedule", b =>
                {
                    b.HasOne("AKDEM.OBYS.Entities.AppSessionBranch", "AppSessionBranch")
                        .WithMany("AppSchedules")
                        .HasForeignKey("SessionBranchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppSessionBranch");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppScheduleDetail", b =>
                {
                    b.HasOne("AKDEM.OBYS.Entities.AppSchedule", "AppSchedule")
                        .WithMany("AppScheduleDetails")
                        .HasForeignKey("ApScheduleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AKDEM.OBYS.Entities.AppLesson", "AppLesson")
                        .WithMany("AppScheduleDetails")
                        .HasForeignKey("LessonId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppLesson");

                    b.Navigation("AppSchedule");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppSessionBranch", b =>
                {
                    b.HasOne("AKDEM.OBYS.Entities.AppBranch", "AppBranch")
                        .WithMany("AppSessionBranches")
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AKDEM.OBYS.Entities.AppSession", "AppSession")
                        .WithMany("AppSessionBranches")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppBranch");

                    b.Navigation("AppSession");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppUser", b =>
                {
                    b.HasOne("AKDEM.OBYS.Entities.AppBranch", "AppBranch")
                        .WithMany("AppUsers")
                        .HasForeignKey("BranchId");

                    b.HasOne("AKDEM.OBYS.Entities.AppClass", "AppClass")
                        .WithMany("AppUsers")
                        .HasForeignKey("ClassId");

                    b.Navigation("AppBranch");

                    b.Navigation("AppClass");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppUserRole", b =>
                {
                    b.HasOne("AKDEM.OBYS.Entities.AppRole", "AppRole")
                        .WithMany("AppUserRoles")
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AKDEM.OBYS.Entities.AppUser", "AppUser")
                        .WithMany("AppUserRoles")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppRole");

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppUserSession", b =>
                {
                    b.HasOne("AKDEM.OBYS.Entities.AppBranch", "AppBranch")
                        .WithMany("AppUserSessions")
                        .HasForeignKey("BranchId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AKDEM.OBYS.Entities.AppSession", "AppSession")
                        .WithMany("AppUserSessions")
                        .HasForeignKey("SessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("AKDEM.OBYS.Entities.AppUser", "AppUser")
                        .WithMany("AppUserSessions")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppBranch");

                    b.Navigation("AppSession");

                    b.Navigation("AppUser");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppUserSessionLesson", b =>
                {
                    b.HasOne("AKDEM.OBYS.Entities.AppLesson", "AppLesson")
                        .WithMany("AppUserSessionLessons")
                        .HasForeignKey("LessonId")
                        .IsRequired();

                    b.HasOne("AKDEM.OBYS.Entities.AppUserSession", "AppUserSession")
                        .WithMany("AppUserSessionLessons")
                        .HasForeignKey("UserSessionId")
                        .IsRequired();

                    b.Navigation("AppLesson");

                    b.Navigation("AppUserSession");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppWarning", b =>
                {
                    b.HasOne("AKDEM.OBYS.Entities.AppUserSession", "AppUserSession")
                        .WithMany("AppWarnings")
                        .HasForeignKey("UserSessionId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AppUserSession");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppBranch", b =>
                {
                    b.Navigation("AppSessionBranches");

                    b.Navigation("AppUsers");

                    b.Navigation("AppUserSessions");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppClass", b =>
                {
                    b.Navigation("AppBranches");

                    b.Navigation("AppUsers");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppLesson", b =>
                {
                    b.Navigation("AppScheduleDetails");

                    b.Navigation("AppUserSessionLessons");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppRole", b =>
                {
                    b.Navigation("AppUserRoles");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppSchedule", b =>
                {
                    b.Navigation("AppScheduleDetails");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppSession", b =>
                {
                    b.Navigation("AppSessionBranches");

                    b.Navigation("AppUserSessions");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppSessionBranch", b =>
                {
                    b.Navigation("AppSchedules");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppUser", b =>
                {
                    b.Navigation("AppGraduateds");

                    b.Navigation("AppLessons");

                    b.Navigation("AppUserRoles");

                    b.Navigation("AppUserSessions");
                });

            modelBuilder.Entity("AKDEM.OBYS.Entities.AppUserSession", b =>
                {
                    b.Navigation("AppUserSessionLessons");

                    b.Navigation("AppWarnings");
                });
#pragma warning restore 612, 618
        }
    }
}
