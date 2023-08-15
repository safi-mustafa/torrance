using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class AddingmultipleapproversforFCOandcomments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FCOLogs_AspNetUsers_ApproverId",
                table: "FCOLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_FCOSection_Contractors_ContractorId",
                table: "FCOSection");

            migrationBuilder.DropForeignKey(
                name: "FK_FCOSection_CraftSkills_CraftId",
                table: "FCOSection");

            migrationBuilder.DropForeignKey(
                name: "FK_FCOSection_FCOLogs_FCOLogId",
                table: "FCOSection");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FCOSection",
                table: "FCOSection");

            migrationBuilder.RenameTable(
                name: "FCOSection",
                newName: "FCOSections");

            migrationBuilder.RenameColumn(
                name: "ApproverId",
                table: "FCOLogs",
                newName: "BusinessTeamLeaderId");

            migrationBuilder.RenameIndex(
                name: "IX_FCOLogs_ApproverId",
                table: "FCOLogs",
                newName: "IX_FCOLogs_BusinessTeamLeaderId");

            migrationBuilder.RenameIndex(
                name: "IX_FCOSection_FCOLogId",
                table: "FCOSections",
                newName: "IX_FCOSections_FCOLogId");

            migrationBuilder.RenameIndex(
                name: "IX_FCOSection_CraftId",
                table: "FCOSections",
                newName: "IX_FCOSections_CraftId");

            migrationBuilder.RenameIndex(
                name: "IX_FCOSection_ContractorId",
                table: "FCOSections",
                newName: "IX_FCOSections_ContractorId");

            migrationBuilder.AddColumn<DateTime>(
                name: "AreaExecutionLeadApprovalDate",
                table: "FCOLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "AreaExecutionLeadId",
                table: "FCOLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BusinessTeamLeaderApprovalDate",
                table: "FCOLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<double>(
                name: "Contingency",
                table: "FCOLogs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_FCOSections",
                table: "FCOSections",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "FCOComments",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Comment = table.Column<string>(type: "nvarchar(max)", nullable: false),
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
                    table.PrimaryKey("PK_FCOComments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FCOComments_FCOLogs_FCOLogId",
                        column: x => x.FCOLogId,
                        principalTable: "FCOLogs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FCOLogs_AreaExecutionLeadId",
                table: "FCOLogs",
                column: "AreaExecutionLeadId");

            migrationBuilder.CreateIndex(
                name: "IX_FCOComments_FCOLogId",
                table: "FCOComments",
                column: "FCOLogId");

            migrationBuilder.AddForeignKey(
                name: "FK_FCOLogs_AspNetUsers_AreaExecutionLeadId",
                table: "FCOLogs",
                column: "AreaExecutionLeadId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FCOLogs_AspNetUsers_BusinessTeamLeaderId",
                table: "FCOLogs",
                column: "BusinessTeamLeaderId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FCOSections_Contractors_ContractorId",
                table: "FCOSections",
                column: "ContractorId",
                principalTable: "Contractors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FCOSections_CraftSkills_CraftId",
                table: "FCOSections",
                column: "CraftId",
                principalTable: "CraftSkills",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FCOSections_FCOLogs_FCOLogId",
                table: "FCOSections",
                column: "FCOLogId",
                principalTable: "FCOLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FCOLogs_AspNetUsers_AreaExecutionLeadId",
                table: "FCOLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_FCOLogs_AspNetUsers_BusinessTeamLeaderId",
                table: "FCOLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_FCOSections_Contractors_ContractorId",
                table: "FCOSections");

            migrationBuilder.DropForeignKey(
                name: "FK_FCOSections_CraftSkills_CraftId",
                table: "FCOSections");

            migrationBuilder.DropForeignKey(
                name: "FK_FCOSections_FCOLogs_FCOLogId",
                table: "FCOSections");

            migrationBuilder.DropTable(
                name: "FCOComments");

            migrationBuilder.DropIndex(
                name: "IX_FCOLogs_AreaExecutionLeadId",
                table: "FCOLogs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_FCOSections",
                table: "FCOSections");

            migrationBuilder.DropColumn(
                name: "AreaExecutionLeadApprovalDate",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "AreaExecutionLeadId",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "BusinessTeamLeaderApprovalDate",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "Contingency",
                table: "FCOLogs");

            migrationBuilder.RenameTable(
                name: "FCOSections",
                newName: "FCOSection");

            migrationBuilder.RenameColumn(
                name: "BusinessTeamLeaderId",
                table: "FCOLogs",
                newName: "ApproverId");

            migrationBuilder.RenameIndex(
                name: "IX_FCOLogs_BusinessTeamLeaderId",
                table: "FCOLogs",
                newName: "IX_FCOLogs_ApproverId");

            migrationBuilder.RenameIndex(
                name: "IX_FCOSections_FCOLogId",
                table: "FCOSection",
                newName: "IX_FCOSection_FCOLogId");

            migrationBuilder.RenameIndex(
                name: "IX_FCOSections_CraftId",
                table: "FCOSection",
                newName: "IX_FCOSection_CraftId");

            migrationBuilder.RenameIndex(
                name: "IX_FCOSections_ContractorId",
                table: "FCOSection",
                newName: "IX_FCOSection_ContractorId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_FCOSection",
                table: "FCOSection",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FCOLogs_AspNetUsers_ApproverId",
                table: "FCOLogs",
                column: "ApproverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FCOSection_Contractors_ContractorId",
                table: "FCOSection",
                column: "ContractorId",
                principalTable: "Contractors",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FCOSection_CraftSkills_CraftId",
                table: "FCOSection",
                column: "CraftId",
                principalTable: "CraftSkills",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FCOSection_FCOLogs_FCOLogId",
                table: "FCOSection",
                column: "FCOLogId",
                principalTable: "FCOLogs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
