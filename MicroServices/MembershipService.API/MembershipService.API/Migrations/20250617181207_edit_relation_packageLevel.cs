using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MembershipService.API.Migrations
{
    /// <inheritdoc />
    public partial class edit_relation_packageLevel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Media_BasicPackages_BasicPackageId",
                table: "Media");

            migrationBuilder.DropForeignKey(
                name: "FK_Media_NextUServices_NexUServiceId",
                table: "Media");

            migrationBuilder.DropTable(
                name: "BasicPackageService");

            migrationBuilder.DropTable(
                name: "ComboPackageServices");

            migrationBuilder.DropTable(
                name: "ServicePricings");

            migrationBuilder.DropTable(
                name: "ComboPackages");

            migrationBuilder.DropTable(
                name: "BasicPackages");

            migrationBuilder.DropTable(
                name: "PackageTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Media",
                table: "Media");

            migrationBuilder.RenameTable(
                name: "Media",
                newName: "MediaGallery");

            migrationBuilder.RenameColumn(
                name: "BasicPackageId",
                table: "MediaGallery",
                newName: "BasicPlanId");

            migrationBuilder.RenameIndex(
                name: "IX_Media_NexUServiceId",
                table: "MediaGallery",
                newName: "IX_MediaGallery_NexUServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_Media_BasicPackageId",
                table: "MediaGallery",
                newName: "IX_MediaGallery_BasicPlanId");

            migrationBuilder.AddColumn<Guid>(
                name: "LocationId",
                table: "NextUServices",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "NextUServices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddPrimaryKey(
                name: "PK_MediaGallery",
                table: "MediaGallery",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BasicPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VerifyBuy = table.Column<bool>(type: "bit", nullable: false),
                    PackageDurationId = table.Column<int>(type: "int", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasicPlans_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BasicPlans_PackageDurations_PackageDurationId",
                        column: x => x.PackageDurationId,
                        principalTable: "PackageDurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BasicPlanServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BasicPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NextUServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicPlanServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasicPlanServices_BasicPlans_BasicPlanId",
                        column: x => x.BasicPlanId,
                        principalTable: "BasicPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BasicPlanServices_NextUServices_NextUServiceId",
                        column: x => x.NextUServiceId,
                        principalTable: "NextUServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComboPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    DiscountRate = table.Column<float>(type: "real", nullable: false),
                    IsSuggested = table.Column<bool>(type: "bit", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PackageLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BasicPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComboPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComboPlans_BasicPlans_BasicPlanId",
                        column: x => x.BasicPlanId,
                        principalTable: "BasicPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComboPlans_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComboPlans_PackageLevels_PackageLevelId",
                        column: x => x.PackageLevelId,
                        principalTable: "PackageLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComboPlanServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComboPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NextUServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComboPlanServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComboPlanServices_ComboPlans_ComboPlanId",
                        column: x => x.ComboPlanId,
                        principalTable: "ComboPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComboPlanServices_NextUServices_NextUServiceId",
                        column: x => x.NextUServiceId,
                        principalTable: "NextUServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NextUServices_LocationId",
                table: "NextUServices",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPlans_LocationId",
                table: "BasicPlans",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPlans_PackageDurationId",
                table: "BasicPlans",
                column: "PackageDurationId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPlanServices_BasicPlanId",
                table: "BasicPlanServices",
                column: "BasicPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPlanServices_NextUServiceId",
                table: "BasicPlanServices",
                column: "NextUServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboPlans_BasicPlanId",
                table: "ComboPlans",
                column: "BasicPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboPlans_LocationId",
                table: "ComboPlans",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboPlans_PackageLevelId",
                table: "ComboPlans",
                column: "PackageLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboPlanServices_ComboPlanId",
                table: "ComboPlanServices",
                column: "ComboPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboPlanServices_NextUServiceId",
                table: "ComboPlanServices",
                column: "NextUServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_MediaGallery_BasicPlans_BasicPlanId",
                table: "MediaGallery",
                column: "BasicPlanId",
                principalTable: "BasicPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaGallery_NextUServices_NexUServiceId",
                table: "MediaGallery",
                column: "NexUServiceId",
                principalTable: "NextUServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_NextUServices_Locations_LocationId",
                table: "NextUServices",
                column: "LocationId",
                principalTable: "Locations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MediaGallery_BasicPlans_BasicPlanId",
                table: "MediaGallery");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaGallery_NextUServices_NexUServiceId",
                table: "MediaGallery");

            migrationBuilder.DropForeignKey(
                name: "FK_NextUServices_Locations_LocationId",
                table: "NextUServices");

            migrationBuilder.DropTable(
                name: "BasicPlanServices");

            migrationBuilder.DropTable(
                name: "ComboPlanServices");

            migrationBuilder.DropTable(
                name: "ComboPlans");

            migrationBuilder.DropTable(
                name: "BasicPlans");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropIndex(
                name: "IX_NextUServices_LocationId",
                table: "NextUServices");

            migrationBuilder.DropPrimaryKey(
                name: "PK_MediaGallery",
                table: "MediaGallery");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "NextUServices");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "NextUServices");

            migrationBuilder.RenameTable(
                name: "MediaGallery",
                newName: "Media");

            migrationBuilder.RenameColumn(
                name: "BasicPlanId",
                table: "Media",
                newName: "BasicPackageId");

            migrationBuilder.RenameIndex(
                name: "IX_MediaGallery_NexUServiceId",
                table: "Media",
                newName: "IX_Media_NexUServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_MediaGallery_BasicPlanId",
                table: "Media",
                newName: "IX_Media_BasicPackageId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Media",
                table: "Media",
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

            migrationBuilder.CreateTable(
                name: "BasicPackages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackageDurationId = table.Column<int>(type: "int", nullable: false),
                    PackageLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PackageTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Duration = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    VerifyBuy = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicPackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasicPackages_PackageDurations_PackageDurationId",
                        column: x => x.PackageDurationId,
                        principalTable: "PackageDurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BasicPackages_PackageLevels_PackageLevelId",
                        column: x => x.PackageLevelId,
                        principalTable: "PackageLevels",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BasicPackages_PackageTypes_PackageTypeId",
                        column: x => x.PackageTypeId,
                        principalTable: "PackageTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
                name: "ComboPackages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BasicPackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DiscountRate = table.Column<float>(type: "real", nullable: false),
                    IsSuggested = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComboPackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComboPackages_BasicPackages_BasicPackageId",
                        column: x => x.BasicPackageId,
                        principalTable: "BasicPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComboPackageServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComboPackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NextUServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComboPackageServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComboPackageServices_ComboPackages_ComboPackageId",
                        column: x => x.ComboPackageId,
                        principalTable: "ComboPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComboPackageServices_NextUServices_NextUServiceId",
                        column: x => x.NextUServiceId,
                        principalTable: "NextUServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ServicePricings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BasicPackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ComboPackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    NextUServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreditCost = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsDefault = table.Column<bool>(type: "bit", nullable: false),
                    IsOptional = table.Column<bool>(type: "bit", nullable: false),
                    OverridePrice = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ServicePricings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ServicePricings_BasicPackages_BasicPackageId",
                        column: x => x.BasicPackageId,
                        principalTable: "BasicPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServicePricings_ComboPackages_ComboPackageId",
                        column: x => x.ComboPackageId,
                        principalTable: "ComboPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ServicePricings_NextUServices_NextUServiceId",
                        column: x => x.NextUServiceId,
                        principalTable: "NextUServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "IX_BasicPackages_PackageTypeId",
                table: "BasicPackages",
                column: "PackageTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPackageService_BasicPackageId",
                table: "BasicPackageService",
                column: "BasicPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPackageService_NextUServiceId",
                table: "BasicPackageService",
                column: "NextUServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboPackages_BasicPackageId",
                table: "ComboPackages",
                column: "BasicPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboPackageServices_ComboPackageId",
                table: "ComboPackageServices",
                column: "ComboPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboPackageServices_NextUServiceId",
                table: "ComboPackageServices",
                column: "NextUServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePricings_BasicPackageId",
                table: "ServicePricings",
                column: "BasicPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePricings_ComboPackageId",
                table: "ServicePricings",
                column: "ComboPackageId");

            migrationBuilder.CreateIndex(
                name: "IX_ServicePricings_NextUServiceId",
                table: "ServicePricings",
                column: "NextUServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Media_BasicPackages_BasicPackageId",
                table: "Media",
                column: "BasicPackageId",
                principalTable: "BasicPackages",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Media_NextUServices_NexUServiceId",
                table: "Media",
                column: "NexUServiceId",
                principalTable: "NextUServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
