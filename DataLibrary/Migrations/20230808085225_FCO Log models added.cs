using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class FCOLogmodelsadded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "CompositeRate",
                table: "Contractors",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateTable(
                name: "FCOLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Status = table.Column<int>(type: "int", nullable: false),
                    DescriptionOfFinding = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AdditionalInformation = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SrNo = table.Column<long>(type: "bigint", nullable: false),
                    PAndIdAttached = table.Column<bool>(type: "bit", nullable: false),
                    ISOAttached = table.Column<bool>(type: "bit", nullable: false),
                    DrawingsAttached = table.Column<bool>(type: "bit", nullable: false),
                    ScheduleImpact = table.Column<bool>(type: "bit", nullable: false),
                    DaysImpacted = table.Column<long>(type: "bigint", nullable: false),
                    EmployeeId = table.Column<long>(type: "bigint", nullable: true),
                    DepartmentId = table.Column<long>(type: "bigint", nullable: false),
                    UnitId = table.Column<long>(type: "bigint", nullable: false),
                    LocationId = table.Column<long>(type: "bigint", nullable: false),
                    ContractorId = table.Column<long>(type: "bigint", nullable: true),
                    CompanyId = table.Column<long>(type: "bigint", nullable: false),
                    AuthorizerForImmediateStartId = table.Column<long>(type: "bigint", nullable: true),
                    AuthorizerForImmediateStartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    DesignatedCoordinatorId = table.Column<long>(type: "bigint", nullable: true),
                    DesignatedCoordinationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndorserBTLId = table.Column<long>(type: "bigint", nullable: true),
                    EndorsmentBTLDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndorserUnitSuperindendantId = table.Column<long>(type: "bigint", nullable: true),
                    EndorsmentUnitSuperindendantDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ActiveStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FCOLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FCOLogs_AspNetUsers_AuthorizerForImmediateStartId",
                        column: x => x.AuthorizerForImmediateStartId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FCOLogs_AspNetUsers_DesignatedCoordinatorId",
                        column: x => x.DesignatedCoordinatorId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FCOLogs_AspNetUsers_EmployeeId",
                        column: x => x.EmployeeId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FCOLogs_AspNetUsers_EndorserBTLId",
                        column: x => x.EndorserBTLId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FCOLogs_AspNetUsers_EndorserUnitSuperindendantId",
                        column: x => x.EndorserUnitSuperindendantId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FCOLogs_Companies_CompanyId",
                        column: x => x.CompanyId,
                        principalTable: "Companies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_FCOLogs_Contractors_ContractorId",
                        column: x => x.ContractorId,
                        principalTable: "Contractors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FCOLogs_Departments_DepartmentId",
                        column: x => x.DepartmentId,
                        principalTable: "Departments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_FCOLogs_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                    table.ForeignKey(
                        name: "FK_FCOLogs_Units_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Units",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.NoAction);
                });

            migrationBuilder.CreateTable(
                name: "FCOReasons",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ActiveStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FCOReasons", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FCOTypes",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ActiveStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FCOTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "FCOSection",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SectionType = table.Column<int>(type: "int", nullable: false),
                    DU = table.Column<double>(type: "float", nullable: false),
                    MN = table.Column<double>(type: "float", nullable: false),
                    Rate = table.Column<double>(type: "float", nullable: false),
                    ContractorId = table.Column<long>(type: "bigint", nullable: true),
                    FCOLogId = table.Column<long>(type: "bigint", nullable: false),
                    IsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    ActiveStatus = table.Column<int>(type: "int", nullable: false),
                    CreatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CreatedBy = table.Column<long>(type: "bigint", nullable: false),
                    UpdatedOn = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedBy = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FCOSection", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FCOSection_Contractors_ContractorId",
                        column: x => x.ContractorId,
                        principalTable: "Contractors",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_FCOSection_FCOLogs_FCOLogId",
                        column: x => x.FCOLogId,
                        principalTable: "FCOLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FCOLogs_AuthorizerForImmediateStartId",
                table: "FCOLogs",
                column: "AuthorizerForImmediateStartId");

            migrationBuilder.CreateIndex(
                name: "IX_FCOLogs_CompanyId",
                table: "FCOLogs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_FCOLogs_ContractorId",
                table: "FCOLogs",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_FCOLogs_DepartmentId",
                table: "FCOLogs",
                column: "DepartmentId");

            migrationBuilder.CreateIndex(
                name: "IX_FCOLogs_DesignatedCoordinatorId",
                table: "FCOLogs",
                column: "DesignatedCoordinatorId");

            migrationBuilder.CreateIndex(
                name: "IX_FCOLogs_EmployeeId",
                table: "FCOLogs",
                column: "EmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_FCOLogs_EndorserBTLId",
                table: "FCOLogs",
                column: "EndorserBTLId");

            migrationBuilder.CreateIndex(
                name: "IX_FCOLogs_EndorserUnitSuperindendantId",
                table: "FCOLogs",
                column: "EndorserUnitSuperindendantId");

            migrationBuilder.CreateIndex(
                name: "IX_FCOLogs_LocationId",
                table: "FCOLogs",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_FCOLogs_UnitId",
                table: "FCOLogs",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_FCOSection_ContractorId",
                table: "FCOSection",
                column: "ContractorId");

            migrationBuilder.CreateIndex(
                name: "IX_FCOSection_FCOLogId",
                table: "FCOSection",
                column: "FCOLogId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FCOReasons");

            migrationBuilder.DropTable(
                name: "FCOSection");

            migrationBuilder.DropTable(
                name: "FCOTypes");

            migrationBuilder.DropTable(
                name: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "CompositeRate",
                table: "Contractors");
        }
    }
}
