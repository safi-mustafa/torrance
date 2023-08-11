using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class addingcoordinatortoFCOLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DesignatedCoordinatorId",
                table: "FCOLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FCOLogs_DesignatedCoordinatorId",
                table: "FCOLogs",
                column: "DesignatedCoordinatorId");

            migrationBuilder.AddForeignKey(
                name: "FK_FCOLogs_AspNetUsers_DesignatedCoordinatorId",
                table: "FCOLogs",
                column: "DesignatedCoordinatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FCOLogs_AspNetUsers_DesignatedCoordinatorId",
                table: "FCOLogs");

            migrationBuilder.DropIndex(
                name: "IX_FCOLogs_DesignatedCoordinatorId",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "DesignatedCoordinatorId",
                table: "FCOLogs");
        }
    }
}
