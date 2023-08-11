using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataLibrary.Migrations
{
    /// <inheritdoc />
    public partial class removiingadditionalpropertiesfromFCOLog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_FCOLogs_AspNetUsers_AuthorizerForImmediateStartId",
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

            migrationBuilder.DropIndex(
                name: "IX_FCOLogs_AuthorizerForImmediateStartId",
                table: "FCOLogs");

            migrationBuilder.DropIndex(
                name: "IX_FCOLogs_BTLApproverId",
                table: "FCOLogs");

            migrationBuilder.DropIndex(
                name: "IX_FCOLogs_MaintManagerId",
                table: "FCOLogs");

            migrationBuilder.DropIndex(
                name: "IX_FCOLogs_RLTMemberId",
                table: "FCOLogs");

            migrationBuilder.DropIndex(
                name: "IX_FCOLogs_TELApproverId",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "AuthorizerForImmediateStartDate",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "AuthorizerForImmediateStartId",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "BTLApproveDate",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "BTLApproverId",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "MaintManagerApprovalDate",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "MaintManagerId",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "RLTMemberApproveDate",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "RLTMemberId",
                table: "FCOLogs");

            migrationBuilder.DropColumn(
                name: "TELApproverId",
                table: "FCOLogs");

            migrationBuilder.RenameColumn(
                name: "TELApprovalDate",
                table: "FCOLogs",
                newName: "ApprovalDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ApprovalDate",
                table: "FCOLogs",
                newName: "TELApprovalDate");

            migrationBuilder.AddColumn<DateTime>(
                name: "AuthorizerForImmediateStartDate",
                table: "FCOLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "AuthorizerForImmediateStartId",
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
                name: "MaintManagerApprovalDate",
                table: "FCOLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "MaintManagerId",
                table: "FCOLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RLTMemberApproveDate",
                table: "FCOLogs",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "RLTMemberId",
                table: "FCOLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "TELApproverId",
                table: "FCOLogs",
                type: "bigint",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_FCOLogs_AuthorizerForImmediateStartId",
                table: "FCOLogs",
                column: "AuthorizerForImmediateStartId");

            migrationBuilder.CreateIndex(
                name: "IX_FCOLogs_BTLApproverId",
                table: "FCOLogs",
                column: "BTLApproverId");

            migrationBuilder.CreateIndex(
                name: "IX_FCOLogs_MaintManagerId",
                table: "FCOLogs",
                column: "MaintManagerId");

            migrationBuilder.CreateIndex(
                name: "IX_FCOLogs_RLTMemberId",
                table: "FCOLogs",
                column: "RLTMemberId");

            migrationBuilder.CreateIndex(
                name: "IX_FCOLogs_TELApproverId",
                table: "FCOLogs",
                column: "TELApproverId");

            migrationBuilder.AddForeignKey(
                name: "FK_FCOLogs_AspNetUsers_AuthorizerForImmediateStartId",
                table: "FCOLogs",
                column: "AuthorizerForImmediateStartId",
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
        }
    }
}
