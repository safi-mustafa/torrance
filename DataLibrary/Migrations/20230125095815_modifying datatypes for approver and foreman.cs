using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class modifyingdatatypesforapproverandforeman : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_AspNetUsers_ApproverId",
                table: "TOTLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_AspNetUsers_ForemanId",
                table: "TOTLogs");

            migrationBuilder.DropIndex(
                name: "IX_TOTLogs_ApproverId",
                table: "TOTLogs");

            migrationBuilder.DropIndex(
                name: "IX_TOTLogs_ForemanId",
                table: "TOTLogs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TOTLogs_ApproverId",
                table: "TOTLogs",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_TOTLogs_ForemanId",
                table: "TOTLogs",
                column: "ForemanId");

            migrationBuilder.AddForeignKey(
                name: "FK_TOTLogs_AspNetUsers_ApproverId",
                table: "TOTLogs",
                column: "ApproverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TOTLogs_AspNetUsers_ForemanId",
                table: "TOTLogs",
                column: "ForemanId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
