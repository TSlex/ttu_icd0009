using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class ProfileGiftsParameters : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "FromProfileId",
                table: "ProfileGifts",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Message",
                table: "ProfileGifts",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ProfileGifts_FromProfileId",
                table: "ProfileGifts",
                column: "FromProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileGifts_Profile_FromProfileId",
                table: "ProfileGifts",
                column: "FromProfileId",
                principalTable: "Profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ProfileGifts_Profile_FromProfileId",
                table: "ProfileGifts");

            migrationBuilder.DropIndex(
                name: "IX_ProfileGifts_FromProfileId",
                table: "ProfileGifts");

            migrationBuilder.DropColumn(
                name: "FromProfileId",
                table: "ProfileGifts");

            migrationBuilder.DropColumn(
                name: "Message",
                table: "ProfileGifts");
        }
    }
}
