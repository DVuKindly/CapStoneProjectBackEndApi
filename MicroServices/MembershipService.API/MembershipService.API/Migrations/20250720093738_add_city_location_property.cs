using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MembershipService.API.Migrations
{
    /// <inheritdoc />
    public partial class add_city_location_property : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Propertys",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Propertys",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Propertys",
                keyColumn: "Id",
                keyValue: new Guid("10000000-0000-0000-0000-000000000003"));

            migrationBuilder.DropColumn(
                name: "Code",
                table: "Propertys");

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "Propertys",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId1",
                table: "Propertys",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CityId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Locations_Cities_CityId1",
                        column: x => x.CityId1,
                        principalTable: "Cities",
                        principalColumn: "Id");
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CreatedAt", "Description", "Name" },
                values: new object[] { new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thủ đô Việt Nam", "Hà Nội" });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "CityId", "CityId1", "CreatedAt", "Name" },
                values: new object[] { new Guid("20000000-0000-0000-0000-000000000001"), new Guid("10000000-0000-0000-0000-000000000001"), null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Hoàng Cầu" });

            migrationBuilder.InsertData(
                table: "Propertys",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "LocationId", "LocationId1", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("30000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Khu vực trụ sở chính Hoàng Cầu 1", new Guid("20000000-0000-0000-0000-000000000001"), null, "Hoàng Cầu Cơ sở 1", null, null },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Khu vực trụ sở chính Hoàng Cầu 2", new Guid("20000000-0000-0000-0000-000000000001"), null, "Hoàng Cầu Cơ sở 2", null, null },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Khu vực trụ sở chính Hoàng Cầu 3", new Guid("20000000-0000-0000-0000-000000000001"), null, "Hoàng Cầu Cơ sở 3", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Propertys_LocationId",
                table: "Propertys",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Propertys_LocationId1",
                table: "Propertys",
                column: "LocationId1");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_CityId",
                table: "Locations",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_CityId1",
                table: "Locations",
                column: "CityId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Propertys_Locations_LocationId",
                table: "Propertys",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_Propertys_Locations_LocationId",
                table: "Propertys");

            migrationBuilder.DropForeignKey(
                name: "FK_Propertys_Locations_LocationId1",
                table: "Propertys");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Cities");

            migrationBuilder.DropIndex(
                name: "IX_Propertys_LocationId",
                table: "Propertys");

            migrationBuilder.DropIndex(
                name: "IX_Propertys_LocationId1",
                table: "Propertys");

            migrationBuilder.DeleteData(
                table: "Propertys",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000001"));

            migrationBuilder.DeleteData(
                table: "Propertys",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000002"));

            migrationBuilder.DeleteData(
                table: "Propertys",
                keyColumn: "Id",
                keyValue: new Guid("30000000-0000-0000-0000-000000000003"));

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Propertys");

            migrationBuilder.DropColumn(
                name: "LocationId1",
                table: "Propertys");

            migrationBuilder.AddColumn<string>(
                name: "Code",
                table: "Propertys",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "Propertys",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "Description", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "HN", null, null, "Khu vực miền Bắc", "Hà Nội", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "DN", null, null, "Khu vực miền Trung", "Đà Nẵng", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000003"), "HP", null, null, "Hải Phòng", "Hải Phòng", null, null }
                });
        }
    }
}
