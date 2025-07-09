using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserService.API.Migrations
{
    /// <inheritdoc />
    public partial class AddStartDateToPendingMembershipRequest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "LocationRegions",
                keyColumn: "Id",
                keyValue: new Guid("5a418674-9e47-4d19-b827-1e8e2b25c324"));

            migrationBuilder.DeleteData(
                table: "LocationRegions",
                keyColumn: "Id",
                keyValue: new Guid("9f38b827-4e1a-4a6e-b8c5-5ff6b759a2a1"));

            migrationBuilder.DeleteData(
                table: "LocationRegions",
                keyColumn: "Id",
                keyValue: new Guid("f0b2b2d9-5e77-4c7e-a601-2e3b9b740e0c"));

            migrationBuilder.AddColumn<DateTime>(
                name: "StartDate",
                table: "PendingMembershipRequests",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StartDate",
                table: "PendingMembershipRequests");

            migrationBuilder.InsertData(
                table: "LocationRegions",
                columns: new[] { "Id", "CreatedAt", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("5a418674-9e47-4d19-b827-1e8e2b25c324"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Khu vực Hải Phòng", "Hải Phòng" },
                    { new Guid("9f38b827-4e1a-4a6e-b8c5-5ff6b759a2a1"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Khu vực Hà Nội", "Hà Nội" },
                    { new Guid("f0b2b2d9-5e77-4c7e-a601-2e3b9b740e0c"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Khu vực Đà Nẵng", "Đà Nẵng" }
                });
        }
    }
}
