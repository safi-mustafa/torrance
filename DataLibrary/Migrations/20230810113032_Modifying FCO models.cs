using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class ModifyingFCOmodels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FCOLogs_AspNetUsers_DesignatedCoordinatorId",
                table: "FCOLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_FCOLogs_AspNetUsers_EndorserBTLId",
                table: "FCOLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_FCOLogs_AspNetUsers_EndorserUnitSuperindendantId",
                table: "FCOLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_FCOLogs_Companies_CompanyId",
                table: "FCOLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_FCOLogs_Locations_LocationId",
                table: "FCOLogs");

            migrationBuilder.DropIndex(
                name: "IX_FCOLogs_CompanyId",
                table: "FCOLogs");

            migrationBuilder.DropIndex(
                name: "IX_FCOLogs_LocationId",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "CompanyId",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "DesignatedCoordinationDate",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "EndorsmentBTLDate",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "EndorsmentUnitSuperindendantDate",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "FCOLogs");

            migrationBuilder.RenameColumn(
                name: "EndorserUnitSuperindendantId",
                table: "FCOLogs",
                newName: "TELApproverId");

            migrationBuilder.RenameColumn(
                name: "EndorserBTLId",
                table: "FCOLogs",
                newName: "RLTMemberId");

            migrationBuilder.RenameColumn(
                name: "DesignatedCoordinatorId",
                table: "FCOLogs",
                newName: "MaintManagerId");

            migrationBuilder.RenameIndex(
                name: "IX_FCOLogs_EndorserUnitSuperindendantId",
                table: "FCOLogs",
                newName: "IX_FCOLogs_TELApproverId");

            migrationBuilder.RenameIndex(
                name: "IX_FCOLogs_EndorserBTLId",
                table: "FCOLogs",
                newName: "IX_FCOLogs_RLTMemberId");

            migrationBuilder.RenameIndex(
                name: "IX_FCOLogs_DesignatedCoordinatorId",
                table: "FCOLogs",
                newName: "IX_FCOLogs_MaintManagerId");

            migrationBuilder.AddColumn<double>(
                name: "Estimate",
                table: "FCOSection",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                table: "FCOSection",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "OverrideType",
                table: "FCOSection",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionOfFinding",
                table: "FCOLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<DateTime>(
                name: "AuthorizerForImmediateStartDate",
                table: "FCOLogs",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.AlterColumn<string>(
                name: "AdditionalInformation",
                table: "FCOLogs",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<bool>(
                name: "AnalysisOfAlternatives",
                table: "FCOLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<long>(
                name: "ApproverId",
                table: "FCOLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "BTLApproveDate",
                table: "FCOLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BTLApproverId",
                table: "FCOLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "FCOLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DuringExecution",
                table: "FCOLogs",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "EquipmentFailureReport",
                table: "FCOLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "EquipmentNumber",
                table: "FCOLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FCOReasonId",
                table: "FCOLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FCOTypeId",
                table: "FCOLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Location",
                table: "FCOLogs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "MaintManagerApprovalDate",
                table: "FCOLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RLTMemberApproveDate",
                table: "FCOLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "ScaffoldRequired",
                table: "FCOLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "ShutdownRequired",
                table: "FCOLogs",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "TELApprovalDate",
                table: "FCOLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "TotalCost",
                table: "FCOLogs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalHeadCount",
                table: "FCOLogs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "TotalHours",
                table: "FCOLogs",
                type: "float",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.CreateIndex(
                name: "IX_FCOLogs_ApproverId",
                table: "FCOLogs",
                column: "ApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_FCOLogs_BTLApproverId",
                table: "FCOLogs",
                column: "BTLApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_FCOLogs_FCOReasonId",
                table: "FCOLogs",
                column: "FCOReasonId");

            migrationBuilder.CreateIndex(
                name: "IX_FCOLogs_FCOTypeId",
                table: "FCOLogs",
                column: "FCOTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_FCOLogs_AspNetUsers_ApproverId",
                table: "FCOLogs",
                column: "ApproverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FCOLogs_AspNetUsers_BTLApproverId",
                table: "FCOLogs",
                column: "BTLApproverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FCOLogs_AspNetUsers_MaintManagerId",
                table: "FCOLogs",
                column: "MaintManagerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FCOLogs_AspNetUsers_RLTMemberId",
                table: "FCOLogs",
                column: "RLTMemberId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FCOLogs_AspNetUsers_TELApproverId",
                table: "FCOLogs",
                column: "TELApproverId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FCOLogs_FCOReasons_FCOReasonId",
                table: "FCOLogs",
                column: "FCOReasonId",
                principalTable: "FCOReasons",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FCOLogs_FCOTypes_FCOTypeId",
                table: "FCOLogs",
                column: "FCOTypeId",
                principalTable: "FCOTypes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FCOLogs_AspNetUsers_ApproverId",
                table: "FCOLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_FCOLogs_AspNetUsers_BTLApproverId",
                table: "FCOLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_FCOLogs_AspNetUsers_MaintManagerId",
                table: "FCOLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_FCOLogs_AspNetUsers_RLTMemberId",
                table: "FCOLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_FCOLogs_AspNetUsers_TELApproverId",
                table: "FCOLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_FCOLogs_FCOReasons_FCOReasonId",
                table: "FCOLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_FCOLogs_FCOTypes_FCOTypeId",
                table: "FCOLogs");

            migrationBuilder.DropIndex(
                name: "IX_FCOLogs_ApproverId",
                table: "FCOLogs");

            migrationBuilder.DropIndex(
                name: "IX_FCOLogs_BTLApproverId",
                table: "FCOLogs");

            migrationBuilder.DropIndex(
                name: "IX_FCOLogs_FCOReasonId",
                table: "FCOLogs");

            migrationBuilder.DropIndex(
                name: "IX_FCOLogs_FCOTypeId",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "Estimate",
                table: "FCOSection");

            migrationBuilder.DropColumn(
                name: "Name",
                table: "FCOSection");

            migrationBuilder.DropColumn(
                name: "OverrideType",
                table: "FCOSection");

            migrationBuilder.DropColumn(
                name: "AnalysisOfAlternatives",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "ApproverId",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "BTLApproveDate",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "BTLApproverId",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "DuringExecution",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "EquipmentFailureReport",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "EquipmentNumber",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "FCOReasonId",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "FCOTypeId",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "Location",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "MaintManagerApprovalDate",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "RLTMemberApproveDate",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "ScaffoldRequired",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "ShutdownRequired",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "TELApprovalDate",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "TotalCost",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "TotalHeadCount",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "TotalHours",
                table: "FCOLogs");

            migrationBuilder.RenameColumn(
                name: "TELApproverId",
                table: "FCOLogs",
                newName: "EndorserUnitSuperindendantId");

            migrationBuilder.RenameColumn(
                name: "RLTMemberId",
                table: "FCOLogs",
                newName: "EndorserBTLId");

            migrationBuilder.RenameColumn(
                name: "MaintManagerId",
                table: "FCOLogs",
                newName: "DesignatedCoordinatorId");

            migrationBuilder.RenameIndex(
                name: "IX_FCOLogs_TELApproverId",
                table: "FCOLogs",
                newName: "IX_FCOLogs_EndorserUnitSuperindendantId");

            migrationBuilder.RenameIndex(
                name: "IX_FCOLogs_RLTMemberId",
                table: "FCOLogs",
                newName: "IX_FCOLogs_EndorserBTLId");

            migrationBuilder.RenameIndex(
                name: "IX_FCOLogs_MaintManagerId",
                table: "FCOLogs",
                newName: "IX_FCOLogs_DesignatedCoordinatorId");

            migrationBuilder.AlterColumn<string>(
                name: "DescriptionOfFinding",
                table: "FCOLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AuthorizerForImmediateStartDate",
                table: "FCOLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdditionalInformation",
                table: "FCOLogs",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "CompanyId",
                table: "FCOLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<DateTime>(
                name: "DesignatedCoordinationDate",
                table: "FCOLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndorsmentBTLDate",
                table: "FCOLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "EndorsmentUnitSuperindendantDate",
                table: "FCOLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<long>(
                name: "LocationId",
                table: "FCOLogs",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_FCOLogs_CompanyId",
                table: "FCOLogs",
                column: "CompanyId");

            migrationBuilder.CreateIndex(
                name: "IX_FCOLogs_LocationId",
                table: "FCOLogs",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_FCOLogs_AspNetUsers_DesignatedCoordinatorId",
                table: "FCOLogs",
                column: "DesignatedCoordinatorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FCOLogs_AspNetUsers_EndorserBTLId",
                table: "FCOLogs",
                column: "EndorserBTLId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FCOLogs_AspNetUsers_EndorserUnitSuperindendantId",
                table: "FCOLogs",
                column: "EndorserUnitSuperindendantId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_FCOLogs_Companies_CompanyId",
                table: "FCOLogs",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_FCOLogs_Locations_LocationId",
                table: "FCOLogs",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
