using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class RemovedCraftRate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OverrideLogs_CraftRates_CraftRateId",
                table: "OverrideLogs");

            migrationBuilder.DropTable(
                name: "CraftRates");

            migrationBuilder.DropIndex(
                name: "IX_OverrideLogs_CraftRateId",
                table: "OverrideLogs");

            migrationBuilder.DropColumn(
                name: "CraftRateId",
                table: "OverrideLogs");

            migrationBuilder.AddColumn<double>(
                name: "Rate",
                table: "CraftSkills",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Rate",
                table: "CraftSkills");

            migrationBuilder.AddColumn<long>(
                name: "CraftRateId",
                table: "OverrideLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "CraftRates",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ActiveStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    Rate = table.Column<float>(type: "real", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CraftRates", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OverrideLogs_CraftRateId",
                table: "OverrideLogs",
                column: "CraftRateId");

            migrationBuilder.AddForeignKey(
                name: "FK_OverrideLogs_CraftRates_CraftRateId",
                table: "OverrideLogs",
                column: "CraftRateId",
                principalTable: "CraftRates",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
