using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace DAL.Migrations
{
    public partial class RepoPattern : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_UserRole_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_UserRole_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMembers_ChatRoles_ChatRoleId",
                table: "ChatMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMembers_Rooms_ChatRoomId",
                table: "ChatMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Posts_PostId",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Rooms_ChatRoomId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileGifts_Gifts_GiftId",
                table: "ProfileGifts");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileRanks_Ranks_RankId",
                table: "ProfileRanks");

            migrationBuilder.DropIndex(
                name: "IX_ProfileRanks_RankId",
                table: "ProfileRanks");

            migrationBuilder.DropIndex(
                name: "IX_ProfileGifts_GiftId",
                table: "ProfileGifts");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ChatRoomId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_PostId",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Comments_PostId",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_ChatMembers_ChatRoleId",
                table: "ChatMembers");

            migrationBuilder.DropIndex(
                name: "IX_ChatMembers_ChatRoomId",
                table: "ChatMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserRole",
                table: "UserRole");

            migrationBuilder.RenameTable(
                name: "UserRole",
                newName: "AspNetRoles");

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "Watchers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Watchers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Watchers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangedAt",
                table: "Watchers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ChangedBy",
                table: "Watchers",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "Rooms",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Rooms",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Rooms",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangedAt",
                table: "Rooms",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ChangedBy",
                table: "Rooms",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "Ranks",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Ranks",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Ranks",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangedAt",
                table: "Ranks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ChangedBy",
                table: "Ranks",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "ProfileRanks",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "ProfileRanks",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "ProfileRanks",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangedAt",
                table: "ProfileRanks",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ChangedBy",
                table: "ProfileRanks",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "RankId1",
                table: "ProfileRanks",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "ProfileGifts",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "ProfileGifts",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "ProfileGifts",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangedAt",
                table: "ProfileGifts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ChangedBy",
                table: "ProfileGifts",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "GiftId1",
                table: "ProfileGifts",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "Posts",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Posts",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Posts",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangedAt",
                table: "Posts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ChangedBy",
                table: "Posts",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "Messages",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Messages",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Messages",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangedAt",
                table: "Messages",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ChangedBy",
                table: "Messages",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ChatRoomId1",
                table: "Messages",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "Gifts",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Gifts",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Gifts",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangedAt",
                table: "Gifts",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ChangedBy",
                table: "Gifts",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "Favorites",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Favorites",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Favorites",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangedAt",
                table: "Favorites",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ChangedBy",
                table: "Favorites",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "PostId1",
                table: "Favorites",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "Comments",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Comments",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "Comments",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangedAt",
                table: "Comments",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ChangedBy",
                table: "Comments",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "PostId1",
                table: "Comments",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "ChatRoles",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "ChatRoles",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "ChatRoles",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangedAt",
                table: "ChatRoles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ChangedBy",
                table: "ChatRoles",
                nullable: false);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "ChatMembers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "ChatMembers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "ChatMembers",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangedAt",
                table: "ChatMembers",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ChangedBy",
                table: "ChatMembers",
                nullable: false);

            migrationBuilder.AddColumn<Guid>(
                name: "ChatRoleId1",
                table: "ChatMembers",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ChatRoomId1",
                table: "ChatMembers",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "BlockedProfiles",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "BlockedProfiles",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "longtext CHARACTER SET utf8mb4",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "Id",
                table: "BlockedProfiles",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "varchar(255) CHARACTER SET utf8mb4");

            migrationBuilder.AddColumn<DateTime>(
                name: "ChangedAt",
                table: "BlockedProfiles",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "ChangedBy",
                table: "BlockedProfiles",
                nullable: false);

            migrationBuilder.AddPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileRanks_RankId1",
                table: "ProfileRanks",
                column: "RankId1");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileGifts_GiftId1",
                table: "ProfileGifts",
                column: "GiftId1");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatRoomId1",
                table: "Messages",
                column: "ChatRoomId1");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_PostId1",
                table: "Favorites",
                column: "PostId1");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId1",
                table: "Comments",
                column: "PostId1");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMembers_ChatRoleId1",
                table: "ChatMembers",
                column: "ChatRoleId1");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMembers_ChatRoomId1",
                table: "ChatMembers",
                column: "ChatRoomId1");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "AspNetRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMembers_ChatRoles_ChatRoleId1",
                table: "ChatMembers",
                column: "ChatRoleId1",
                principalTable: "ChatRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMembers_Rooms_ChatRoomId1",
                table: "ChatMembers",
                column: "ChatRoomId1",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostId1",
                table: "Comments",
                column: "PostId1",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Posts_PostId1",
                table: "Favorites",
                column: "PostId1",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Rooms_ChatRoomId1",
                table: "Messages",
                column: "ChatRoomId1",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileGifts_Gifts_GiftId1",
                table: "ProfileGifts",
                column: "GiftId1",
                principalTable: "Gifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileRanks_Ranks_RankId1",
                table: "ProfileRanks",
                column: "RankId1",
                principalTable: "Ranks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetRoleClaims_AspNetRoles_RoleId",
                table: "AspNetRoleClaims");

            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUserRoles_AspNetRoles_RoleId",
                table: "AspNetUserRoles");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMembers_ChatRoles_ChatRoleId1",
                table: "ChatMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_ChatMembers_Rooms_ChatRoomId1",
                table: "ChatMembers");

            migrationBuilder.DropForeignKey(
                name: "FK_Comments_Posts_PostId1",
                table: "Comments");

            migrationBuilder.DropForeignKey(
                name: "FK_Favorites_Posts_PostId1",
                table: "Favorites");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Rooms_ChatRoomId1",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileGifts_Gifts_GiftId1",
                table: "ProfileGifts");

            migrationBuilder.DropForeignKey(
                name: "FK_ProfileRanks_Ranks_RankId1",
                table: "ProfileRanks");

            migrationBuilder.DropIndex(
                name: "IX_ProfileRanks_RankId1",
                table: "ProfileRanks");

            migrationBuilder.DropIndex(
                name: "IX_ProfileGifts_GiftId1",
                table: "ProfileGifts");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ChatRoomId1",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Favorites_PostId1",
                table: "Favorites");

            migrationBuilder.DropIndex(
                name: "IX_Comments_PostId1",
                table: "Comments");

            migrationBuilder.DropIndex(
                name: "IX_ChatMembers_ChatRoleId1",
                table: "ChatMembers");

            migrationBuilder.DropIndex(
                name: "IX_ChatMembers_ChatRoomId1",
                table: "ChatMembers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_AspNetRoles",
                table: "AspNetRoles");

            migrationBuilder.DropColumn(
                name: "ChangedAt",
                table: "Watchers");

            migrationBuilder.DropColumn(
                name: "ChangedBy",
                table: "Watchers");

            migrationBuilder.DropColumn(
                name: "ChangedAt",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "ChangedBy",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "ChangedAt",
                table: "Ranks");

            migrationBuilder.DropColumn(
                name: "ChangedBy",
                table: "Ranks");

            migrationBuilder.DropColumn(
                name: "ChangedAt",
                table: "ProfileRanks");

            migrationBuilder.DropColumn(
                name: "ChangedBy",
                table: "ProfileRanks");

            migrationBuilder.DropColumn(
                name: "RankId1",
                table: "ProfileRanks");

            migrationBuilder.DropColumn(
                name: "ChangedAt",
                table: "ProfileGifts");

            migrationBuilder.DropColumn(
                name: "ChangedBy",
                table: "ProfileGifts");

            migrationBuilder.DropColumn(
                name: "GiftId1",
                table: "ProfileGifts");

            migrationBuilder.DropColumn(
                name: "ChangedAt",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ChangedBy",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "ChangedAt",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ChangedBy",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ChatRoomId1",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ChangedAt",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "ChangedBy",
                table: "Gifts");

            migrationBuilder.DropColumn(
                name: "ChangedAt",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "ChangedBy",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "PostId1",
                table: "Favorites");

            migrationBuilder.DropColumn(
                name: "ChangedAt",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ChangedBy",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "PostId1",
                table: "Comments");

            migrationBuilder.DropColumn(
                name: "ChangedAt",
                table: "ChatRoles");

            migrationBuilder.DropColumn(
                name: "ChangedBy",
                table: "ChatRoles");

            migrationBuilder.DropColumn(
                name: "ChangedAt",
                table: "ChatMembers");

            migrationBuilder.DropColumn(
                name: "ChangedBy",
                table: "ChatMembers");

            migrationBuilder.DropColumn(
                name: "ChatRoleId1",
                table: "ChatMembers");

            migrationBuilder.DropColumn(
                name: "ChatRoomId1",
                table: "ChatMembers");

            migrationBuilder.DropColumn(
                name: "ChangedAt",
                table: "BlockedProfiles");

            migrationBuilder.DropColumn(
                name: "ChangedBy",
                table: "BlockedProfiles");

            migrationBuilder.RenameTable(
                name: "AspNetRoles",
                newName: "UserRole");

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "Watchers",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Watchers",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Watchers",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "Rooms",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Rooms",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Rooms",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "Ranks",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Ranks",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Ranks",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "ProfileRanks",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "ProfileRanks",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "ProfileRanks",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "ProfileGifts",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "ProfileGifts",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "ProfileGifts",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "Posts",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Posts",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Posts",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "Messages",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Messages",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Messages",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "Gifts",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Gifts",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Gifts",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "Favorites",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Favorites",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Favorites",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "Comments",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "Comments",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "Comments",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "ChatRoles",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "ChatRoles",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "ChatRoles",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "ChatMembers",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "ChatMembers",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "ChatMembers",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AlterColumn<string>(
                name: "DeletedBy",
                table: "BlockedProfiles",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "CreatedBy",
                table: "BlockedProfiles",
                type: "longtext CHARACTER SET utf8mb4",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Id",
                table: "BlockedProfiles",
                type: "varchar(255) CHARACTER SET utf8mb4",
                nullable: false,
                oldClrType: typeof(Guid));

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserRole",
                table: "UserRole",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileRanks_RankId",
                table: "ProfileRanks",
                column: "RankId");

            migrationBuilder.CreateIndex(
                name: "IX_ProfileGifts_GiftId",
                table: "ProfileGifts",
                column: "GiftId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ChatRoomId",
                table: "Messages",
                column: "ChatRoomId");

            migrationBuilder.CreateIndex(
                name: "IX_Favorites_PostId",
                table: "Favorites",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMembers_ChatRoleId",
                table: "ChatMembers",
                column: "ChatRoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ChatMembers_ChatRoomId",
                table: "ChatMembers",
                column: "ChatRoomId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetRoleClaims_UserRole_RoleId",
                table: "AspNetRoleClaims",
                column: "RoleId",
                principalTable: "UserRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUserRoles_UserRole_RoleId",
                table: "AspNetUserRoles",
                column: "RoleId",
                principalTable: "UserRole",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMembers_ChatRoles_ChatRoleId",
                table: "ChatMembers",
                column: "ChatRoleId",
                principalTable: "ChatRoles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ChatMembers_Rooms_ChatRoomId",
                table: "ChatMembers",
                column: "ChatRoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Comments_Posts_PostId",
                table: "Comments",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Favorites_Posts_PostId",
                table: "Favorites",
                column: "PostId",
                principalTable: "Posts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Rooms_ChatRoomId",
                table: "Messages",
                column: "ChatRoomId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileGifts_Gifts_GiftId",
                table: "ProfileGifts",
                column: "GiftId",
                principalTable: "Gifts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ProfileRanks_Ranks_RankId",
                table: "ProfileRanks",
                column: "RankId",
                principalTable: "Ranks",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
