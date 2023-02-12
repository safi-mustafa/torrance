using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class OverrideLogCostAddedToDBSet : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OverrideLogCost",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    OverrideLogId = table.Column<long>(type: "bigint", nullable: false),
                    OverrideType = table.Column<int>(type: "int", nullable: false),
                    CraftSkillId = table.Column<long>(type: "bigint", nullable: false),
                    OverrideHours = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverrideLogCost", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OverrideLogCost_CraftSkills_CraftSkillId",
                        column: x => x.CraftSkillId,
                        principalTable: "CraftSkills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OverrideLogCost_OverrideLogs_OverrideLogId",
                        column: x => x.OverrideLogId,
                        principalTable: "OverrideLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OverrideLogCost_CraftSkillId",
                table: "OverrideLogCost",
                column: "CraftSkillId");

            migrationBuilder.CreateIndex(
                name: "IX_OverrideLogCost_OverrideLogId",
                table: "OverrideLogCost",
                column: "OverrideLogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OverrideLogCost");
        }
    }
}
