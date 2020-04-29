using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class followermissing : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Watchers_Profile_FollowerProfileId",
                table: "Watchers");

            migrationBuilder.DropForeignKey(
                name: "FK_Watchers_Profile_ProfileId",
                table: "Watchers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Watchers",
                table: "Watchers");

            migrationBuilder.RenameTable(
                name: "Watchers",
                newName: "Follower");

            migrationBuilder.RenameIndex(
                name: "IX_Watchers_ProfileId",
                table: "Follower",
                newName: "IX_Follower_ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Watchers_FollowerProfileId",
                table: "Follower",
                newName: "IX_Follower_FollowerProfileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Follower",
                table: "Follower",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Follower_Profile_FollowerProfileId",
                table: "Follower",
                column: "FollowerProfileId",
                principalTable: "Profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Follower_Profile_ProfileId",
                table: "Follower",
                column: "ProfileId",
                principalTable: "Profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Follower_Profile_FollowerProfileId",
                table: "Follower");

            migrationBuilder.DropForeignKey(
                name: "FK_Follower_Profile_ProfileId",
                table: "Follower");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Follower",
                table: "Follower");

            migrationBuilder.RenameTable(
                name: "Follower",
                newName: "Watchers");

            migrationBuilder.RenameIndex(
                name: "IX_Follower_ProfileId",
                table: "Watchers",
                newName: "IX_Watchers_ProfileId");

            migrationBuilder.RenameIndex(
                name: "IX_Follower_FollowerProfileId",
                table: "Watchers",
                newName: "IX_Watchers_FollowerProfileId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Watchers",
                table: "Watchers",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Watchers_Profile_FollowerProfileId",
                table: "Watchers",
                column: "FollowerProfileId",
                principalTable: "Profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Watchers_Profile_ProfileId",
                table: "Watchers",
                column: "ProfileId",
                principalTable: "Profile",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
