using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class NullablesOverrideLogCostCraftSkill : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OverrideLogCost_CraftSkills_CraftSkillId",
                table: "OverrideLogCost");

            migrationBuilder.AlterColumn<long>(
                name: "CraftSkillId",
                table: "OverrideLogCost",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_OverrideLogCost_CraftSkills_CraftSkillId",
                table: "OverrideLogCost",
                column: "CraftSkillId",
                principalTable: "CraftSkills",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OverrideLogCost_CraftSkills_CraftSkillId",
                table: "OverrideLogCost");

            migrationBuilder.AlterColumn<long>(
                name: "CraftSkillId",
                table: "OverrideLogCost",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OverrideLogCost_CraftSkills_CraftSkillId",
                table: "OverrideLogCost",
                column: "CraftSkillId",
                principalTable: "CraftSkills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
