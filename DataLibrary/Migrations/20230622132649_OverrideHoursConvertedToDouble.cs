using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class OverrideHoursConvertedToDouble : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "OverrideHours",
                table: "OverrideLogCost",
                type: "float",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OverrideHours",
                table: "OverrideLogCost",
                type: "int",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
