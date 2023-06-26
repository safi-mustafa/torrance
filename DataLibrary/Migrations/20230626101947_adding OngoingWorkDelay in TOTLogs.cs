using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class addingOngoingWorkDelayinTOTLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "OngoingWorkDelayId",
                table: "TOTLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TOTLogs_OngoingWorkDelayId",
                table: "TOTLogs",
                column: "OngoingWorkDelayId");

            migrationBuilder.AddForeignKey(
                name: "FK_TOTLogs_OngoingWorkDelays_OngoingWorkDelayId",
                table: "TOTLogs",
                column: "OngoingWorkDelayId",
                principalTable: "OngoingWorkDelays",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_OngoingWorkDelays_OngoingWorkDelayId",
                table: "TOTLogs");

            migrationBuilder.DropIndex(
                name: "IX_TOTLogs_OngoingWorkDelayId",
                table: "TOTLogs");

            migrationBuilder.DropColumn(
                name: "OngoingWorkDelayId",
                table: "TOTLogs");
        }
    }
}
