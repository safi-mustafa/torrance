using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class addingapproverinWRRLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ApproverId",
                table: "WRRLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WRRLogs_ApproverId",
                table: "WRRLogs",
                column: "ApproverId");

            migrationBuilder.AddForeignKey(
                name: "FK_WRRLogs_AspNetUsers_ApproverId",
                table: "WRRLogs",
                column: "ApproverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WRRLogs_AspNetUsers_ApproverId",
                table: "WRRLogs");

            migrationBuilder.DropIndex(
                name: "IX_WRRLogs_ApproverId",
                table: "WRRLogs");

            migrationBuilder.DropColumn(
                name: "ApproverId",
                table: "WRRLogs");
        }
    }
}
