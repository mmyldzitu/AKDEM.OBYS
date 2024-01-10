using Microsoft.EntityFrameworkCore.Migrations;

namespace AKDEM.OBYS.DataAccess.Migrations
{
    public partial class hopeLast : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Status",
                table: "AppBranches",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Status",
                table: "AppBranches");
        }
    }
}
