using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class Modifyingforemanintotlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_TOTLogs_ForemanId",
                table: "TOTLogs",
                column: "ForemanId");

            migrationBuilder.AddForeignKey(
                name: "FK_TOTLogs_AspNetUsers_ForemanId",
                table: "TOTLogs",
                column: "ForemanId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_AspNetUsers_ForemanId",
                table: "TOTLogs");

            migrationBuilder.DropIndex(
                name: "IX_TOTLogs_ForemanId",
                table: "TOTLogs");
        }
    }
}
