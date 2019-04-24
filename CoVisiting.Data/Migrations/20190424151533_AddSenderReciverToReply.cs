using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CoVisiting.Data.Migrations
{
    public partial class AddSenderReciverToReply : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventReplies_AspNetUsers_UserId",
                table: "EventReplies");

            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "EventReplies",
                newName: "SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_EventReplies_UserId",
                table: "EventReplies",
                newName: "IX_EventReplies_SenderId");

            migrationBuilder.AddColumn<string>(
                name: "RecieverId",
                table: "EventReplies",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_EventReplies_RecieverId",
                table: "EventReplies",
                column: "RecieverId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventReplies_AspNetUsers_RecieverId",
                table: "EventReplies",
                column: "RecieverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_EventReplies_AspNetUsers_SenderId",
                table: "EventReplies",
                column: "SenderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_EventReplies_AspNetUsers_RecieverId",
                table: "EventReplies");

            migrationBuilder.DropForeignKey(
                name: "FK_EventReplies_AspNetUsers_SenderId",
                table: "EventReplies");

            migrationBuilder.DropIndex(
                name: "IX_EventReplies_RecieverId",
                table: "EventReplies");

            migrationBuilder.DropColumn(
                name: "RecieverId",
                table: "EventReplies");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "EventReplies",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_EventReplies_SenderId",
                table: "EventReplies",
                newName: "IX_EventReplies_UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_EventReplies_AspNetUsers_UserId",
                table: "EventReplies",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
