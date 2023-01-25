using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class modifiedlogmodels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_AspNetUsers_ApproverId",
                table: "TOTLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_AspNetUsers_ForemanId",
                table: "TOTLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_WRRLogs_Employees_EmployeeId",
                table: "WRRLogs");

            migrationBuilder.RenameColumn(
                name: "ManPower",
                table: "TOTLogs",
                newName: "PermittingIssueId");

            migrationBuilder.AlterColumn<double>(
                name: "RodReturnedWasteLbs",
                table: "WRRLogs",
                type: "float",
                nullable: true,
                oldClrType: typeof(double),
                oldType: "float");

            migrationBuilder.AlterColumn<int>(
                name: "FumeControlUsed",
                table: "WRRLogs",
                type: "int",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<long>(
                name: "EmployeeId",
                table: "WRRLogs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "WRRLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<long>(
                name: "ManHours",
                table: "TOTLogs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "ForemanId",
                table: "TOTLogs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<string>(
                name: "DelayReason",
                table: "TOTLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<long>(
                name: "ApproverId",
                table: "TOTLogs",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "Comment",
                table: "TOTLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ManPowerAffected",
                table: "TOTLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_TOTLogs_PermittingIssueId",
                table: "TOTLogs",
                column: "PermittingIssueId");

            migrationBuilder.AddForeignKey(
                name: "FK_TOTLogs_AspNetUsers_ApproverId",
                table: "TOTLogs",
                column: "ApproverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TOTLogs_AspNetUsers_ForemanId",
                table: "TOTLogs",
                column: "ForemanId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_TOTLogs_PermittingIssues_PermittingIssueId",
                table: "TOTLogs",
                column: "PermittingIssueId",
                principalTable: "PermittingIssues",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WRRLogs_Employees_EmployeeId",
                table: "WRRLogs",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_AspNetUsers_ApproverId",
                table: "TOTLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_AspNetUsers_ForemanId",
                table: "TOTLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_TOTLogs_PermittingIssues_PermittingIssueId",
                table: "TOTLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_WRRLogs_Employees_EmployeeId",
                table: "WRRLogs");

            migrationBuilder.DropIndex(
                name: "IX_TOTLogs_PermittingIssueId",
                table: "TOTLogs");

            migrationBuilder.DropColumn(
                name: "Comment",
                table: "TOTLogs");

            migrationBuilder.DropColumn(
                name: "ManPowerAffected",
                table: "TOTLogs");

            migrationBuilder.RenameColumn(
                name: "PermittingIssueId",
                table: "TOTLogs",
                newName: "ManPower");

            migrationBuilder.AlterColumn<double>(
                name: "RodReturnedWasteLbs",
                table: "WRRLogs",
                type: "float",
                nullable: false,
                defaultValue: 0.0,
                oldClrType: typeof(double),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FumeControlUsed",
                table: "WRRLogs",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "EmployeeId",
                table: "WRRLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "WRRLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ManHours",
                table: "TOTLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ForemanId",
                table: "TOTLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DelayReason",
                table: "TOTLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "ApproverId",
                table: "TOTLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TOTLogs_AspNetUsers_ApproverId",
                table: "TOTLogs",
                column: "ApproverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TOTLogs_AspNetUsers_ForemanId",
                table: "TOTLogs",
                column: "ForemanId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_WRRLogs_Employees_EmployeeId",
                table: "WRRLogs",
                column: "EmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
