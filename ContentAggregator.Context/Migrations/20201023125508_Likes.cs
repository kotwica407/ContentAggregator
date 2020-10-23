using Microsoft.EntityFrameworkCore.Migrations;

namespace ContentAggregator.Context.Migrations
{
    public partial class Likes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Responses");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Posts");

            migrationBuilder.DropColumn(
                name: "Rate",
                table: "Comments");

            migrationBuilder.CreateTable(
                name: "CommentLikes",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    EntityId = table.Column<string>(nullable: false),
                    IsLike = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommentLikes", x => new { x.EntityId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "PostLikes",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    EntityId = table.Column<string>(nullable: false),
                    IsLike = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PostLikes", x => new { x.EntityId, x.UserId });
                });

            migrationBuilder.CreateTable(
                name: "ResponseLikes",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    EntityId = table.Column<string>(nullable: false),
                    IsLike = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResponseLikes", x => new { x.EntityId, x.UserId });
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommentLikes");

            migrationBuilder.DropTable(
                name: "PostLikes");

            migrationBuilder.DropTable(
                name: "ResponseLikes");

            migrationBuilder.AddColumn<int>(
                name: "Rate",
                table: "Responses",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rate",
                table: "Posts",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Rate",
                table: "Comments",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
