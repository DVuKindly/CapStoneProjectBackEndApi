using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MembershipService.API.Migrations
{
    /// <inheritdoc />
    public partial class add_entity_basicPackageService_add_entity_packageDuration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ComboPackages_PackageLevels_PackageLevelId",
                table: "ComboPackages");

            migrationBuilder.DropIndex(
                name: "IX_ComboPackages_PackageLevelId",
                table: "ComboPackages");

            migrationBuilder.DropColumn(
                name: "PackageLevelId",
                table: "ComboPackages");

            migrationBuilder.AddColumn<int>(
                name: "PackageDurationId",
                table: "BasicPackages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "PackageLevelId",
                table: "BasicPackages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BasicPackageService",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BasicPackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NextUServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicPackageService", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasicPackageService_BasicPackages_BasicPackageId",
                        column: x => x.BasicPackageId,
                        principalTable: "BasicPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BasicPackageService_NextUServices_NextUServiceId",
                        column: x => x.NextUServiceId,
                        principalTable: "NextUServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PackageDurations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<int>(type: "int", nullable: false),
                    Unit = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageDurations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasicPackages_PackageDurationId",
                table: "BasicPackages",
                column: "PackageDurationId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPackages_PackageLevelId",
                table: "BasicPackages",
                column: "PackageLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPackageService_BasicPackageId",
                table: "BasicPackageService",
                column: "BasicPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPackageService_NextUServiceId",
                table: "BasicPackageService",
                column: "NextUServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasicPackages_PackageDurations_PackageDurationId",
                table: "BasicPackages",
                column: "PackageDurationId",
                principalTable: "PackageDurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BasicPackages_PackageLevels_PackageLevelId",
                table: "BasicPackages",
                column: "PackageLevelId",
                principalTable: "PackageLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasicPackages_PackageDurations_PackageDurationId",
                table: "BasicPackages");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicPackages_PackageLevels_PackageLevelId",
                table: "BasicPackages");

            migrationBuilder.DropTable(
                name: "BasicPackageService");

            migrationBuilder.DropTable(
                name: "PackageDurations");

            migrationBuilder.DropIndex(
                name: "IX_BasicPackages_PackageDurationId",
                table: "BasicPackages");

            migrationBuilder.DropIndex(
                name: "IX_BasicPackages_PackageLevelId",
                table: "BasicPackages");

            migrationBuilder.DropColumn(
                name: "PackageDurationId",
                table: "BasicPackages");

            migrationBuilder.DropColumn(
                name: "PackageLevelId",
                table: "BasicPackages");

            migrationBuilder.AddColumn<Guid>(
                name: "PackageLevelId",
                table: "ComboPackages",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_ComboPackages_PackageLevelId",
                table: "ComboPackages",
                column: "PackageLevelId");

            migrationBuilder.AddForeignKey(
                name: "FK_ComboPackages_PackageLevels_PackageLevelId",
                table: "ComboPackages",
                column: "PackageLevelId",
                principalTable: "PackageLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
