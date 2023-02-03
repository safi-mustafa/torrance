using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class RemovedRequiredFromRelationshipsInTOTLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_Contractors_ContractorId",
                table: "TOTLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_Departments_DepartmentId",
                table: "TOTLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_PermittingIssues_PermittingIssueId",
                table: "TOTLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_ReworkDelays_ReworkDelayId",
                table: "TOTLogs");

            migrationBuilder.AlterColumn<long>(
                name: "ReworkDelayId",
                table: "TOTLogs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "PermittingIssueId",
                table: "TOTLogs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "DepartmentId",
                table: "TOTLogs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "ContractorId",
                table: "TOTLogs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_TOTLogs_Contractors_ContractorId",
                table: "TOTLogs",
                column: "ContractorId",
                principalTable: "Contractors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TOTLogs_Departments_DepartmentId",
                table: "TOTLogs",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TOTLogs_PermittingIssues_PermittingIssueId",
                table: "TOTLogs",
                column: "PermittingIssueId",
                principalTable: "PermittingIssues",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TOTLogs_ReworkDelays_ReworkDelayId",
                table: "TOTLogs",
                column: "ReworkDelayId",
                principalTable: "ReworkDelays",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_Contractors_ContractorId",
                table: "TOTLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_Departments_DepartmentId",
                table: "TOTLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_PermittingIssues_PermittingIssueId",
                table: "TOTLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_ReworkDelays_ReworkDelayId",
                table: "TOTLogs");

            migrationBuilder.AlterColumn<long>(
                name: "ReworkDelayId",
                table: "TOTLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PermittingIssueId",
                table: "TOTLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "DepartmentId",
                table: "TOTLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ContractorId",
                table: "TOTLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TOTLogs_Contractors_ContractorId",
                table: "TOTLogs",
                column: "ContractorId",
                principalTable: "Contractors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TOTLogs_Departments_DepartmentId",
                table: "TOTLogs",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TOTLogs_PermittingIssues_PermittingIssueId",
                table: "TOTLogs",
                column: "PermittingIssueId",
                principalTable: "PermittingIssues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TOTLogs_ReworkDelays_ReworkDelayId",
                table: "TOTLogs",
                column: "ReworkDelayId",
                principalTable: "ReworkDelays",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
