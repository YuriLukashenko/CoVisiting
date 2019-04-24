using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace CoVisiting.Data.Migrations
{
    public partial class AddEnums : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsOnlyForAuthor",
                table: "EventReplies");

            migrationBuilder.AddColumn<int>(
                name: "ReplyScope",
                table: "EventReplies",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ReplyScope",
                table: "EventReplies");

            migrationBuilder.AddColumn<bool>(
                name: "IsOnlyForAuthor",
                table: "EventReplies",
                nullable: false,
                defaultValue: false);
        }
    }
}
