using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class Image2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Images",
                maxLength: 300,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(300) CHARACTER SET utf8mb4",
                oldMaxLength: 300,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OriginalImageUrl",
                table: "Images",
                maxLength: 300,
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalImageUrl",
                table: "Images");

            migrationBuilder.AlterColumn<string>(
                name: "ImageUrl",
                table: "Images",
                type: "varchar(300) CHARACTER SET utf8mb4",
                maxLength: 300,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 300);
        }
    }
}
