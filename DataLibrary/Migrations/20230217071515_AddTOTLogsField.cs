using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddTOTLogsField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DelayDescription",
                table: "TOTLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DelayReason",
                table: "TOTLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "PermitNo",
                table: "TOTLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StartOfWorkDelayId",
                table: "TOTLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StartOfWorkDelays",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ActiveStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StartOfWorkDelays", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TOTLogs_StartOfWorkDelayId",
                table: "TOTLogs",
                column: "StartOfWorkDelayId");

            migrationBuilder.AddForeignKey(
                name: "FK_TOTLogs_StartOfWorkDelays_StartOfWorkDelayId",
                table: "TOTLogs",
                column: "StartOfWorkDelayId",
                principalTable: "StartOfWorkDelays",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_StartOfWorkDelays_StartOfWorkDelayId",
                table: "TOTLogs");

            migrationBuilder.DropTable(
                name: "StartOfWorkDelays");

            migrationBuilder.DropIndex(
                name: "IX_TOTLogs_StartOfWorkDelayId",
                table: "TOTLogs");

            migrationBuilder.DropColumn(
                name: "DelayDescription",
                table: "TOTLogs");

            migrationBuilder.DropColumn(
                name: "DelayReason",
                table: "TOTLogs");

            migrationBuilder.DropColumn(
                name: "PermitNo",
                table: "TOTLogs");

            migrationBuilder.DropColumn(
                name: "StartOfWorkDelayId",
                table: "TOTLogs");
        }
    }
}
