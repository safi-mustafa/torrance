using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class LogFieldsUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "EquipmentNo",
                table: "TOTLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<int>(
                name: "DelayReason",
                table: "OverrideLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "DelayTypeId",
                table: "OverrideLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "ReworkDelayId",
                table: "OverrideLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ShiftDelayId",
                table: "OverrideLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "StartOfWorkDelayId",
                table: "OverrideLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_OverrideLogs_DelayTypeId",
                table: "OverrideLogs",
                column: "DelayTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OverrideLogs_ReworkDelayId",
                table: "OverrideLogs",
                column: "ReworkDelayId");

            migrationBuilder.CreateIndex(
                name: "IX_OverrideLogs_ShiftDelayId",
                table: "OverrideLogs",
                column: "ShiftDelayId");

            migrationBuilder.CreateIndex(
                name: "IX_OverrideLogs_StartOfWorkDelayId",
                table: "OverrideLogs",
                column: "StartOfWorkDelayId");

            migrationBuilder.AddForeignKey(
                name: "FK_OverrideLogs_DelayTypes_DelayTypeId",
                table: "OverrideLogs",
                column: "DelayTypeId",
                principalTable: "DelayTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OverrideLogs_ReworkDelays_ReworkDelayId",
                table: "OverrideLogs",
                column: "ReworkDelayId",
                principalTable: "ReworkDelays",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OverrideLogs_ShiftDelays_ShiftDelayId",
                table: "OverrideLogs",
                column: "ShiftDelayId",
                principalTable: "ShiftDelays",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OverrideLogs_StartOfWorkDelays_StartOfWorkDelayId",
                table: "OverrideLogs",
                column: "StartOfWorkDelayId",
                principalTable: "StartOfWorkDelays",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OverrideLogs_DelayTypes_DelayTypeId",
                table: "OverrideLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_OverrideLogs_ReworkDelays_ReworkDelayId",
                table: "OverrideLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_OverrideLogs_ShiftDelays_ShiftDelayId",
                table: "OverrideLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_OverrideLogs_StartOfWorkDelays_StartOfWorkDelayId",
                table: "OverrideLogs");

            migrationBuilder.DropIndex(
                name: "IX_OverrideLogs_DelayTypeId",
                table: "OverrideLogs");

            migrationBuilder.DropIndex(
                name: "IX_OverrideLogs_ReworkDelayId",
                table: "OverrideLogs");

            migrationBuilder.DropIndex(
                name: "IX_OverrideLogs_ShiftDelayId",
                table: "OverrideLogs");

            migrationBuilder.DropIndex(
                name: "IX_OverrideLogs_StartOfWorkDelayId",
                table: "OverrideLogs");

            migrationBuilder.DropColumn(
                name: "DelayReason",
                table: "OverrideLogs");

            migrationBuilder.DropColumn(
                name: "DelayTypeId",
                table: "OverrideLogs");

            migrationBuilder.DropColumn(
                name: "ReworkDelayId",
                table: "OverrideLogs");

            migrationBuilder.DropColumn(
                name: "ShiftDelayId",
                table: "OverrideLogs");

            migrationBuilder.DropColumn(
                name: "StartOfWorkDelayId",
                table: "OverrideLogs");

            migrationBuilder.AlterColumn<string>(
                name: "EquipmentNo",
                table: "TOTLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }
    }
}
