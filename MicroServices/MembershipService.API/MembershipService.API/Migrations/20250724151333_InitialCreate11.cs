using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MembershipService.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate11 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasicPlans_BasicPlanCategories_BasicPlanCategoryId",
                table: "BasicPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicPlans_BasicPlanLevels_PlanLevelId",
                table: "BasicPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_ComboPlans_PackageLevels_PackageLevelId",
                table: "ComboPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaGallery_AccommodationOptions_AccommodationOptionId",
                table: "MediaGallery");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaGallery_NextUServices_NextUServiceId",
                table: "MediaGallery");

            migrationBuilder.DropTable(
                name: "BasicPlanCategories");

            migrationBuilder.DropTable(
                name: "BasicPlanLevels");

            migrationBuilder.DropTable(
                name: "PackageLevels");

            migrationBuilder.DropIndex(
                name: "IX_MediaGallery_AccommodationOptionId",
                table: "MediaGallery");

            migrationBuilder.DropIndex(
                name: "IX_MediaGallery_NextUServiceId",
                table: "MediaGallery");

            migrationBuilder.DropIndex(
                name: "IX_ComboPlans_PackageLevelId",
                table: "ComboPlans");

            migrationBuilder.DropColumn(
                name: "NextUServiceId",
                table: "MediaGallery");

            migrationBuilder.DropColumn(
                name: "PackageLevelId",
                table: "ComboPlans");

            migrationBuilder.RenameColumn(
                name: "AccommodationOptionId",
                table: "MediaGallery",
                newName: "ActorId");

            migrationBuilder.AddColumn<int>(
                name: "ActorType",
                table: "MediaGallery",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "BasicPlanCategoryId",
                table: "ComboPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlanLevelId",
                table: "ComboPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TargetAudienceId",
                table: "ComboPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "PlanCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BasicPlanTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanCategories_BasicPlanTypes_BasicPlanTypeId",
                        column: x => x.BasicPlanTypeId,
                        principalTable: "BasicPlanTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PlanLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BasicPlanTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PlanLevels_BasicPlanTypes_BasicPlanTypeId",
                        column: x => x.BasicPlanTypeId,
                        principalTable: "BasicPlanTypes",
                        principalColumn: "Id");
                });

            migrationBuilder.UpdateData(
                table: "BasicPlanTypes",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0000-0000-0000-000000000001"),
                columns: new[] { "Code", "Name" },
                values: new object[] { "ACCOMMODATION", "Accommodation" });

            migrationBuilder.InsertData(
                table: "PlanCategories",
                columns: new[] { "Id", "BasicPlanTypeId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, null, "Áp dụng cho các gói từ 1 tháng trở lên", "Dài hạn" },
                    { 2, null, "Dành cho nhu cầu ngắn ngày hoặc linh hoạt", "Ngắn hạn" },
                    { 3, null, "Dành riêng cho các gói hoạt động theo sự kiện cụ thể", "Theo sự kiện" }
                });

            migrationBuilder.InsertData(
                table: "PlanLevels",
                columns: new[] { "Id", "BasicPlanTypeId", "Name" },
                values: new object[,]
                {
                    { 1, null, "Cơ bản" },
                    { 2, null, "Tiêu chuẩn" },
                    { 3, null, "Cao cấp" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_ComboPlans_BasicPlanCategoryId",
                table: "ComboPlans",
                column: "BasicPlanCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboPlans_PlanLevelId",
                table: "ComboPlans",
                column: "PlanLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboPlans_TargetAudienceId",
                table: "ComboPlans",
                column: "TargetAudienceId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanCategories_BasicPlanTypeId",
                table: "PlanCategories",
                column: "BasicPlanTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_PlanLevels_BasicPlanTypeId",
                table: "PlanLevels",
                column: "BasicPlanTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasicPlans_PlanCategories_BasicPlanCategoryId",
                table: "BasicPlans",
                column: "BasicPlanCategoryId",
                principalTable: "PlanCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BasicPlans_PlanLevels_PlanLevelId",
                table: "BasicPlans",
                column: "PlanLevelId",
                principalTable: "PlanLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ComboPlans_PlanCategories_BasicPlanCategoryId",
                table: "ComboPlans",
                column: "BasicPlanCategoryId",
                principalTable: "PlanCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ComboPlans_PlanLevels_PlanLevelId",
                table: "ComboPlans",
                column: "PlanLevelId",
                principalTable: "PlanLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ComboPlans_PlanTargetAudiences_TargetAudienceId",
                table: "ComboPlans",
                column: "TargetAudienceId",
                principalTable: "PlanTargetAudiences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasicPlans_PlanCategories_BasicPlanCategoryId",
                table: "BasicPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicPlans_PlanLevels_PlanLevelId",
                table: "BasicPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_ComboPlans_PlanCategories_BasicPlanCategoryId",
                table: "ComboPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_ComboPlans_PlanLevels_PlanLevelId",
                table: "ComboPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_ComboPlans_PlanTargetAudiences_TargetAudienceId",
                table: "ComboPlans");

            migrationBuilder.DropTable(
                name: "PlanCategories");

            migrationBuilder.DropTable(
                name: "PlanLevels");

            migrationBuilder.DropIndex(
                name: "IX_ComboPlans_BasicPlanCategoryId",
                table: "ComboPlans");

            migrationBuilder.DropIndex(
                name: "IX_ComboPlans_PlanLevelId",
                table: "ComboPlans");

            migrationBuilder.DropIndex(
                name: "IX_ComboPlans_TargetAudienceId",
                table: "ComboPlans");

            migrationBuilder.DropColumn(
                name: "ActorType",
                table: "MediaGallery");

            migrationBuilder.DropColumn(
                name: "BasicPlanCategoryId",
                table: "ComboPlans");

            migrationBuilder.DropColumn(
                name: "PlanLevelId",
                table: "ComboPlans");

            migrationBuilder.DropColumn(
                name: "TargetAudienceId",
                table: "ComboPlans");

            migrationBuilder.RenameColumn(
                name: "ActorId",
                table: "MediaGallery",
                newName: "AccommodationOptionId");

            migrationBuilder.AddColumn<Guid>(
                name: "NextUServiceId",
                table: "MediaGallery",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PackageLevelId",
                table: "ComboPlans",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "BasicPlanCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BasicPlanTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicPlanCategories", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasicPlanCategories_BasicPlanTypes_BasicPlanTypeId",
                        column: x => x.BasicPlanTypeId,
                        principalTable: "BasicPlanTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "BasicPlanLevels",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    BasicPlanTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicPlanLevels", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasicPlanLevels_BasicPlanTypes_BasicPlanTypeId",
                        column: x => x.BasicPlanTypeId,
                        principalTable: "BasicPlanTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PackageLevels",
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
                    table.PrimaryKey("PK_PackageLevels", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "BasicPlanCategories",
                columns: new[] { "Id", "BasicPlanTypeId", "Description", "Name" },
                values: new object[,]
                {
                    { 1, new Guid("60000000-0000-0000-0000-000000000001"), "Áp dụng cho các gói từ 1 tháng trở lên", "Dài hạn" },
                    { 2, new Guid("60000000-0000-0000-0000-000000000001"), "Dành cho nhu cầu ngắn ngày hoặc linh hoạt", "Ngắn hạn" },
                    { 3, new Guid("60000000-0000-0000-0000-000000000001"), "Dành riêng cho các gói hoạt động theo sự kiện cụ thể", "Theo sự kiện" }
                });

            migrationBuilder.InsertData(
                table: "BasicPlanLevels",
                columns: new[] { "Id", "BasicPlanTypeId", "Name" },
                values: new object[,]
                {
                    { 1, new Guid("60000000-0000-0000-0000-000000000001"), "Cơ bản" },
                    { 2, new Guid("60000000-0000-0000-0000-000000000001"), "Tiêu chuẩn" },
                    { 3, new Guid("60000000-0000-0000-0000-000000000001"), "Cao cấp" }
                });

            migrationBuilder.UpdateData(
                table: "BasicPlanTypes",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0000-0000-0000-000000000001"),
                columns: new[] { "Code", "Name" },
                values: new object[] { "LIVING", "Living" });

            migrationBuilder.CreateIndex(
                name: "IX_MediaGallery_AccommodationOptionId",
                table: "MediaGallery",
                column: "AccommodationOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_MediaGallery_NextUServiceId",
                table: "MediaGallery",
                column: "NextUServiceId");

            migrationBuilder.CreateIndex(
                name: "IX_ComboPlans_PackageLevelId",
                table: "ComboPlans",
                column: "PackageLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPlanCategories_BasicPlanTypeId",
                table: "BasicPlanCategories",
                column: "BasicPlanTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPlanLevels_BasicPlanTypeId",
                table: "BasicPlanLevels",
                column: "BasicPlanTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasicPlans_BasicPlanCategories_BasicPlanCategoryId",
                table: "BasicPlans",
                column: "BasicPlanCategoryId",
                principalTable: "BasicPlanCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BasicPlans_BasicPlanLevels_PlanLevelId",
                table: "BasicPlans",
                column: "PlanLevelId",
                principalTable: "BasicPlanLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ComboPlans_PackageLevels_PackageLevelId",
                table: "ComboPlans",
                column: "PackageLevelId",
                principalTable: "PackageLevels",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaGallery_AccommodationOptions_AccommodationOptionId",
                table: "MediaGallery",
                column: "AccommodationOptionId",
                principalTable: "AccommodationOptions",
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
    }
}
