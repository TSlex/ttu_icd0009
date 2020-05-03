using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class ImageDepe : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProfileAvatarId",
                table: "Profile",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PostImageId",
                table: "Posts",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "GiftImageId",
                table: "Gifts",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PostImageId",
                table: "Favorites",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Profile_ProfileAvatarId",
                table: "Profile",
                column: "ProfileAvatarId");

            migrationBuilder.CreateIndex(
                name: "IX_Posts_PostImageId",
                table: "Posts",
                column: "PostImageId");

            migrationBuilder.CreateIndex(
                name: "IX_Gifts_GiftImageId",
                table: "Gifts",
                column: "GiftImageId");

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

            migrationBuilder.AddForeignKey(
                name: "FK_Gifts_Images_GiftImageId",
                table: "Gifts",
                column: "GiftImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Posts_Images_PostImageId",
                table: "Posts",
                column: "PostImageId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Profile_Images_ProfileAvatarId",
                table: "Profile",
                column: "ProfileAvatarId",
                principalTable: "Images",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Images_PostImageId",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Gifts_Images_GiftImageId",
                table: "Gifts");

            migrationBuilder.DropForeignKey(
                name: "FK_Posts_Images_PostImageId",
                table: "Posts");

            migrationBuilder.DropForeignKey(
                name: "FK_Profile_Images_ProfileAvatarId",
                table: "Profile");

            migrationBuilder.DropIndex(
                name: "IX_Profile_ProfileAvatarId",
                table: "Profile");

            migrationBuilder.DropIndex(
                name: "IX_Posts_PostImageId",
                table: "Posts");

            migrationBuilder.DropIndex(
                name: "IX_Gifts_GiftImageId",
                table: "Gifts");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_PostImageId",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "ProfileAvatarId",
                table: "Profile");

            migrationBuilder.DropColumn(
                name: "PostImageId",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "GiftImageId",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "PostImageId",
                table: "Favorites");
        }
    }
}
