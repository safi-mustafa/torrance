using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class ModifyingMobileFilemodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Type",
                table: "MobileFiles",
                newName: "FileType");

            migrationBuilder.AddColumn<string>(
                name: "ExtensionType",
                table: "MobileFiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "MobileFiles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "UploadDate",
                table: "MobileFiles",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExtensionType",
                table: "MobileFiles");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "MobileFiles");

            migrationBuilder.DropColumn(
                name: "UploadDate",
                table: "MobileFiles");

            migrationBuilder.RenameColumn(
                name: "FileType",
                table: "MobileFiles",
                newName: "Type");
        }
    }
}
