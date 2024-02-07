using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AKDEM.OBYS.DataAccess.Migrations
{
    public partial class lastMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppClasses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Definition = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppClasses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Definition = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Definition = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    MinAverageNote = table.Column<double>(type: "float", nullable: false),
                    MinLessonNote = table.Column<double>(type: "float", nullable: false),
                    MinAbsenteeism = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    Status2 = table.Column<bool>(type: "bit", nullable: false),
                    SessionPresident = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSessions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppBranches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Definition = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppBranches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppBranches_AppClasses_ClassId",
                        column: x => x.ClassId,
                        principalTable: "AppClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppSessionBranches",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSessionBranches", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSessionBranches_AppBranches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "AppBranches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppSessionBranches_AppSessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "AppSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUsers",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    FirstName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SecondName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Status = table.Column<bool>(type: "bit", nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalAverage = table.Column<double>(type: "float", nullable: false),
                    SıraNo = table.Column<int>(type: "int", nullable: false),
                    DepartReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TotalWarningCount = table.Column<double>(type: "float", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: true),
                    ClassId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUsers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUsers_AppBranches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "AppBranches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppUsers_AppClasses_ClassId",
                        column: x => x.ClassId,
                        principalTable: "AppClasses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Definition = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SessionBranchId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSchedules_AppSessionBranches_SessionBranchId",
                        column: x => x.SessionBranchId,
                        principalTable: "AppSessionBranches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppGraduateds",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    President = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    year = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    studentName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    belgeNo = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppGraduateds", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppGraduateds_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppLessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Definition = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppLessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppLessons_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserRoles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    RoleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserRoles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUserRoles_AppRoles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "AppRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserRoles_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserSessions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Average = table.Column<double>(type: "float", nullable: false),
                    SessionWarningCount = table.Column<double>(type: "float", nullable: false),
                    SessionLessonWarningCount = table.Column<double>(type: "float", nullable: false),
                    SessionAbsentWarningCount = table.Column<double>(type: "float", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false),
                    ClassId = table.Column<int>(type: "int", nullable: false),
                    Status = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserSessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUserSessions_AppBranches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "AppBranches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserSessions_AppSessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "AppSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppUserSessions_AppUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AppUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppScheduleDetails",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Day = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hours = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LessonId = table.Column<int>(type: "int", nullable: false),
                    ApScheduleId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppScheduleDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppScheduleDetails_AppLessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "AppLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppScheduleDetails_AppSchedules_ApScheduleId",
                        column: x => x.ApScheduleId,
                        principalTable: "AppSchedules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AppUserSessionLessons",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    LessonId = table.Column<int>(type: "int", nullable: false),
                    UserSessionId = table.Column<int>(type: "int", nullable: false),
                    Not = table.Column<int>(type: "int", nullable: false),
                    Devamsızlık = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppUserSessionLessons", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppUserSessionLessons_AppLessons_LessonId",
                        column: x => x.LessonId,
                        principalTable: "AppLessons",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_AppUserSessionLessons_AppUserSessions_UserSessionId",
                        column: x => x.UserSessionId,
                        principalTable: "AppUserSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppWarnings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WarningCount = table.Column<double>(type: "float", nullable: false),
                    WarningTime = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "getdate()"),
                    WarningReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserSessionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppWarnings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppWarnings_AppUserSessions_UserSessionId",
                        column: x => x.UserSessionId,
                        principalTable: "AppUserSessions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppBranches_ClassId",
                table: "AppBranches",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_AppGraduateds_UserId",
                table: "AppGraduateds",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppLessons_UserId",
                table: "AppLessons",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AppScheduleDetails_ApScheduleId",
                table: "AppScheduleDetails",
                column: "ApScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_AppScheduleDetails_LessonId",
                table: "AppScheduleDetails",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSchedules_SessionBranchId",
                table: "AppSchedules",
                column: "SessionBranchId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSessionBranches_BranchId",
                table: "AppSessionBranches",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSessionBranches_SessionId_BranchId",
                table: "AppSessionBranches",
                columns: new[] { "SessionId", "BranchId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUserRoles_RoleId",
                table: "AppUserRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserRoles_UserId_RoleId",
                table: "AppUserRoles",
                columns: new[] { "UserId", "RoleId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_BranchId",
                table: "AppUsers",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUsers_ClassId",
                table: "AppUsers",
                column: "ClassId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserSessionLessons_LessonId_UserSessionId",
                table: "AppUserSessionLessons",
                columns: new[] { "LessonId", "UserSessionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppUserSessionLessons_UserSessionId",
                table: "AppUserSessionLessons",
                column: "UserSessionId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserSessions_BranchId",
                table: "AppUserSessions",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserSessions_SessionId",
                table: "AppUserSessions",
                column: "SessionId");

            migrationBuilder.CreateIndex(
                name: "IX_AppUserSessions_UserId_SessionId",
                table: "AppUserSessions",
                columns: new[] { "UserId", "SessionId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AppWarnings_UserSessionId",
                table: "AppWarnings",
                column: "UserSessionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppGraduateds");

            migrationBuilder.DropTable(
                name: "AppScheduleDetails");

            migrationBuilder.DropTable(
                name: "AppUserRoles");

            migrationBuilder.DropTable(
                name: "AppUserSessionLessons");

            migrationBuilder.DropTable(
                name: "AppWarnings");

            migrationBuilder.DropTable(
                name: "AppSchedules");

            migrationBuilder.DropTable(
                name: "AppRoles");

            migrationBuilder.DropTable(
                name: "AppLessons");

            migrationBuilder.DropTable(
                name: "AppUserSessions");

            migrationBuilder.DropTable(
                name: "AppSessionBranches");

            migrationBuilder.DropTable(
                name: "AppUsers");

            migrationBuilder.DropTable(
                name: "AppSessions");

            migrationBuilder.DropTable(
                name: "AppBranches");

            migrationBuilder.DropTable(
                name: "AppClasses");
        }
    }
}
