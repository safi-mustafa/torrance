using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class addingnullabledelayTypeintotlogsdbmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "DelayTypeId",
                table: "TOTLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TOTLogs_DelayTypeId",
                table: "TOTLogs",
                column: "DelayTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_TOTLogs_DelayTypes_DelayTypeId",
                table: "TOTLogs",
                column: "DelayTypeId",
                principalTable: "DelayTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_DelayTypes_DelayTypeId",
                table: "TOTLogs");

            migrationBuilder.DropIndex(
                name: "IX_TOTLogs_DelayTypeId",
                table: "TOTLogs");

            migrationBuilder.DropColumn(
                name: "DelayTypeId",
                table: "TOTLogs");
        }
    }
}
