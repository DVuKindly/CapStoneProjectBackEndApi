using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MembershipService.API.Migrations
{
    /// <inheritdoc />
    public partial class AddBasicPlanRoomRoomInstanceFK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasicPlanRooms_Rooms_RoomInstanceId",
                table: "BasicPlanRooms");

            migrationBuilder.DropColumn(
                name: "RoomId",
                table: "BasicPlanRooms");

            migrationBuilder.AddForeignKey(
                name: "FK_BasicPlanRooms_Rooms_RoomInstanceId",
                table: "BasicPlanRooms",
                column: "RoomInstanceId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasicPlanRooms_Rooms_RoomInstanceId",
                table: "BasicPlanRooms");

            migrationBuilder.AddColumn<Guid>(
                name: "RoomId",
                table: "BasicPlanRooms",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddForeignKey(
                name: "FK_BasicPlanRooms_Rooms_RoomInstanceId",
                table: "BasicPlanRooms",
                column: "RoomInstanceId",
                principalTable: "Rooms",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
