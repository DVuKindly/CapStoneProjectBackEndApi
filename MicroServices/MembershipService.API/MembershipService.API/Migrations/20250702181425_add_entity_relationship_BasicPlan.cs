using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MembershipService.API.Migrations
{
    /// <inheritdoc />
    public partial class add_entity_relationship_BasicPlan : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasicPlans_PlanCategories_PlanCategoryId",
                table: "BasicPlans");

            migrationBuilder.DropTable(
                name: "PlanCategories");

            migrationBuilder.RenameColumn(
                name: "PlanCategoryId",
                table: "BasicPlans",
                newName: "BasicPlanTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_BasicPlans_PlanCategoryId",
                table: "BasicPlans",
                newName: "IX_BasicPlans_BasicPlanTypeId");

            migrationBuilder.AddColumn<int>(
                name: "BasicPlanCategoryId",
                table: "BasicPlans",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "BasicPlanLevelId",
                table: "BasicPlans",
                type: "int",
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
                name: "BasicPlanCategory",
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
                    table.PrimaryKey("PK_BasicPlanCategory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasicPlanCategory_BasicPlanTypes_BasicPlanTypeId",
                        column: x => x.BasicPlanTypeId,
                        principalTable: "BasicPlanTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "BasicPlanLevel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    BasicPlanTypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_BasicPlanLevel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_BasicPlanLevel_BasicPlanTypes_BasicPlanTypeId",
                        column: x => x.BasicPlanTypeId,
                        principalTable: "BasicPlanTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_BasicPlans_BasicPlanCategoryId",
                table: "BasicPlans",
                column: "BasicPlanCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPlans_BasicPlanLevelId",
                table: "BasicPlans",
                column: "BasicPlanLevelId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPlanCategory_BasicPlanTypeId",
                table: "BasicPlanCategory",
                column: "BasicPlanTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_BasicPlanLevel_BasicPlanTypeId",
                table: "BasicPlanLevel",
                column: "BasicPlanTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_BasicPlans_BasicPlanCategory_BasicPlanCategoryId",
                table: "BasicPlans",
                column: "BasicPlanCategoryId",
                principalTable: "BasicPlanCategory",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BasicPlans_BasicPlanLevel_BasicPlanLevelId",
                table: "BasicPlans",
                column: "BasicPlanLevelId",
                principalTable: "BasicPlanLevel",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_BasicPlans_BasicPlanTypes_BasicPlanTypeId",
                table: "BasicPlans",
                column: "BasicPlanTypeId",
                principalTable: "BasicPlanTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BasicPlans_BasicPlanCategory_BasicPlanCategoryId",
                table: "BasicPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicPlans_BasicPlanLevel_BasicPlanLevelId",
                table: "BasicPlans");

            migrationBuilder.DropForeignKey(
                name: "FK_BasicPlans_BasicPlanTypes_BasicPlanTypeId",
                table: "BasicPlans");

            migrationBuilder.DropTable(
                name: "BasicPlanCategory");

            migrationBuilder.DropTable(
                name: "BasicPlanLevel");

            migrationBuilder.DropTable(
                name: "BasicPlanTypes");

            migrationBuilder.DropIndex(
                name: "IX_BasicPlans_BasicPlanCategoryId",
                table: "BasicPlans");

            migrationBuilder.DropIndex(
                name: "IX_BasicPlans_BasicPlanLevelId",
                table: "BasicPlans");

            migrationBuilder.DropColumn(
                name: "BasicPlanCategoryId",
                table: "BasicPlans");

            migrationBuilder.DropColumn(
                name: "BasicPlanLevelId",
                table: "BasicPlans");

            migrationBuilder.RenameColumn(
                name: "BasicPlanTypeId",
                table: "BasicPlans",
                newName: "PlanCategoryId");

            migrationBuilder.RenameIndex(
                name: "IX_BasicPlans_BasicPlanTypeId",
                table: "BasicPlans",
                newName: "IX_BasicPlans_PlanCategoryId");

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
