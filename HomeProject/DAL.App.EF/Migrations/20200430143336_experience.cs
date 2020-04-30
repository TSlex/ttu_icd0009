using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class experience : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Experience",
                table: "ProfileRanks");

            migrationBuilder.AddColumn<int>(
                name: "Experience",
                table: "Profile",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Experience",
                table: "Profile");

            migrationBuilder.AddColumn<int>(
                name: "Experience",
                table: "ProfileRanks",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
