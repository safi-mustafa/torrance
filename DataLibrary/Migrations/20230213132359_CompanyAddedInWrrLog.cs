using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class CompanyAddedInWrrLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CompanyId",
                table: "WRRLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_WRRLogs_CompanyId",
                table: "WRRLogs",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_WRRLogs_Companies_CompanyId",
                table: "WRRLogs",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WRRLogs_Companies_CompanyId",
                table: "WRRLogs");

            migrationBuilder.DropIndex(
                name: "IX_WRRLogs_CompanyId",
                table: "WRRLogs");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "WRRLogs");
        }
    }
}
