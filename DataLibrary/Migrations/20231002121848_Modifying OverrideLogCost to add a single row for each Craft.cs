using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class ModifyingOverrideLogCosttoaddasinglerowforeachCraft : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "DTHours",
                table: "OverrideLogCost",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "OTHours",
                table: "OverrideLogCost",
                type: "float",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "STHours",
                table: "OverrideLogCost",
                type: "float",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DTHours",
                table: "OverrideLogCost");

            migrationBuilder.DropColumn(
                name: "OTHours",
                table: "OverrideLogCost");

            migrationBuilder.DropColumn(
                name: "STHours",
                table: "OverrideLogCost");
        }
    }
}
