using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.API.Migrations
{
    /// <inheritdoc />
    public partial class InitUsserBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "BookingId",
                table: "PendingMembershipRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RequireBooking",
                table: "PendingMembershipRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "RoomInstanceId",
                table: "PendingMembershipRequests",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "BookingId",
                table: "Memberships",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RoomInstanceId",
                table: "Memberships",
                type: "uniqueidentifier",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "PendingMembershipRequests");

            migrationBuilder.DropColumn(
                name: "RequireBooking",
                table: "PendingMembershipRequests");

            migrationBuilder.DropColumn(
                name: "RoomInstanceId",
                table: "PendingMembershipRequests");

            migrationBuilder.DropColumn(
                name: "BookingId",
                table: "Memberships");

            migrationBuilder.DropColumn(
                name: "RoomInstanceId",
                table: "Memberships");
        }
    }
}
