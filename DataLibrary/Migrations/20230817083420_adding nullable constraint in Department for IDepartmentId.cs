using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class addingnullableconstraintinDepartmentforIDepartmentId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OverrideLogs_Departments_DepartmentId",
                table: "OverrideLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_Departments_DepartmentId",
                table: "TOTLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_WRRLogs_Departments_DepartmentId",
                table: "WRRLogs");

            migrationBuilder.AlterColumn<long>(
                name: "DepartmentId",
                table: "WRRLogs",
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
                name: "DepartmentId",
                table: "OverrideLogs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_OverrideLogs_Departments_DepartmentId",
                table: "OverrideLogs",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TOTLogs_Departments_DepartmentId",
                table: "TOTLogs",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WRRLogs_Departments_DepartmentId",
                table: "WRRLogs",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_OverrideLogs_Departments_DepartmentId",
                table: "OverrideLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_Departments_DepartmentId",
                table: "TOTLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_WRRLogs_Departments_DepartmentId",
                table: "WRRLogs");

            migrationBuilder.AlterColumn<long>(
                name: "DepartmentId",
                table: "WRRLogs",
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
                name: "DepartmentId",
                table: "OverrideLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_OverrideLogs_Departments_DepartmentId",
                table: "OverrideLogs",
                column: "DepartmentId",
                principalTable: "Departments",
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
                name: "FK_WRRLogs_Departments_DepartmentId",
                table: "WRRLogs",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
