using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class indexes1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "GiftCode",
                table: "Gifts",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Ranks_RankTitle",
                table: "Ranks",
                column: "RankTitle");

            migrationBuilder.CreateIndex(
                name: "IX_Gifts_GiftCode",
                table: "Gifts",
                column: "GiftCode");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoles_RoleTitle",
                table: "ChatRoles",
                column: "RoleTitle");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ranks_RankTitle",
                table: "Ranks");

            migrationBuilder.DropIndex(
                name: "IX_Gifts_GiftCode",
                table: "Gifts");

            migrationBuilder.DropIndex(
                name: "IX_ChatRoles_RoleTitle",
                table: "ChatRoles");

            migrationBuilder.DropColumn(
                name: "GiftCode",
                table: "Gifts");
        }
    }
}
