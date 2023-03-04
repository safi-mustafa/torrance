using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class LogsFieldsUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OverrideLogs_DelayTypes_DelayTypeId",
                table: "OverrideLogs");

            migrationBuilder.AlterColumn<long>(
                name: "DelayTypeId",
                table: "OverrideLogs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<int>(
                name: "HeadCount",
                table: "OverrideLogCost",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_OverrideLogs_DelayTypes_DelayTypeId",
                table: "OverrideLogs",
                column: "DelayTypeId",
                principalTable: "DelayTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OverrideLogs_DelayTypes_DelayTypeId",
                table: "OverrideLogs");

            migrationBuilder.DropColumn(
                name: "HeadCount",
                table: "OverrideLogCost");

            migrationBuilder.AlterColumn<long>(
                name: "DelayTypeId",
                table: "OverrideLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OverrideLogs_DelayTypes_DelayTypeId",
                table: "OverrideLogs",
                column: "DelayTypeId",
                principalTable: "DelayTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
