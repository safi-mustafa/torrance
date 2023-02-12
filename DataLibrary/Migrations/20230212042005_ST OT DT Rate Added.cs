using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class STOTDTRateAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OverrideLogs_OverrideTypes_OverrideTypeId",
                table: "OverrideLogs");

            migrationBuilder.DropIndex(
                name: "IX_OverrideLogs_OverrideTypeId",
                table: "OverrideLogs");

            migrationBuilder.DropColumn(
                name: "OverrideTypeId",
                table: "OverrideLogs");

            migrationBuilder.RenameColumn(
                name: "Rate",
                table: "CraftSkills",
                newName: "STRate");

            migrationBuilder.AddColumn<int>(
                name: "OverrideType",
                table: "OverrideLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<double>(
                name: "DTRate",
                table: "CraftSkills",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "OTRate",
                table: "CraftSkills",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OverrideType",
                table: "OverrideLogs");

            migrationBuilder.DropColumn(
                name: "DTRate",
                table: "CraftSkills");

            migrationBuilder.DropColumn(
                name: "OTRate",
                table: "CraftSkills");

            migrationBuilder.RenameColumn(
                name: "STRate",
                table: "CraftSkills",
                newName: "Rate");

            migrationBuilder.AddColumn<long>(
                name: "OverrideTypeId",
                table: "OverrideLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_OverrideLogs_OverrideTypeId",
                table: "OverrideLogs",
                column: "OverrideTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_OverrideLogs_OverrideTypes_OverrideTypeId",
                table: "OverrideLogs",
                column: "OverrideTypeId",
                principalTable: "OverrideTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
