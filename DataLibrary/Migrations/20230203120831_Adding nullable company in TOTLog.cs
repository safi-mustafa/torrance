using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddingnullablecompanyinTOTLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CompanyId",
                table: "TOTLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TOTLogs_CompanyId",
                table: "TOTLogs",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_TOTLogs_Companies_CompanyId",
                table: "TOTLogs",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_Companies_CompanyId",
                table: "TOTLogs");

            migrationBuilder.DropIndex(
                name: "IX_TOTLogs_CompanyId",
                table: "TOTLogs");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "TOTLogs");
        }
    }
}
