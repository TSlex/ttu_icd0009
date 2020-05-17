using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class NoURL : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfileAvatarUrl",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "PostImageUrl",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "GiftImageUrl",
                table: "Gifts");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfileAvatarUrl",
                table: "Profile",
                type: "varchar(300) CHARACTER SET utf8mb4",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostImageUrl",
                table: "Posts",
                type: "varchar(300) CHARACTER SET utf8mb4",
                maxLength: 300,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "GiftImageUrl",
                table: "Gifts",
                type: "varchar(300) CHARACTER SET utf8mb4",
                maxLength: 300,
                nullable: true);
        }
    }
}
