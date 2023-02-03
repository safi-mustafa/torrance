using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class modifyingoverridelogsdbmodel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OverrideLogs_Contractors_ContractorId",
                table: "OverrideLogs");

            migrationBuilder.DropTable(
                name: "OverrideLogEmployees");

            migrationBuilder.DropColumn(
                name: "Requester",
                table: "OverrideLogs");

            migrationBuilder.RenameColumn(
                name: "RequesterEmail",
                table: "OverrideLogs",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "DateOfWorkCompleted",
                table: "OverrideLogs",
                newName: "WorkCompletedDate");

            migrationBuilder.AlterColumn<string>(
                name: "WorkScope",
                table: "OverrideLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<long>(
                name: "ContractorId",
                table: "OverrideLogs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<long>(
                name: "CompanyId",
                table: "OverrideLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "RequesterId",
                table: "OverrideLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "UnitId",
                table: "OverrideLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_OverrideLogs_CompanyId",
                table: "OverrideLogs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_OverrideLogs_RequesterId",
                table: "OverrideLogs",
                column: "RequesterId");

            migrationBuilder.CreateIndex(
                name: "IX_OverrideLogs_UnitId",
                table: "OverrideLogs",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_OverrideLogs_Companies_CompanyId",
                table: "OverrideLogs",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OverrideLogs_Contractors_ContractorId",
                table: "OverrideLogs",
                column: "ContractorId",
                principalTable: "Contractors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_OverrideLogs_Employees_RequesterId",
                table: "OverrideLogs",
                column: "RequesterId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_OverrideLogs_Units_UnitId",
                table: "OverrideLogs",
                column: "UnitId",
                principalTable: "Units",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OverrideLogs_Companies_CompanyId",
                table: "OverrideLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_OverrideLogs_Contractors_ContractorId",
                table: "OverrideLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_OverrideLogs_Employees_RequesterId",
                table: "OverrideLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_OverrideLogs_Units_UnitId",
                table: "OverrideLogs");

            migrationBuilder.DropIndex(
                name: "IX_OverrideLogs_CompanyId",
                table: "OverrideLogs");

            migrationBuilder.DropIndex(
                name: "IX_OverrideLogs_RequesterId",
                table: "OverrideLogs");

            migrationBuilder.DropIndex(
                name: "IX_OverrideLogs_UnitId",
                table: "OverrideLogs");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "OverrideLogs");

            migrationBuilder.DropColumn(
                name: "RequesterId",
                table: "OverrideLogs");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "OverrideLogs");

            migrationBuilder.RenameColumn(
                name: "WorkCompletedDate",
                table: "OverrideLogs",
                newName: "DateOfWorkCompleted");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "OverrideLogs",
                newName: "RequesterEmail");

            migrationBuilder.AlterColumn<string>(
                name: "WorkScope",
                table: "OverrideLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ContractorId",
                table: "OverrideLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Requester",
                table: "OverrideLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "OverrideLogEmployees",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    OverrideLogId = table.Column<long>(type: "bigint", nullable: false),
                    ActiveStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverrideLogEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OverrideLogEmployees_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OverrideLogEmployees_OverrideLogs_OverrideLogId",
                        column: x => x.OverrideLogId,
                        principalTable: "OverrideLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OverrideLogEmployees_EmployeeId",
                table: "OverrideLogEmployees",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_OverrideLogEmployees_OverrideLogId",
                table: "OverrideLogEmployees",
                column: "OverrideLogId");

            migrationBuilder.AddForeignKey(
                name: "FK_OverrideLogs_Contractors_ContractorId",
                table: "OverrideLogs",
                column: "ContractorId",
                principalTable: "Contractors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
