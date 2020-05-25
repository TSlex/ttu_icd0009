using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class BlahBlah1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostImageUrl",
                table: "Favorites");

            migrationBuilder.AlterColumn<string>(
                name: "PostTitle",
                table: "Favorites",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4");

            migrationBuilder.AddColumn<Guid>(
                name: "PostImageId",
                table: "Favorites",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PostImageId",
                table: "Favorites");

            migrationBuilder.AlterColumn<string>(
                name: "PostTitle",
                table: "Favorites",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "PostImageUrl",
                table: "Favorites",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true);
        }
    }
}
