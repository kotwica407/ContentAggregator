using Microsoft.EntityFrameworkCore.Migrations;

namespace ContentAggregator.Context.Migrations
{
    public partial class baseEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Hashes",
                table: "Hashes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Hashes");

            migrationBuilder.AddColumn<string>(
                name: "Id",
                table: "Hashes",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Hashes",
                table: "Hashes",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Hashes",
                table: "Hashes");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "Hashes");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Hashes",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Hashes",
                table: "Hashes",
                column: "UserId");
        }
    }
}
