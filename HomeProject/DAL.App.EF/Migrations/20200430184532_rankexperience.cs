using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class rankexperience : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MinExperience",
                table: "Ranks",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MinExperience",
                table: "Ranks");
        }
    }
}
