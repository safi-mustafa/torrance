using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class Modifyingapproverintotlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TOTLogs_ApproverId",
                table: "TOTLogs",
                column: "ApproverId");

            migrationBuilder.AddForeignKey(
                name: "FK_TOTLogs_AspNetUsers_ApproverId",
                table: "TOTLogs",
                column: "ApproverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_AspNetUsers_ApproverId",
                table: "TOTLogs");

            migrationBuilder.DropIndex(
                name: "IX_TOTLogs_ApproverId",
                table: "TOTLogs");
        }
    }
}
