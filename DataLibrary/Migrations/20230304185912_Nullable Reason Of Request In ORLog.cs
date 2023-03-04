using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class NullableReasonOfRequestInORLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OverrideLogs_ReasonForRequests_ReasonForRequestId",
                table: "OverrideLogs");

            migrationBuilder.AlterColumn<long>(
                name: "ReasonForRequestId",
                table: "OverrideLogs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_OverrideLogs_ReasonForRequests_ReasonForRequestId",
                table: "OverrideLogs",
                column: "ReasonForRequestId",
                principalTable: "ReasonForRequests",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OverrideLogs_ReasonForRequests_ReasonForRequestId",
                table: "OverrideLogs");

            migrationBuilder.AlterColumn<long>(
                name: "ReasonForRequestId",
                table: "OverrideLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OverrideLogs_ReasonForRequests_ReasonForRequestId",
                table: "OverrideLogs",
                column: "ReasonForRequestId",
                principalTable: "ReasonForRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
