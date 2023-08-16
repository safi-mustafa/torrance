using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class ModifyingFCOLogforEquipmentShopMaterial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "EquipmentName",
                table: "FCOLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "EquipmentRate",
                table: "FCOLogs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "MaterialName",
                table: "FCOLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "MaterialRate",
                table: "FCOLogs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "ShopName",
                table: "FCOLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<double>(
                name: "ShopRate",
                table: "FCOLogs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EquipmentName",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "EquipmentRate",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "MaterialName",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "MaterialRate",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "ShopName",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "ShopRate",
                table: "FCOLogs");
        }
    }
}
