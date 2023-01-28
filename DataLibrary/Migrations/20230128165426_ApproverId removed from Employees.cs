using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class ApproverIdremovedfromEmployees : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_AspNetUsers_ApproverId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ApproverId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ApproverId",
                table: "Employees");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ApproverId",
                table: "Employees",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ApproverId",
                table: "Employees",
                column: "ApproverId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_AspNetUsers_ApproverId",
                table: "Employees",
                column: "ApproverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
