using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MembershipService.API.Migrations
{
    /// <inheritdoc />
    public partial class remove_roomInsance_BasicPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasicPlanRooms_Rooms_RoomInstanceId",
                table: "BasicPlanRooms");

            migrationBuilder.DropIndex(
                name: "IX_BasicPlanRooms_RoomInstanceId",
                table: "BasicPlanRooms");

            migrationBuilder.DropColumn(
                name: "CustomPricePerNight",
                table: "BasicPlanRooms");

            migrationBuilder.DropColumn(
                name: "NightsIncluded",
                table: "BasicPlanRooms");

            migrationBuilder.DropColumn(
                name: "RoomInstanceId",
                table: "BasicPlanRooms");

            migrationBuilder.DropColumn(
                name: "TotalPrice",
                table: "BasicPlanRooms");

            migrationBuilder.RenameColumn(
                name: "LimitPerWeek",
                table: "EntitlementRules",
                newName: "LimitPerPeriod");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "LimitPerPeriod",
                table: "EntitlementRules",
                newName: "LimitPerWeek");

            migrationBuilder.AddColumn<decimal>(
                name: "CustomPricePerNight",
                table: "BasicPlanRooms",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NightsIncluded",
                table: "BasicPlanRooms",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RoomInstanceId",
                table: "BasicPlanRooms",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TotalPrice",
                table: "BasicPlanRooms",
                type: "decimal(18,4)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BasicPlanRooms_RoomInstanceId",
                table: "BasicPlanRooms",
                column: "RoomInstanceId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasicPlanRooms_Rooms_RoomInstanceId",
                table: "BasicPlanRooms",
                column: "RoomInstanceId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
