using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CoVisiting.Data.Migrations
{
    public partial class ManyToManySubsEvents : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AspNetUsers_Events_EventId",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "IX_AspNetUsers_EventId",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "EventId",
                table: "AspNetUsers");

            migrationBuilder.CreateTable(
                name: "UserEventJoinTable",
                columns: table => new
                {
                    EventId = table.Column<int>(nullable: false),
                    UserId = table.Column<string>(nullable: false),
                    AppUserId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserEventJoinTable", x => new { x.EventId, x.UserId });
                    table.ForeignKey(
                        name: "FK_UserEventJoinTable_AspNetUsers_AppUserId",
                        column: x => x.AppUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserEventJoinTable_Events_EventId",
                        column: x => x.EventId,
                        principalTable: "Events",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_UserEventJoinTable_AppUserId",
                table: "UserEventJoinTable",
                column: "AppUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "UserEventJoinTable");

            migrationBuilder.AddColumn<int>(
                name: "EventId",
                table: "AspNetUsers",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_AspNetUsers_EventId",
                table: "AspNetUsers",
                column: "EventId");

            migrationBuilder.AddForeignKey(
                name: "FK_AspNetUsers_Events_EventId",
                table: "AspNetUsers",
                column: "EventId",
                principalTable: "Events",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
