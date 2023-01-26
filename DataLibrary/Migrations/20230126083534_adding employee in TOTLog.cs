using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class addingemployeeinTOTLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "EmployeeId",
                table: "TOTLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TOTLogs_EmployeeId",
                table: "TOTLogs",
                column: "EmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TOTLogs_Employees_EmployeeId",
                table: "TOTLogs",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_Employees_EmployeeId",
                table: "TOTLogs");

            migrationBuilder.DropIndex(
                name: "IX_TOTLogs_EmployeeId",
                table: "TOTLogs");

            migrationBuilder.DropColumn(
                name: "EmployeeId",
                table: "TOTLogs");
        }
    }
}
