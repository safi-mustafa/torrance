using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddingFolderIdinAttachments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "FolderId",
                table: "Attachments",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_FolderId",
                table: "Attachments",
                column: "FolderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Attachments_Folders_FolderId",
                table: "Attachments",
                column: "FolderId",
                principalTable: "Folders",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Attachments_Folders_FolderId",
                table: "Attachments");

            migrationBuilder.DropIndex(
                name: "IX_Attachments_FolderId",
                table: "Attachments");

            migrationBuilder.DropColumn(
                name: "FolderId",
                table: "Attachments");
        }
    }
}
