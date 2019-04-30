using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CoVisiting.Data.Migrations
{
    public partial class Fixes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserEventJoinTable_AspNetUsers_AppUserId",
                table: "UserEventJoinTable");

            migrationBuilder.RenameColumn(
                name: "AppUserId",
                table: "UserEventJoinTable",
                newName: "ApplicationUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserEventJoinTable_AppUserId",
                table: "UserEventJoinTable",
                newName: "IX_UserEventJoinTable_ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserEventJoinTable_AspNetUsers_ApplicationUserId",
                table: "UserEventJoinTable",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserEventJoinTable_AspNetUsers_ApplicationUserId",
                table: "UserEventJoinTable");

            migrationBuilder.RenameColumn(
                name: "ApplicationUserId",
                table: "UserEventJoinTable",
                newName: "AppUserId");

            migrationBuilder.RenameIndex(
                name: "IX_UserEventJoinTable_ApplicationUserId",
                table: "UserEventJoinTable",
                newName: "IX_UserEventJoinTable_AppUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_UserEventJoinTable_AspNetUsers_AppUserId",
                table: "UserEventJoinTable",
                column: "AppUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
