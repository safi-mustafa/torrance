using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class addingapproverinoverridelogsdbmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ApproverId",
                table: "OverrideLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OverrideLogs_ApproverId",
                table: "OverrideLogs",
                column: "ApproverId");

            migrationBuilder.AddForeignKey(
                name: "FK_OverrideLogs_AspNetUsers_ApproverId",
                table: "OverrideLogs",
                column: "ApproverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OverrideLogs_AspNetUsers_ApproverId",
                table: "OverrideLogs");

            migrationBuilder.DropIndex(
                name: "IX_OverrideLogs_ApproverId",
                table: "OverrideLogs");

            migrationBuilder.DropColumn(
                name: "ApproverId",
                table: "OverrideLogs");
        }
    }
}
