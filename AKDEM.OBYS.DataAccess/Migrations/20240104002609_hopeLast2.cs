using Microsoft.EntityFrameworkCore.Migrations;

namespace AKDEM.OBYS.DataAccess.Migrations
{
    public partial class hopeLast2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Status",
                table: "AppUserSessions",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "AppUserSessions");
        }
    }
}
