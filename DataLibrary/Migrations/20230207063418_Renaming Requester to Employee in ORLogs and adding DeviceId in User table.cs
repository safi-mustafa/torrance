using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class RenamingRequestertoEmployeeinORLogsandaddingDeviceIdinUsertable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OverrideLogs_Employees_RequesterId",
                table: "OverrideLogs");

            migrationBuilder.RenameColumn(
                name: "RequesterId",
                table: "OverrideLogs",
                newName: "EmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_OverrideLogs_RequesterId",
                table: "OverrideLogs",
                newName: "IX_OverrideLogs_EmployeeId");

            migrationBuilder.AddColumn<long>(
                name: "Entity",
                table: "Notifications",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "EntityType",
                table: "Notifications",
                type: "int",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OverrideLogs_Employees_EmployeeId",
                table: "OverrideLogs",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OverrideLogs_Employees_EmployeeId",
                table: "OverrideLogs");

            migrationBuilder.DropColumn(
                name: "Entity",
                table: "Notifications");

            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "Notifications");

            migrationBuilder.RenameColumn(
                name: "EmployeeId",
                table: "OverrideLogs",
                newName: "RequesterId");

            migrationBuilder.RenameIndex(
                name: "IX_OverrideLogs_EmployeeId",
                table: "OverrideLogs",
                newName: "IX_OverrideLogs_RequesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_OverrideLogs_Employees_RequesterId",
                table: "OverrideLogs",
                column: "RequesterId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
