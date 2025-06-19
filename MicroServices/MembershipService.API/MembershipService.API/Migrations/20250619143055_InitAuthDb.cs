using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MembershipService.API.Migrations
{
    /// <inheritdoc />
    public partial class InitAuthDb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Ecosystems",
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
                    table.PrimaryKey("PK_Ecosystems", x => x.Id);
                });

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

            migrationBuilder.CreateTable(
                name: "PackageLevels",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageLevels", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NextUServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UnitType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EcosystemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NextUServices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NextUServices_Ecosystems_EcosystemId",
                        column: x => x.EcosystemId,
                        principalTable: "Ecosystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_NextUServices_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
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
                name: "MediaGallery",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    NexUServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BasicPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaGallery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaGallery_BasicPlans_BasicPlanId",
                        column: x => x.BasicPlanId,
                        principalTable: "BasicPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MediaGallery_NextUServices_NexUServiceId",
                        column: x => x.NexUServiceId,
                        principalTable: "NextUServices",
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

            migrationBuilder.CreateIndex(
                name: "IX_MediaGallery_BasicPlanId",
                table: "MediaGallery",
                column: "BasicPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaGallery_NexUServiceId",
                table: "MediaGallery",
                column: "NexUServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_NextUServices_EcosystemId",
                table: "NextUServices",
                column: "EcosystemId");

            migrationBuilder.CreateIndex(
                name: "IX_NextUServices_LocationId",
                table: "NextUServices",
                column: "LocationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasicPlanServices");

            migrationBuilder.DropTable(
                name: "ComboPlanServices");

            migrationBuilder.DropTable(
                name: "MediaGallery");

            migrationBuilder.DropTable(
                name: "ComboPlans");

            migrationBuilder.DropTable(
                name: "NextUServices");

            migrationBuilder.DropTable(
                name: "BasicPlans");

            migrationBuilder.DropTable(
                name: "PackageLevels");

            migrationBuilder.DropTable(
                name: "Ecosystems");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "PackageDurations");
        }
    }
}
