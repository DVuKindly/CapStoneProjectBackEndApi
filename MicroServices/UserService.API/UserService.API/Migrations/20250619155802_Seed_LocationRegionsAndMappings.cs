using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserService.API.Migrations
{
    /// <inheritdoc />
    public partial class Seed_LocationRegionsAndMappings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "LocationMappings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LocationRegionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MembershipLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocationMappings_LocationRegions_LocationRegionId",
                        column: x => x.LocationRegionId,
                        principalTable: "LocationRegions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "LocationMappings",
                columns: new[] { "Id", "CreatedAt", "LocationRegionId", "MembershipLocationId", "RegionName" },
                values: new object[,]
                {
                    { new Guid("aaaa1111-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("9f38b827-4e1a-4a6e-b8c5-5ff6b759a2a1"), new Guid("10000000-0000-0000-0000-000000000001"), "Hà Nội" },
                    { new Guid("aaaa1111-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("5a418674-9e47-4d19-b827-1e8e2b25c324"), new Guid("10000000-0000-0000-0000-000000000002"), "Hải Phòng" },
                    { new Guid("aaaa1111-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new Guid("f0b2b2d9-5e77-4c7e-a601-2e3b9b740e0c"), new Guid("10000000-0000-0000-0000-000000000003"), "Đà Nẵng" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LocationMappings_LocationRegionId_MembershipLocationId",
                table: "LocationMappings",
                columns: new[] { "LocationRegionId", "MembershipLocationId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LocationMappings");
        }
    }
}
