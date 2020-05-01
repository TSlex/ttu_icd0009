using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class gifts : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "ProfileGifts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Price",
                table: "Gifts",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PostDescription",
                table: "Favorites",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostImageUrl",
                table: "Favorites",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostTitle",
                table: "Favorites",
                nullable: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Price",
                table: "ProfileGifts");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "PostDescription",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "PostImageUrl",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "PostTitle",
                table: "Favorites");
        }
    }
}
