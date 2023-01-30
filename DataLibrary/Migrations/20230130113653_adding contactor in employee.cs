using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class addingcontactorinemployee : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "TerminationDate",
                table: "Employees",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfHire",
                table: "Employees",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AddColumn<long>(
                name: "ContractorId",
                table: "Employees",
                type: "bigint",
                nullable: true,
                defaultValue: 0L);

            migrationBuilder.AddColumn<bool>(
                name: "IsApprover",
                table: "Employees",
                type: "bit",
                nullable: true,
                defaultValue: false);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_ContractorId",
                table: "Employees",
                column: "ContractorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Employees_Contractors_ContractorId",
                table: "Employees",
                column: "ContractorId",
                principalTable: "Contractors",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Employees_Contractors_ContractorId",
                table: "Employees");

            migrationBuilder.DropIndex(
                name: "IX_Employees_ContractorId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "ContractorId",
                table: "Employees");

            migrationBuilder.DropColumn(
                name: "IsApprover",
                table: "Employees");

            migrationBuilder.AlterColumn<DateTime>(
                name: "TerminationDate",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "DateOfHire",
                table: "Employees",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);
        }
    }
}
