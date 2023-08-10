using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class CompanymadenullableinFCOLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FCOLogs_Departments_CompanyId",
                table: "FCOLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_FCOLogs_Companies_CompanyId",
                table: "FCOLogs",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FCOLogs_Companies_CompanyId",
                table: "FCOLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_FCOLogs_Departments_CompanyId",
                table: "FCOLogs",
                column: "CompanyId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
