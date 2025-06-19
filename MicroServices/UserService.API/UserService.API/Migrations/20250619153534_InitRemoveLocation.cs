using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.API.Migrations
{
    /// <inheritdoc />
    public partial class InitRemoveLocation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PendingMembershipRequests_LocationRegions_LocationId",
                table: "PendingMembershipRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PendingThirdPartyRequests_LocationRegions_LocationId",
                table: "PendingThirdPartyRequests");

            migrationBuilder.DropIndex(
                name: "IX_PendingMembershipRequests_LocationId",
                table: "PendingMembershipRequests");

            migrationBuilder.AddColumn<Guid>(
                name: "LocationRegionId",
                table: "PendingMembershipRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PendingMembershipRequests_LocationRegionId",
                table: "PendingMembershipRequests",
                column: "LocationRegionId");

            migrationBuilder.AddForeignKey(
                name: "FK_PendingMembershipRequests_LocationRegions_LocationRegionId",
                table: "PendingMembershipRequests",
                column: "LocationRegionId",
                principalTable: "LocationRegions",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_PendingThirdPartyRequests_LocationRegions_LocationId",
                table: "PendingThirdPartyRequests",
                column: "LocationId",
                principalTable: "LocationRegions",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PendingMembershipRequests_LocationRegions_LocationRegionId",
                table: "PendingMembershipRequests");

            migrationBuilder.DropForeignKey(
                name: "FK_PendingThirdPartyRequests_LocationRegions_LocationId",
                table: "PendingThirdPartyRequests");

            migrationBuilder.DropIndex(
                name: "IX_PendingMembershipRequests_LocationRegionId",
                table: "PendingMembershipRequests");

            migrationBuilder.DropColumn(
                name: "LocationRegionId",
                table: "PendingMembershipRequests");

            migrationBuilder.CreateIndex(
                name: "IX_PendingMembershipRequests_LocationId",
                table: "PendingMembershipRequests",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_PendingMembershipRequests_LocationRegions_LocationId",
                table: "PendingMembershipRequests",
                column: "LocationId",
                principalTable: "LocationRegions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);

            migrationBuilder.AddForeignKey(
                name: "FK_PendingThirdPartyRequests_LocationRegions_LocationId",
                table: "PendingThirdPartyRequests",
                column: "LocationId",
                principalTable: "LocationRegions",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }
    }
}
