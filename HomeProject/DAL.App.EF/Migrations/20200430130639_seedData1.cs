using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class seedData1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ranks_RankTitle",
                table: "Ranks");

            migrationBuilder.AlterColumn<string>(
                name: "RankIcon",
                table: "Ranks",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(20) CHARACTER SET utf8mb4",
                oldMaxLength: 20);

            migrationBuilder.AddColumn<string>(
                name: "RankCode",
                table: "Ranks",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Ranks_RankCode",
                table: "Ranks",
                column: "RankCode");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Ranks_RankCode",
                table: "Ranks");

            migrationBuilder.DropColumn(
                name: "RankCode",
                table: "Ranks");

            migrationBuilder.AlterColumn<string>(
                name: "RankIcon",
                table: "Ranks",
                type: "varchar(20) CHARACTER SET utf8mb4",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Ranks_RankTitle",
                table: "Ranks",
                column: "RankTitle");
        }
    }
}
