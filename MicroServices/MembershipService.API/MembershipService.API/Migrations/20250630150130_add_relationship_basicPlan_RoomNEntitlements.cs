using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MembershipService.API.Migrations
{
    /// <inheritdoc />
    public partial class add_relationship_basicPlan_RoomNEntitlements : Migration
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
                name: "Propertys",
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
                    table.PrimaryKey("PK_Propertys", x => x.Id);
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
                name: "PlanCategories",
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
                    table.PrimaryKey("PK_PlanCategories", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "NextUServices",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ServiceType = table.Column<int>(type: "int", nullable: false),
                    EcosystemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                        name: "FK_NextUServices_Propertys_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Propertys",
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
                    DiscountRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsSuggested = table.Column<bool>(type: "bit", nullable: false),
                    VerifyBuy = table.Column<bool>(type: "bit", nullable: false),
                    PropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PackageLevelId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComboPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComboPlans_Propertys_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Propertys",
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
                name: "BasicPlans",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    VerifyBuy = table.Column<bool>(type: "bit", nullable: false),
                    PlanCategoryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicPlans", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasicPlans_Propertys_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Propertys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BasicPlans_PlanCategories_PlanCategoryId",
                        column: x => x.PlanCategoryId,
                        principalTable: "PlanCategories",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AccommodationOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NextUServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuantityAvailable = table.Column<int>(type: "int", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    PricePerNight = table.Column<decimal>(type: "decimal(18,4)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AccommodationOptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AccommodationOptions_NextUServices_NextUServiceId",
                        column: x => x.NextUServiceId,
                        principalTable: "NextUServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "EntitlementRules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NextUServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreditAmount = table.Column<int>(type: "int", nullable: false),
                    Period = table.Column<int>(type: "int", nullable: false),
                    LimitPerWeek = table.Column<int>(type: "int", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EntitlementRules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EntitlementRules_NextUServices_NextUServiceId",
                        column: x => x.NextUServiceId,
                        principalTable: "NextUServices",
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
                    NextUServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MediaGallery", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MediaGallery_NextUServices_NextUServiceId",
                        column: x => x.NextUServiceId,
                        principalTable: "NextUServices",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComboPlanBasics",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComboPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BasicPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComboPlanBasics", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComboPlanBasics_BasicPlans_BasicPlanId",
                        column: x => x.BasicPlanId,
                        principalTable: "BasicPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComboPlanBasics_ComboPlans_ComboPlanId",
                        column: x => x.ComboPlanId,
                        principalTable: "ComboPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ComboPlanDurations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ComboPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BasicPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PackageDurationId = table.Column<int>(type: "int", nullable: false),
                    DiscountDurationRate = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ComboPlanDurations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ComboPlanDurations_BasicPlans_BasicPlanId",
                        column: x => x.BasicPlanId,
                        principalTable: "BasicPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComboPlanDurations_ComboPlans_ComboPlanId",
                        column: x => x.ComboPlanId,
                        principalTable: "ComboPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ComboPlanDurations_PackageDurations_PackageDurationId",
                        column: x => x.PackageDurationId,
                        principalTable: "PackageDurations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MembershipHistory",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BasicPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ComboPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PackageNameSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriceSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PurchasedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MembershipHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MembershipHistory_BasicPlans_BasicPlanId",
                        column: x => x.BasicPlanId,
                        principalTable: "BasicPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MembershipHistory_ComboPlans_ComboPlanId",
                        column: x => x.ComboPlanId,
                        principalTable: "ComboPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Memberships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BasicPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ComboPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PackageNameSnapshot = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PriceSnapshot = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memberships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Memberships_BasicPlans_BasicPlanId",
                        column: x => x.BasicPlanId,
                        principalTable: "BasicPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Memberships_ComboPlans_ComboPlanId",
                        column: x => x.ComboPlanId,
                        principalTable: "ComboPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Rooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccommodationOptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomCode = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Floor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Rooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Rooms_AccommodationOptions_AccommodationOptionId",
                        column: x => x.AccommodationOptionId,
                        principalTable: "AccommodationOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BasicPlanEntitlements",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BasicPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EntitlementRuleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicPlanEntitlements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasicPlanEntitlements_BasicPlans_BasicPlanId",
                        column: x => x.BasicPlanId,
                        principalTable: "BasicPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BasicPlanEntitlements_EntitlementRules_EntitlementRuleId",
                        column: x => x.EntitlementRuleId,
                        principalTable: "EntitlementRules",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BasicPlanRooms",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BasicPlanId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomInstanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NightsIncluded = table.Column<int>(type: "int", nullable: false),
                    CustomPricePerNight = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    TotalPrice = table.Column<decimal>(type: "decimal(18,4)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicPlanRooms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasicPlanRooms_BasicPlans_BasicPlanId",
                        column: x => x.BasicPlanId,
                        principalTable: "BasicPlans",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_BasicPlanRooms_Rooms_RoomInstanceId",
                        column: x => x.RoomInstanceId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Bookings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MemberId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomInstanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    EndDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<int>(type: "int", nullable: false),
                    Note = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bookings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Bookings_Rooms_RoomInstanceId",
                        column: x => x.RoomInstanceId,
                        principalTable: "Rooms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Propertys",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "Description", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("10000000-0000-0000-0000-000000000001"), "HN", null, null, "Khu vực miền Bắc", "Hà Nội", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000002"), "DN", null, null, "Khu vực miền Trung", "Đà Nẵng", null, null },
                    { new Guid("10000000-0000-0000-0000-000000000003"), "HP", null, null, "Hải Phòng", "Hải Phòng", null, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationOptions_NextUServiceId",
                table: "AccommodationOptions",
                column: "NextUServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPlanEntitlements_BasicPlanId",
                table: "BasicPlanEntitlements",
                column: "BasicPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPlanEntitlements_EntitlementRuleId",
                table: "BasicPlanEntitlements",
                column: "EntitlementRuleId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPlanRooms_BasicPlanId",
                table: "BasicPlanRooms",
                column: "BasicPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPlanRooms_RoomInstanceId",
                table: "BasicPlanRooms",
                column: "RoomInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPlans_PropertyId",
                table: "BasicPlans",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPlans_PlanCategoryId",
                table: "BasicPlans",
                column: "PlanCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_RoomInstanceId",
                table: "Bookings",
                column: "RoomInstanceId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboPlanBasics_BasicPlanId",
                table: "ComboPlanBasics",
                column: "BasicPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboPlanBasics_ComboPlanId",
                table: "ComboPlanBasics",
                column: "ComboPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboPlanDurations_BasicPlanId",
                table: "ComboPlanDurations",
                column: "BasicPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboPlanDurations_ComboPlanId",
                table: "ComboPlanDurations",
                column: "ComboPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboPlanDurations_PackageDurationId",
                table: "ComboPlanDurations",
                column: "PackageDurationId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboPlans_PropertyId",
                table: "ComboPlans",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboPlans_PackageLevelId",
                table: "ComboPlans",
                column: "PackageLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_EntitlementRules_NextUServiceId",
                table: "EntitlementRules",
                column: "NextUServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaGallery_NextUServiceId",
                table: "MediaGallery",
                column: "NextUServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipHistory_BasicPlanId",
                table: "MembershipHistory",
                column: "BasicPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_MembershipHistory_ComboPlanId",
                table: "MembershipHistory",
                column: "ComboPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_BasicPlanId",
                table: "Memberships",
                column: "BasicPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_ComboPlanId",
                table: "Memberships",
                column: "ComboPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_NextUServices_EcosystemId",
                table: "NextUServices",
                column: "EcosystemId");

            migrationBuilder.CreateIndex(
                name: "IX_NextUServices_PropertyId",
                table: "NextUServices",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_Rooms_AccommodationOptionId",
                table: "Rooms",
                column: "AccommodationOptionId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "BasicPlanEntitlements");

            migrationBuilder.DropTable(
                name: "BasicPlanRooms");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "ComboPlanBasics");

            migrationBuilder.DropTable(
                name: "ComboPlanDurations");

            migrationBuilder.DropTable(
                name: "MediaGallery");

            migrationBuilder.DropTable(
                name: "MembershipHistory");

            migrationBuilder.DropTable(
                name: "Memberships");

            migrationBuilder.DropTable(
                name: "EntitlementRules");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "PackageDurations");

            migrationBuilder.DropTable(
                name: "BasicPlans");

            migrationBuilder.DropTable(
                name: "ComboPlans");

            migrationBuilder.DropTable(
                name: "AccommodationOptions");

            migrationBuilder.DropTable(
                name: "PlanCategories");

            migrationBuilder.DropTable(
                name: "PackageLevels");

            migrationBuilder.DropTable(
                name: "NextUServices");

            migrationBuilder.DropTable(
                name: "Ecosystems");

            migrationBuilder.DropTable(
                name: "Propertys");
        }
    }
}
