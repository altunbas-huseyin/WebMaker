using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebMaker.Migrations
{
    /// <inheritdoc />
    public partial class Added_Article_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AppArticles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", maxLength: 10000, nullable: false),
                    Summary = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    IsPublished = table.Column<bool>(type: "bit", nullable: false),
                    PublishDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    SeoTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SeoDescription = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    SeoKeywords = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SeoSlug = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppArticles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AppCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(128)", maxLength: 128, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    ParentCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SeoTitle = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    SeoDescription = table.Column<string>(type: "nvarchar(160)", maxLength: 160, nullable: false),
                    SeoKeywords = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    SeoSlug = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    ExtraProperties = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ConcurrencyStamp = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatorId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AppCategories_AppCategories_ParentCategoryId",
                        column: x => x.ParentCategoryId,
                        principalTable: "AppCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AppArticleCategories",
                columns: table => new
                {
                    ArticleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AppArticleCategories", x => new { x.ArticleId, x.CategoryId });
                    table.ForeignKey(
                        name: "FK_AppArticleCategories_AppArticles_ArticleId",
                        column: x => x.ArticleId,
                        principalTable: "AppArticles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AppArticleCategories_AppCategories_CategoryId",
                        column: x => x.CategoryId,
                        principalTable: "AppCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AppArticleCategories_ArticleId",
                table: "AppArticleCategories",
                column: "ArticleId");

            migrationBuilder.CreateIndex(
                name: "IX_AppArticleCategories_CategoryId",
                table: "AppArticleCategories",
                column: "CategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AppArticles_SeoSlug",
                table: "AppArticles",
                column: "SeoSlug");

            migrationBuilder.CreateIndex(
                name: "IX_AppCategories_ParentCategoryId",
                table: "AppCategories",
                column: "ParentCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AppCategories_SeoSlug",
                table: "AppCategories",
                column: "SeoSlug");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AppArticleCategories");

            migrationBuilder.DropTable(
                name: "AppArticles");

            migrationBuilder.DropTable(
                name: "AppCategories");
        }
    }
}
