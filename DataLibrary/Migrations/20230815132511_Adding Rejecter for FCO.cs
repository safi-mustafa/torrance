using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddingRejecterforFCO : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "BusinessTeamLeaderApprovalDate",
                table: "FCOLogs",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AreaExecutionLeadApprovalDate",
                table: "FCOLogs",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<DateTime>(
                name: "RejecterDate",
                table: "FCOLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RejecterId",
                table: "FCOLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FCOLogs_RejecterId",
                table: "FCOLogs",
                column: "RejecterId");

            migrationBuilder.AddForeignKey(
                name: "FK_FCOLogs_AspNetUsers_RejecterId",
                table: "FCOLogs",
                column: "RejecterId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FCOLogs_AspNetUsers_RejecterId",
                table: "FCOLogs");

            migrationBuilder.DropIndex(
                name: "IX_FCOLogs_RejecterId",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "RejecterDate",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "RejecterId",
                table: "FCOLogs");

            migrationBuilder.AlterColumn<DateTime>(
                name: "BusinessTeamLeaderApprovalDate",
                table: "FCOLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AreaExecutionLeadApprovalDate",
                table: "FCOLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
