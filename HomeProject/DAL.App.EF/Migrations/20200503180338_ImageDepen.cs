using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class ImageDepen : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Images_PostImageId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_PostImageId",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "PostImageId",
                table: "Favorites");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PostImageId",
                table: "Favorites",
                type: "char(36)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_PostImageId",
                table: "Favorites",
                column: "PostImageId");

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Images_PostImageId",
                table: "Favorites",
                column: "PostImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
