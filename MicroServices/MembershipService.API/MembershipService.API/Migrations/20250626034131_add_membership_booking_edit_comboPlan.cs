using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MembershipService.API.Migrations
{
    /// <inheritdoc />
    public partial class add_membership_booking_edit_comboPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasicPlans_PackageDurations_PackageDurationId",
                table: "BasicPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_ComboPlans_BasicPlans_BasicPlanId",
                table: "ComboPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaGallery_BasicPlans_BasicPlanId",
                table: "MediaGallery");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaGallery_NextUServices_NexUServiceId",
                table: "MediaGallery");

            migrationBuilder.DropTable(
                name: "ComboPlanServices");

            migrationBuilder.DropIndex(
                name: "IX_MediaGallery_BasicPlanId",
                table: "MediaGallery");

            migrationBuilder.DropIndex(
                name: "IX_ComboPlans_BasicPlanId",
                table: "ComboPlans");

            migrationBuilder.DropIndex(
                name: "IX_BasicPlans_PackageDurationId",
                table: "BasicPlans");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "NextUServices");

            migrationBuilder.DropColumn(
                name: "UnitType",
                table: "NextUServices");

            migrationBuilder.DropColumn(
                name: "BasicPlanId",
                table: "MediaGallery");

            migrationBuilder.DropColumn(
                name: "BasicPlanId",
                table: "ComboPlans");

            migrationBuilder.DropColumn(
                name: "PackageDurationId",
                table: "BasicPlans");

            migrationBuilder.RenameColumn(
                name: "NexUServiceId",
                table: "MediaGallery",
                newName: "NextUServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_MediaGallery_NexUServiceId",
                table: "MediaGallery",
                newName: "IX_MediaGallery_NextUServiceId");

            migrationBuilder.AddColumn<int>(
                name: "ServiceType",
                table: "NextUServices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AlterColumn<decimal>(
                name: "DiscountRate",
                table: "ComboPlans",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.AddColumn<bool>(
                name: "VerifyBuy",
                table: "ComboPlans",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "PlanCategoryId",
                table: "BasicPlans",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "AccommodationOptions",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    NextUServiceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoomType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    QuantityAvailable = table.Column<int>(type: "int", nullable: false),
                    Capacity = table.Column<int>(type: "int", nullable: false),
                    PricePerNight = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_BasicPlans_PlanCategoryId",
                table: "BasicPlans",
                column: "PlanCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationOptions_NextUServiceId",
                table: "AccommodationOptions",
                column: "NextUServiceId");

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
                name: "IX_EntitlementRules_NextUServiceId",
                table: "EntitlementRules",
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
                name: "IX_Rooms_AccommodationOptionId",
                table: "Rooms",
                column: "AccommodationOptionId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasicPlans_PlanCategories_PlanCategoryId",
                table: "BasicPlans",
                column: "PlanCategoryId",
                principalTable: "PlanCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaGallery_NextUServices_NextUServiceId",
                table: "MediaGallery",
                column: "NextUServiceId",
                principalTable: "NextUServices",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasicPlans_PlanCategories_PlanCategoryId",
                table: "BasicPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaGallery_NextUServices_NextUServiceId",
                table: "MediaGallery");

            migrationBuilder.DropTable(
                name: "Bookings");

            migrationBuilder.DropTable(
                name: "ComboPlanBasics");

            migrationBuilder.DropTable(
                name: "ComboPlanDurations");

            migrationBuilder.DropTable(
                name: "EntitlementRules");

            migrationBuilder.DropTable(
                name: "MembershipHistory");

            migrationBuilder.DropTable(
                name: "Memberships");

            migrationBuilder.DropTable(
                name: "PlanCategories");

            migrationBuilder.DropTable(
                name: "Rooms");

            migrationBuilder.DropTable(
                name: "AccommodationOptions");

            migrationBuilder.DropIndex(
                name: "IX_BasicPlans_PlanCategoryId",
                table: "BasicPlans");

            migrationBuilder.DropColumn(
                name: "ServiceType",
                table: "NextUServices");

            migrationBuilder.DropColumn(
                name: "VerifyBuy",
                table: "ComboPlans");

            migrationBuilder.DropColumn(
                name: "PlanCategoryId",
                table: "BasicPlans");

            migrationBuilder.RenameColumn(
                name: "NextUServiceId",
                table: "MediaGallery",
                newName: "NexUServiceId");

            migrationBuilder.RenameIndex(
                name: "IX_MediaGallery_NextUServiceId",
                table: "MediaGallery",
                newName: "IX_MediaGallery_NexUServiceId");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "NextUServices",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "UnitType",
                table: "NextUServices",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "BasicPlanId",
                table: "MediaGallery",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AlterColumn<float>(
                name: "DiscountRate",
                table: "ComboPlans",
                type: "real",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AddColumn<Guid>(
                name: "BasicPlanId",
                table: "ComboPlans",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "PackageDurationId",
                table: "BasicPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

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
                name: "IX_MediaGallery_BasicPlanId",
                table: "MediaGallery",
                column: "BasicPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboPlans_BasicPlanId",
                table: "ComboPlans",
                column: "BasicPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPlans_PackageDurationId",
                table: "BasicPlans",
                column: "PackageDurationId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboPlanServices_ComboPlanId",
                table: "ComboPlanServices",
                column: "ComboPlanId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboPlanServices_NextUServiceId",
                table: "ComboPlanServices",
                column: "NextUServiceId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasicPlans_PackageDurations_PackageDurationId",
                table: "BasicPlans",
                column: "PackageDurationId",
                principalTable: "PackageDurations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ComboPlans_BasicPlans_BasicPlanId",
                table: "ComboPlans",
                column: "BasicPlanId",
                principalTable: "BasicPlans",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
        }
    }
}
