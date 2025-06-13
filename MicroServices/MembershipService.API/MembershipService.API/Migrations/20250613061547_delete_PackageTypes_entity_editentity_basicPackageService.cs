using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MembershipService.API.Migrations
{
    /// <inheritdoc />
    public partial class delete_PackageTypes_entity_editentity_basicPackageService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasicPackages_PackageTypes_PackageTypeId",
                table: "BasicPackages");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicPackageService_BasicPackages_BasicPackageId",
                table: "BasicPackageService");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicPackageService_NextUServices_NextUServiceId",
                table: "BasicPackageService");

            migrationBuilder.DropTable(
                name: "PackageTypes");

            migrationBuilder.DropIndex(
                name: "IX_BasicPackages_PackageTypeId",
                table: "BasicPackages");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BasicPackageService",
                table: "BasicPackageService");

            migrationBuilder.DropColumn(
                name: "Duration",
                table: "BasicPackages");

            migrationBuilder.DropColumn(
                name: "PackageTypeId",
                table: "BasicPackages");

            migrationBuilder.RenameTable(
                name: "BasicPackageService",
                newName: "BasicPackageServices");

            migrationBuilder.RenameIndex(
                name: "IX_BasicPackageService_NextUServiceId",
                table: "BasicPackageServices",
                newName: "IX_BasicPackageServices_NextUServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_BasicPackageService_BasicPackageId",
                table: "BasicPackageServices",
                newName: "IX_BasicPackageServices_BasicPackageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_BasicPackageServices",
                table: "BasicPackageServices",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BasicPackageServices_BasicPackages_BasicPackageId",
                table: "BasicPackageServices",
                column: "BasicPackageId",
                principalTable: "BasicPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BasicPackageServices_NextUServices_NextUServiceId",
                table: "BasicPackageServices",
                column: "NextUServiceId",
                principalTable: "NextUServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasicPackageServices_BasicPackages_BasicPackageId",
                table: "BasicPackageServices");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicPackageServices_NextUServices_NextUServiceId",
                table: "BasicPackageServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_BasicPackageServices",
                table: "BasicPackageServices");

            migrationBuilder.RenameTable(
                name: "BasicPackageServices",
                newName: "BasicPackageService");

            migrationBuilder.RenameIndex(
                name: "IX_BasicPackageServices_NextUServiceId",
                table: "BasicPackageService",
                newName: "IX_BasicPackageService_NextUServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_BasicPackageServices_BasicPackageId",
                table: "BasicPackageService",
                newName: "IX_BasicPackageService_BasicPackageId");

            migrationBuilder.AddColumn<int>(
                name: "Duration",
                table: "BasicPackages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "PackageTypeId",
                table: "BasicPackages",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddPrimaryKey(
                name: "PK_BasicPackageService",
                table: "BasicPackageService",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "PackageTypes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageTypes", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasicPackages_PackageTypeId",
                table: "BasicPackages",
                column: "PackageTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasicPackages_PackageTypes_PackageTypeId",
                table: "BasicPackages",
                column: "PackageTypeId",
                principalTable: "PackageTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BasicPackageService_BasicPackages_BasicPackageId",
                table: "BasicPackageService",
                column: "BasicPackageId",
                principalTable: "BasicPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BasicPackageService_NextUServices_NextUServiceId",
                table: "BasicPackageService",
                column: "NextUServiceId",
                principalTable: "NextUServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
