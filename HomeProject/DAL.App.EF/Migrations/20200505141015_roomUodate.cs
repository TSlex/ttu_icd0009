using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class roomUodate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "LastMessageDateTime",
                table: "ChatRooms");

            migrationBuilder.DropColumn(
                name: "LastMessageValue",
                table: "ChatRooms");

            migrationBuilder.AddColumn<Guid>(
                name: "ChatRoomImageId",
                table: "ChatRooms",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ChatRoomImageUrl",
                table: "ChatRooms",
                maxLength: 300,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatRoomImageId",
                table: "ChatRooms");

            migrationBuilder.DropColumn(
                name: "ChatRoomImageUrl",
                table: "ChatRooms");

            migrationBuilder.AddColumn<DateTime>(
                name: "LastMessageDateTime",
                table: "ChatRooms",
                type: "datetime(6)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LastMessageValue",
                table: "ChatRooms",
                type: "varchar(100) CHARACTER SET utf8mb4",
                maxLength: 100,
                nullable: true);
        }
    }
}
