using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class EmployeeChangedTOUserInLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OverrideLogs_Employees_EmployeeId",
                table: "OverrideLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_Employees_EmployeeId",
                table: "TOTLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_WRRLogs_Employees_EmployeeId",
                table: "WRRLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_OverrideLogs_AspNetUsers_EmployeeId",
                table: "OverrideLogs",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TOTLogs_AspNetUsers_EmployeeId",
                table: "TOTLogs",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WRRLogs_AspNetUsers_EmployeeId",
                table: "WRRLogs",
                column: "EmployeeId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OverrideLogs_AspNetUsers_EmployeeId",
                table: "OverrideLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_AspNetUsers_EmployeeId",
                table: "TOTLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_WRRLogs_AspNetUsers_EmployeeId",
                table: "WRRLogs");

            migrationBuilder.AddForeignKey(
                name: "FK_OverrideLogs_Employees_EmployeeId",
                table: "OverrideLogs",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TOTLogs_Employees_EmployeeId",
                table: "TOTLogs",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WRRLogs_Employees_EmployeeId",
                table: "WRRLogs",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }
    }
}
