using Microsoft.EntityFrameworkCore.Migrations;

namespace AKDEM.OBYS.DataAccess.Migrations
{
    public partial class AddSchedule : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SıraNo",
                table: "AppUsers",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "TotalAverage",
                table: "AppUsers",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<bool>(
                name: "Status2",
                table: "AppSessions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "AppSchedules",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Definition = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SessionId = table.Column<int>(type: "int", nullable: false),
                    BranchId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppSchedules_AppBranches_BranchId",
                        column: x => x.BranchId,
                        principalTable: "AppBranches",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppSchedules_AppSessions_SessionId",
                        column: x => x.SessionId,
                        principalTable: "AppSessions",
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

            migrationBuilder.CreateIndex(
                name: "IX_AppScheduleDetails_ApScheduleId",
                table: "AppScheduleDetails",
                column: "ApScheduleId");

            migrationBuilder.CreateIndex(
                name: "IX_AppScheduleDetails_LessonId",
                table: "AppScheduleDetails",
                column: "LessonId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSchedules_BranchId",
                table: "AppSchedules",
                column: "BranchId");

            migrationBuilder.CreateIndex(
                name: "IX_AppSchedules_SessionId",
                table: "AppSchedules",
                column: "SessionId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppScheduleDetails");

            migrationBuilder.DropTable(
                name: "AppSchedules");

            migrationBuilder.DropColumn(
                name: "SıraNo",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "TotalAverage",
                table: "AppUsers");

            migrationBuilder.DropColumn(
                name: "Status2",
                table: "AppSessions");
        }
    }
}
