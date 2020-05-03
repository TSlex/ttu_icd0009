using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class Image4 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileGender",
                table: "Images");

            migrationBuilder.AddColumn<int>(
                name: "ImageType",
                table: "Images",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageType",
                table: "Images");

            migrationBuilder.AddColumn<int>(
                name: "ProfileGender",
                table: "Images",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}
