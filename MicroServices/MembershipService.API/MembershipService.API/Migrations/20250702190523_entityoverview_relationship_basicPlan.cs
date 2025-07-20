using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MembershipService.API.Migrations
{
    /// <inheritdoc />
    public partial class entityoverview_relationship_basicPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasicPlans_PlanCategories_PlanCategoryId",
                table: "BasicPlans");

            migrationBuilder.DropTable(
                name: "PlanCategories");

            migrationBuilder.DropColumn(
                name: "RoomType",
                table: "AccommodationOptions");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Rooms",
                newName: "DescriptionDetails");

            migrationBuilder.RenameColumn(
                name: "PlanCategoryId",
                table: "BasicPlans",
                newName: "BasicPlanTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_BasicPlans_PlanCategoryId",
                table: "BasicPlans",
                newName: "IX_BasicPlans_BasicPlanTypeId");

            migrationBuilder.RenameColumn(
                name: "QuantityAvailable",
                table: "AccommodationOptions",
                newName: "RoomTypeId");

            migrationBuilder.AlterColumn<int>(
                name: "Floor",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RoomName",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "AccommodationOptionId",
                table: "MediaGallery",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "BasicPlanCategoryId",
                table: "BasicPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "PlanLevelId",
                table: "BasicPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TargetAudienceId",
                table: "BasicPlans",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "PropertyId",
                table: "AccommodationOptions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "BasicPlanTypes",
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
                    table.PrimaryKey("PK_BasicPlanTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PlanTargetAudiences",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanTargetAudiences", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomType",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomType", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "BasicPlanCategories",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    BasicPlanTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BasicPlanTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
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

            migrationBuilder.CreateIndex(
                name: "IX_MediaGallery_AccommodationOptionId",
                table: "MediaGallery",
                column: "AccommodationOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPlans_BasicPlanCategoryId",
                table: "BasicPlans",
                column: "BasicPlanCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPlans_PlanLevelId",
                table: "BasicPlans",
                column: "PlanLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPlans_TargetAudienceId",
                table: "BasicPlans",
                column: "TargetAudienceId");

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationOptions_PropertyId",
                table: "AccommodationOptions",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_AccommodationOptions_RoomTypeId",
                table: "AccommodationOptions",
                column: "RoomTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPlanCategories_BasicPlanTypeId",
                table: "BasicPlanCategories",
                column: "BasicPlanTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPlanLevels_BasicPlanTypeId",
                table: "BasicPlanLevels",
                column: "BasicPlanTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_AccommodationOptions_Propertys_PropertyId",
                table: "AccommodationOptions",
                column: "PropertyId",
                principalTable: "Propertys",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AccommodationOptions_RoomType_RoomTypeId",
                table: "AccommodationOptions",
                column: "RoomTypeId",
                principalTable: "RoomType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

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
                name: "FK_BasicPlans_BasicPlanTypes_BasicPlanTypeId",
                table: "BasicPlans",
                column: "BasicPlanTypeId",
                principalTable: "BasicPlanTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_BasicPlans_PlanTargetAudiences_TargetAudienceId",
                table: "BasicPlans",
                column: "TargetAudienceId",
                principalTable: "PlanTargetAudiences",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MediaGallery_AccommodationOptions_AccommodationOptionId",
                table: "MediaGallery",
                column: "AccommodationOptionId",
                principalTable: "AccommodationOptions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AccommodationOptions_Propertys_PropertyId",
                table: "AccommodationOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_AccommodationOptions_RoomType_RoomTypeId",
                table: "AccommodationOptions");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicPlans_BasicPlanCategories_BasicPlanCategoryId",
                table: "BasicPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicPlans_BasicPlanLevels_PlanLevelId",
                table: "BasicPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicPlans_BasicPlanTypes_BasicPlanTypeId",
                table: "BasicPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicPlans_PlanTargetAudiences_TargetAudienceId",
                table: "BasicPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_MediaGallery_AccommodationOptions_AccommodationOptionId",
                table: "MediaGallery");

            migrationBuilder.DropTable(
                name: "BasicPlanCategories");

            migrationBuilder.DropTable(
                name: "BasicPlanLevels");

            migrationBuilder.DropTable(
                name: "PlanTargetAudiences");

            migrationBuilder.DropTable(
                name: "RoomType");

            migrationBuilder.DropTable(
                name: "BasicPlanTypes");

            migrationBuilder.DropIndex(
                name: "IX_MediaGallery_AccommodationOptionId",
                table: "MediaGallery");

            migrationBuilder.DropIndex(
                name: "IX_BasicPlans_BasicPlanCategoryId",
                table: "BasicPlans");

            migrationBuilder.DropIndex(
                name: "IX_BasicPlans_PlanLevelId",
                table: "BasicPlans");

            migrationBuilder.DropIndex(
                name: "IX_BasicPlans_TargetAudienceId",
                table: "BasicPlans");

            migrationBuilder.DropIndex(
                name: "IX_AccommodationOptions_PropertyId",
                table: "AccommodationOptions");

            migrationBuilder.DropIndex(
                name: "IX_AccommodationOptions_RoomTypeId",
                table: "AccommodationOptions");

            migrationBuilder.DropColumn(
                name: "RoomName",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "AccommodationOptionId",
                table: "MediaGallery");

            migrationBuilder.DropColumn(
                name: "BasicPlanCategoryId",
                table: "BasicPlans");

            migrationBuilder.DropColumn(
                name: "PlanLevelId",
                table: "BasicPlans");

            migrationBuilder.DropColumn(
                name: "TargetAudienceId",
                table: "BasicPlans");

            migrationBuilder.DropColumn(
                name: "PropertyId",
                table: "AccommodationOptions");

            migrationBuilder.RenameColumn(
                name: "DescriptionDetails",
                table: "Rooms",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "BasicPlanTypeId",
                table: "BasicPlans",
                newName: "PlanCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_BasicPlans_BasicPlanTypeId",
                table: "BasicPlans",
                newName: "IX_BasicPlans_PlanCategoryId");

            migrationBuilder.RenameColumn(
                name: "RoomTypeId",
                table: "AccommodationOptions",
                newName: "QuantityAvailable");

            migrationBuilder.AlterColumn<string>(
                name: "Floor",
                table: "Rooms",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<string>(
                name: "RoomType",
                table: "AccommodationOptions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PlanCategories",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    UpdatedBy = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PlanCategories", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_BasicPlans_PlanCategories_PlanCategoryId",
                table: "BasicPlans",
                column: "PlanCategoryId",
                principalTable: "PlanCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
