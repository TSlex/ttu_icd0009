using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class indexfixunique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ranks_RankCode",
                table: "Ranks");

            migrationBuilder.DropIndex(
                name: "IX_Gifts_GiftCode",
                table: "Gifts");

            migrationBuilder.DropIndex(
                name: "IX_ChatRoles_RoleTitle",
                table: "ChatRoles");

            migrationBuilder.CreateIndex(
                name: "IX_Ranks_RankCode",
                table: "Ranks",
                column: "RankCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Gifts_GiftCode",
                table: "Gifts",
                column: "GiftCode",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoles_RoleTitle",
                table: "ChatRoles",
                column: "RoleTitle",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ranks_RankCode",
                table: "Ranks");

            migrationBuilder.DropIndex(
                name: "IX_Gifts_GiftCode",
                table: "Gifts");

            migrationBuilder.DropIndex(
                name: "IX_ChatRoles_RoleTitle",
                table: "ChatRoles");

            migrationBuilder.CreateIndex(
                name: "IX_Ranks_RankCode",
                table: "Ranks",
                column: "RankCode");

            migrationBuilder.CreateIndex(
                name: "IX_Gifts_GiftCode",
                table: "Gifts",
                column: "GiftCode");

            migrationBuilder.CreateIndex(
                name: "IX_ChatRoles_RoleTitle",
                table: "ChatRoles",
                column: "RoleTitle");
        }
    }
}
