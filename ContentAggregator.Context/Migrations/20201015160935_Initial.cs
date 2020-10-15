using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContentAggregator.Context.Migrations
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Hashes",
                columns: table => new
                {
                    UserId = table.Column<string>(nullable: false),
                    PasswordHash = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Hashes", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Posts",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AuthorId = table.Column<string>(nullable: false),
                    Title = table.Column<string>(maxLength: 50, nullable: false),
                    Content = table.Column<string>(maxLength: 2000, nullable: false),
                    Tags = table.Column<string[]>(nullable: true),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastUpdateTime = table.Column<DateTime>(nullable: false),
                    Rate = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Posts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Tags",
                columns: table => new
                {
                    Name = table.Column<string>(nullable: false),
                    PostsNumber = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tags", x => x.Name);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    Name = table.Column<string>(maxLength: 25, nullable: false),
                    Email = table.Column<string>(nullable: false),
                    CredentialLevel = table.Column<byte>(nullable: false),
                    Description = table.Column<string>(maxLength: 300, nullable: true),
                    BlackListedTags = table.Column<string[]>(nullable: true),
                    FollowedTags = table.Column<string[]>(nullable: true),
                    BlackListedUserIds = table.Column<string[]>(nullable: true),
                    FollowedUserIds = table.Column<string[]>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Comments",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AuthorId = table.Column<string>(nullable: false),
                    Content = table.Column<string>(maxLength: 1000, nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastUpdateTime = table.Column<DateTime>(nullable: false),
                    PostId = table.Column<string>(nullable: false),
                    Rate = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Comments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Comments_Posts_PostId",
                        column: x => x.PostId,
                        principalTable: "Posts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Responses",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false),
                    AuthorId = table.Column<string>(nullable: false),
                    Content = table.Column<string>(maxLength: 1000, nullable: false),
                    CreationTime = table.Column<DateTime>(nullable: false),
                    LastUpdateTime = table.Column<DateTime>(nullable: false),
                    CommentId = table.Column<string>(nullable: false),
                    Rate = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Responses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Responses_Comments_CommentId",
                        column: x => x.CommentId,
                        principalTable: "Comments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Comments_PostId",
                table: "Comments",
                column: "PostId");

            migrationBuilder.CreateIndex(
                name: "IX_Responses_CommentId",
                table: "Responses",
                column: "CommentId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Hashes");

            migrationBuilder.DropTable(
                name: "Responses");

            migrationBuilder.DropTable(
                name: "Tags");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Comments");

            migrationBuilder.DropTable(
                name: "Posts");
        }
    }
}
