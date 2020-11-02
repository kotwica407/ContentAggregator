using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContentAggregator.Context.Migrations
{
    public partial class NotMappedArraysOfString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BlackListedTags",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "BlackListedUserIds",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FollowedTags",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "FollowedUserIds",
                table: "Users");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "BlackListedTags",
                table: "Users",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "BlackListedUserIds",
                table: "Users",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "FollowedTags",
                table: "Users",
                type: "text[]",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "FollowedUserIds",
                table: "Users",
                type: "text[]",
                nullable: true);
        }
    }
}
