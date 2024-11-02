using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebMaker.Migrations
{
    /// <inheritdoc />
    public partial class InitialTest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "AppCategoryTranslations",
                type: "nvarchar(40)",
                maxLength: 40,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "AppCategoryTranslations",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "AppCategoryTranslations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "AppCategoryTranslations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "AppCategoryTranslations",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "AppCategoryTranslations",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "AppCategoryTranslations",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "AppCategoryTranslations");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "AppCategoryTranslations");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "AppCategoryTranslations");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "AppCategoryTranslations");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "AppCategoryTranslations");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "AppCategoryTranslations");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "AppCategoryTranslations");
        }
    }
}
