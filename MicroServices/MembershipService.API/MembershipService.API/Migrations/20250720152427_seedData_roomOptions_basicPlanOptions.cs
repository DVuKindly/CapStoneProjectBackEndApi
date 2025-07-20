using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MembershipService.API.Migrations
{
    /// <inheritdoc />
    public partial class seedData_roomOptions_basicPlanOptions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "BasicPlanTypes",
                columns: new[] { "Id", "Code", "CreatedAt", "CreatedBy", "Description", "Name", "UpdatedAt", "UpdatedBy" },
                values: new object[,]
                {
                    { new Guid("60000000-0000-0000-0000-000000000001"), "LIVING", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Các gói dịch vụ liên quan đến chỗ ở", "Living", null, null },
                    { new Guid("60000000-0000-0000-0000-000000000003"), "LIFE_ACTIVITY", new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Các hoạt động trải nghiệm, xã hội, giải trí", "Life Activity", null, null }
                });

            migrationBuilder.InsertData(
                table: "PackageDurations",
                columns: new[] { "Id", "CreatedAt", "CreatedBy", "Description", "Unit", "UpdatedAt", "UpdatedBy", "Value" },
                values: new object[,]
                {
                    { 1, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "1 tháng", 2, null, null, 1 },
                    { 2, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "3 tháng", 2, null, null, 3 },
                    { 3, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "6 tháng", 2, null, null, 6 }
                });

            migrationBuilder.InsertData(
                table: "PlanTargetAudiences",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { 1, "Cá nhân" },
                    { 2, "Nhóm" },
                    { 3, "Doanh nghiệp" }
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
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "BasicPlanCategories",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BasicPlanCategories",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BasicPlanCategories",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "BasicPlanLevels",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "BasicPlanLevels",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "BasicPlanLevels",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "BasicPlanTypes",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0000-0000-0000-000000000003"));

            migrationBuilder.DeleteData(
                table: "PackageDurations",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PackageDurations",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PackageDurations",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "PlanTargetAudiences",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "PlanTargetAudiences",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "PlanTargetAudiences",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "BasicPlanTypes",
                keyColumn: "Id",
                keyValue: new Guid("60000000-0000-0000-0000-000000000001"));
        }
    }
}
