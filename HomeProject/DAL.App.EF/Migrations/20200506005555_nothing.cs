using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class nothing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MessageValue",
                table: "Messages",
                maxLength: 3000,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(2000) CHARACTER SET utf8mb4",
                oldMaxLength: 2000);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "MessageValue",
                table: "Messages",
                type: "varchar(2000) CHARACTER SET utf8mb4",
                maxLength: 2000,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 3000);
        }
    }
}
