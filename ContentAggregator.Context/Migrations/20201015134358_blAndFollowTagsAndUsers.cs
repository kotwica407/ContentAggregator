using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContentAggregator.Context.Migrations
{
    public partial class blAndFollowTagsAndUsers : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string[]>(
                name: "BlackListedTags",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "BlackListedUserIds",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "FollowedTags",
                table: "Users",
                nullable: true);

            migrationBuilder.AddColumn<string[]>(
                name: "FollowedUserIds",
                table: "Users",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
    }
}
