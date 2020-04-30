using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class rankImprovement1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "MaxExperience",
                table: "Ranks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "NextRankId",
                table: "Ranks",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PreviousRankId",
                table: "Ranks",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RankColor",
                table: "Ranks",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RankIcon",
                table: "Ranks",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "RankTextColor",
                table: "Ranks",
                maxLength: 20,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "Experience",
                table: "ProfileRanks",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Ranks_NextRankId",
                table: "Ranks",
                column: "NextRankId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ranks_PreviousRankId",
                table: "Ranks",
                column: "PreviousRankId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Ranks_Ranks_NextRankId",
                table: "Ranks",
                column: "NextRankId",
                principalTable: "Ranks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Ranks_Ranks_PreviousRankId",
                table: "Ranks",
                column: "PreviousRankId",
                principalTable: "Ranks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Ranks_Ranks_NextRankId",
                table: "Ranks");

            migrationBuilder.DropForeignKey(
                name: "FK_Ranks_Ranks_PreviousRankId",
                table: "Ranks");

            migrationBuilder.DropIndex(
                name: "IX_Ranks_NextRankId",
                table: "Ranks");

            migrationBuilder.DropIndex(
                name: "IX_Ranks_PreviousRankId",
                table: "Ranks");

            migrationBuilder.DropColumn(
                name: "MaxExperience",
                table: "Ranks");

            migrationBuilder.DropColumn(
                name: "NextRankId",
                table: "Ranks");

            migrationBuilder.DropColumn(
                name: "PreviousRankId",
                table: "Ranks");

            migrationBuilder.DropColumn(
                name: "RankColor",
                table: "Ranks");

            migrationBuilder.DropColumn(
                name: "RankIcon",
                table: "Ranks");

            migrationBuilder.DropColumn(
                name: "RankTextColor",
                table: "Ranks");

            migrationBuilder.DropColumn(
                name: "Experience",
                table: "ProfileRanks");
        }
    }
}
