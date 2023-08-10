using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class CraftaddedtoFCOSection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "CraftId",
                table: "FCOSection",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FCOSection_CraftId",
                table: "FCOSection",
                column: "CraftId");

            migrationBuilder.AddForeignKey(
                name: "FK_FCOSection_CraftSkills_CraftId",
                table: "FCOSection",
                column: "CraftId",
                principalTable: "CraftSkills",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FCOSection_CraftSkills_CraftId",
                table: "FCOSection");

            migrationBuilder.DropIndex(
                name: "IX_FCOSection_CraftId",
                table: "FCOSection");

            migrationBuilder.DropColumn(
                name: "CraftId",
                table: "FCOSection");
        }
    }
}
