using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class addingcontractorinWRRLogmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "ContractorId",
                table: "WRRLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_WRRLogs_ContractorId",
                table: "WRRLogs",
                column: "ContractorId");

            migrationBuilder.AddForeignKey(
                name: "FK_WRRLogs_Contractors_ContractorId",
                table: "WRRLogs",
                column: "ContractorId",
                principalTable: "Contractors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WRRLogs_Contractors_ContractorId",
                table: "WRRLogs");

            migrationBuilder.DropIndex(
                name: "IX_WRRLogs_ContractorId",
                table: "WRRLogs");

            migrationBuilder.DropColumn(
                name: "ContractorId",
                table: "WRRLogs");
        }
    }
}
