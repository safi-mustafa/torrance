using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class OverrideLogsadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "OverrideLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Requester = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequesterEmail = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateSubmitted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    TimeSubmitted = table.Column<TimeSpan>(type: "time", nullable: false),
                    DateOfWorkCompleted = table.Column<DateTime>(type: "datetime2", nullable: false),
                    WorkScope = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    OverrideHours = table.Column<int>(type: "int", nullable: false),
                    PONumber = table.Column<long>(type: "bigint", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    ShiftId = table.Column<long>(type: "bigint", nullable: false),
                    ReasonForRequestId = table.Column<long>(type: "bigint", nullable: false),
                    CraftRateId = table.Column<long>(type: "bigint", nullable: false),
                    CraftSkillId = table.Column<long>(type: "bigint", nullable: false),
                    OverrideTypeId = table.Column<long>(type: "bigint", nullable: false),
                    ContractorId = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ActiveStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverrideLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OverrideLogs_Contractors_ContractorId",
                        column: x => x.ContractorId,
                        principalTable: "Contractors",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_OverrideLogs_CraftRates_CraftRateId",
                        column: x => x.CraftRateId,
                        principalTable: "CraftRates",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_OverrideLogs_CraftSkills_CraftSkillId",
                        column: x => x.CraftSkillId,
                        principalTable: "CraftSkills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_OverrideLogs_OverrideTypes_OverrideTypeId",
                        column: x => x.OverrideTypeId,
                        principalTable: "OverrideTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_OverrideLogs_ReasonForRequests_ReasonForRequestId",
                        column: x => x.ReasonForRequestId,
                        principalTable: "ReasonForRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_OverrideLogs_Shifts_ShiftId",
                        column: x => x.ShiftId,
                        principalTable: "Shifts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "OverrideLogEmployees",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: false),
                    OverrideLogId = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ActiveStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OverrideLogEmployees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_OverrideLogEmployees_Employees_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_OverrideLogEmployees_OverrideLogs_OverrideLogId",
                        column: x => x.OverrideLogId,
                        principalTable: "OverrideLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateIndex(
                name: "IX_OverrideLogEmployees_EmployeeId",
                table: "OverrideLogEmployees",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_OverrideLogEmployees_OverrideLogId",
                table: "OverrideLogEmployees",
                column: "OverrideLogId");

            migrationBuilder.CreateIndex(
                name: "IX_OverrideLogs_ContractorId",
                table: "OverrideLogs",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_OverrideLogs_CraftRateId",
                table: "OverrideLogs",
                column: "CraftRateId");

            migrationBuilder.CreateIndex(
                name: "IX_OverrideLogs_CraftSkillId",
                table: "OverrideLogs",
                column: "CraftSkillId");

            migrationBuilder.CreateIndex(
                name: "IX_OverrideLogs_OverrideTypeId",
                table: "OverrideLogs",
                column: "OverrideTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_OverrideLogs_ReasonForRequestId",
                table: "OverrideLogs",
                column: "ReasonForRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_OverrideLogs_ShiftId",
                table: "OverrideLogs",
                column: "ShiftId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "OverrideLogEmployees");

            migrationBuilder.DropTable(
                name: "OverrideLogs");
        }
    }
}
