using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class member_room_title : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ChatRoomTitle",
                table: "ChatMembers",
                maxLength: 100,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChatRoomTitle",
                table: "ChatMembers");
        }
    }
}
