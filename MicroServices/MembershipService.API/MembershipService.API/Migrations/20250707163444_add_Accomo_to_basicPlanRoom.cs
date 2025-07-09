using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MembershipService.API.Migrations
{
    /// <inheritdoc />
    public partial class add_Accomo_to_basicPlanRoom : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "RoomInstanceId",
                table: "BasicPlanRooms",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AlterColumn<int>(
                name: "NightsIncluded",
                table: "BasicPlanRooms",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<Guid>(
                name: "AccommodationOptionId",
                table: "BasicPlanRooms",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "AccommodationOptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BasicPlanRooms_AccommodationOptionId",
                table: "BasicPlanRooms",
                column: "AccommodationOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasicPlanRooms_AccommodationOptions_AccommodationOptionId",
                table: "BasicPlanRooms",
                column: "AccommodationOptionId",
                principalTable: "AccommodationOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasicPlanRooms_AccommodationOptions_AccommodationOptionId",
                table: "BasicPlanRooms");

            migrationBuilder.DropIndex(
                name: "IX_BasicPlanRooms_AccommodationOptionId",
                table: "BasicPlanRooms");

            migrationBuilder.DropColumn(
                name: "AccommodationOptionId",
                table: "BasicPlanRooms");

            migrationBuilder.AlterColumn<Guid>(
                name: "RoomInstanceId",
                table: "BasicPlanRooms",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "NightsIncluded",
                table: "BasicPlanRooms",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "AccommodationOptions",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");
        }
    }
}
