using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class ForemanAsStringInToTLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_AspNetUsers_ForemanId",
                table: "TOTLogs");

            migrationBuilder.DropIndex(
                name: "IX_TOTLogs_ForemanId",
                table: "TOTLogs");

            migrationBuilder.DropColumn(
                name: "ForemanId",
                table: "TOTLogs");

            migrationBuilder.AddColumn<string>(
                name: "Foreman",
                table: "TOTLogs",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Foreman",
                table: "TOTLogs");

            migrationBuilder.AddColumn<long>(
                name: "ForemanId",
                table: "TOTLogs",
                type: "bigint",
                nullable: true);

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
    }
}
