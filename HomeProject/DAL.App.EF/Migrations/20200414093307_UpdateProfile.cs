using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class UpdateProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileFullName",
                table: "Profile",
                maxLength: 100,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ProfileGender",
                table: "Profile",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileGenderOwn",
                table: "Profile",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ProfileWorkPlace",
                table: "Profile",
                maxLength: 300,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileFullName",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "ProfileGender",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "ProfileGenderOwn",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "ProfileWorkPlace",
                table: "Profile");
        }
    }
}
