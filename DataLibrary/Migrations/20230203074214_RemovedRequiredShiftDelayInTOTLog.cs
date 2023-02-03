﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class RemovedRequiredShiftDelayInTOTLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_ShiftDelays_ShiftDelayId",
                table: "TOTLogs");

            migrationBuilder.AlterColumn<long>(
                name: "ShiftDelayId",
                table: "TOTLogs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_TOTLogs_ShiftDelays_ShiftDelayId",
                table: "TOTLogs",
                column: "ShiftDelayId",
                principalTable: "ShiftDelays",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_ShiftDelays_ShiftDelayId",
                table: "TOTLogs");

            migrationBuilder.AlterColumn<long>(
                name: "ShiftDelayId",
                table: "TOTLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TOTLogs_ShiftDelays_ShiftDelayId",
                table: "TOTLogs",
                column: "ShiftDelayId",
                principalTable: "ShiftDelays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
