using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class OverrideLogCostAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OverrideLogs_CraftSkills_CraftSkillId",
                table: "OverrideLogs");

            migrationBuilder.DropIndex(
                name: "IX_OverrideLogs_CraftSkillId",
                table: "OverrideLogs");

            migrationBuilder.DropColumn(
                name: "CraftSkillId",
                table: "OverrideLogs");

            migrationBuilder.DropColumn(
                name: "OverrideType",
                table: "OverrideLogs");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CraftSkillId",
                table: "OverrideLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "OverrideType",
                table: "OverrideLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_OverrideLogs_CraftSkillId",
                table: "OverrideLogs",
                column: "CraftSkillId");

            migrationBuilder.AddForeignKey(
                name: "FK_OverrideLogs_CraftSkills_CraftSkillId",
                table: "OverrideLogs",
                column: "CraftSkillId",
                principalTable: "CraftSkills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
