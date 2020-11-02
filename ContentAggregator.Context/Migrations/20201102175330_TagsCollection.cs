using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContentAggregator.Context.Migrations
{
    public partial class TagsCollection : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Tags",
                table: "Posts");

            migrationBuilder.AddColumn<string>(
                name: "TagCollection",
                table: "Posts",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TagCollection",
                table: "Posts");

            migrationBuilder.AddColumn<string[]>(
                name: "Tags",
                table: "Posts",
                type: "text[]",
                nullable: true);
        }
    }
}
