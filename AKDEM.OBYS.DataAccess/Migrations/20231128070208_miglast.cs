using Microsoft.EntityFrameworkCore.Migrations;

namespace AKDEM.OBYS.DataAccess.Migrations
{
    public partial class miglast : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppSchedules_BranchId",
                table: "AppSchedules");

            migrationBuilder.AddColumn<int>(
                name: "ScheduleId",
                table: "AppBranches",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_AppSchedules_BranchId",
                table: "AppSchedules",
                column: "BranchId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_AppSchedules_BranchId",
                table: "AppSchedules");

            migrationBuilder.DropColumn(
                name: "ScheduleId",
                table: "AppBranches");

            migrationBuilder.CreateIndex(
                name: "IX_AppSchedules_BranchId",
                table: "AppSchedules",
                column: "BranchId");
        }
    }
}
