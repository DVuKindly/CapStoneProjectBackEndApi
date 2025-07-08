using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.API.Migrations
{
    /// <inheritdoc />
    public partial class AddStartDateToPendingMembershipRequest23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "PendingRequestId",
                table: "Memberships",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_PendingRequestId",
                table: "Memberships",
                column: "PendingRequestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Memberships_PendingMembershipRequests_PendingRequestId",
                table: "Memberships",
                column: "PendingRequestId",
                principalTable: "PendingMembershipRequests",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memberships_PendingMembershipRequests_PendingRequestId",
                table: "Memberships");

            migrationBuilder.DropIndex(
                name: "IX_Memberships_PendingRequestId",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "PendingRequestId",
                table: "Memberships");
        }
    }
}
