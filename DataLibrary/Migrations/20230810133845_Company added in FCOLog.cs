using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class CompanyaddedinFCOLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CompanyId",
                table: "FCOLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_FCOLogs_CompanyId",
                table: "FCOLogs",
                column: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_FCOLogs_Departments_CompanyId",
                table: "FCOLogs",
                column: "CompanyId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.NoAction);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FCOLogs_Departments_CompanyId",
                table: "FCOLogs");

            migrationBuilder.DropIndex(
                name: "IX_FCOLogs_CompanyId",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "FCOLogs");
        }
    }
}
