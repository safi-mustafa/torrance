using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddedDepartmentInOverrideLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DepartmentId",
                table: "OverrideLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OverrideLogs_DepartmentId",
                table: "OverrideLogs",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_OverrideLogs_Departments_DepartmentId",
                table: "OverrideLogs",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OverrideLogs_Departments_DepartmentId",
                table: "OverrideLogs");

            migrationBuilder.DropIndex(
                name: "IX_OverrideLogs_DepartmentId",
                table: "OverrideLogs");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "OverrideLogs");
        }
    }
}
