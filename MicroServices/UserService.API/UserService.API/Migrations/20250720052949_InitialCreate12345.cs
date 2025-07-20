using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UserService.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate12345 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Propertys_Locations_LocationId",
                table: "Propertys");

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId1",
                table: "Propertys",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CityId1",
                table: "Locations",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Locations",
                keyColumn: "Id",
                keyValue: new Guid("20000000-0000-0000-0000-000000000001"),
                column: "CityId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "Propertys",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"),
                column: "LocationId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "Propertys",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000002"),
                column: "LocationId1",
                value: null);

            migrationBuilder.UpdateData(
                table: "Propertys",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000003"),
                column: "LocationId1",
                value: null);

            migrationBuilder.CreateIndex(
                name: "IX_Propertys_LocationId1",
                table: "Propertys",
                column: "LocationId1");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_CityId1",
                table: "Locations",
                column: "CityId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Locations_Cities_CityId1",
                table: "Locations",
                column: "CityId1",
                principalTable: "Cities",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Propertys_Locations_LocationId",
                table: "Propertys",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Propertys_Locations_LocationId1",
                table: "Propertys",
                column: "LocationId1",
                principalTable: "Locations",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Locations_Cities_CityId1",
                table: "Locations");

            migrationBuilder.DropForeignKey(
                name: "FK_Propertys_Locations_LocationId",
                table: "Propertys");

            migrationBuilder.DropForeignKey(
                name: "FK_Propertys_Locations_LocationId1",
                table: "Propertys");

            migrationBuilder.DropIndex(
                name: "IX_Propertys_LocationId1",
                table: "Propertys");

            migrationBuilder.DropIndex(
                name: "IX_Locations_CityId1",
                table: "Locations");

            migrationBuilder.DropColumn(
                name: "LocationId1",
                table: "Propertys");

            migrationBuilder.DropColumn(
                name: "CityId1",
                table: "Locations");

            migrationBuilder.AddForeignKey(
                name: "FK_Propertys_Locations_LocationId",
                table: "Propertys",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id");
        }
    }
}
