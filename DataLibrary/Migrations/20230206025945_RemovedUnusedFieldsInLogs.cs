using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class RemovedUnusedFieldsInLogs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DelayReason",
                table: "TOTLogs");

            migrationBuilder.DropColumn(
                name: "DateSubmitted",
                table: "OverrideLogs");

            migrationBuilder.DropColumn(
                name: "TimeSubmitted",
                table: "OverrideLogs");

            migrationBuilder.AddColumn<long>(
                name: "ReasonForRequestId",
                table: "TOTLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TOTLogs_ReasonForRequestId",
                table: "TOTLogs",
                column: "ReasonForRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_TOTLogs_ReasonForRequests_ReasonForRequestId",
                table: "TOTLogs",
                column: "ReasonForRequestId",
                principalTable: "ReasonForRequests",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_ReasonForRequests_ReasonForRequestId",
                table: "TOTLogs");

            migrationBuilder.DropIndex(
                name: "IX_TOTLogs_ReasonForRequestId",
                table: "TOTLogs");

            migrationBuilder.DropColumn(
                name: "ReasonForRequestId",
                table: "TOTLogs");

            migrationBuilder.AddColumn<string>(
                name: "DelayReason",
                table: "TOTLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateSubmitted",
                table: "OverrideLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<TimeSpan>(
                name: "TimeSubmitted",
                table: "OverrideLogs",
                type: "time",
                nullable: false,
                defaultValue: new TimeSpan(0, 0, 0, 0, 0));
        }
    }
}
