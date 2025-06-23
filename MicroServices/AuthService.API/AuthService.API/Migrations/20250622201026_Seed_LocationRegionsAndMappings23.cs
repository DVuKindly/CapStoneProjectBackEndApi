using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AuthService.API.Migrations
{
    /// <inheritdoc />
    public partial class Seed_LocationRegionsAndMappings23 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "PermissionId", "Description", "PermissionKey" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0007"), "Tạo Manager", "create_manager" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0008"), "Tạo Staff Onboarding", "create_staff_onboarding" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0009"), "Tạo Staff Service", "create_staff_service" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0010"), "Tạo Staff Content", "create_staff_content" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0011"), "Tạo Coach", "create_coach" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0004"), new Guid("7b6c3929-7bce-48e8-9f60-b0df90792c5c") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0005"), new Guid("7b6c3929-7bce-48e8-9f60-b0df90792c5c") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0006"), new Guid("7b6c3929-7bce-48e8-9f60-b0df90792c5c") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0007"), new Guid("7b6c3929-7bce-48e8-9f60-b0df90792c5c") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0008"), new Guid("7b6c3929-7bce-48e8-9f60-b0df90792c5c") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0009"), new Guid("7b6c3929-7bce-48e8-9f60-b0df90792c5c") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0010"), new Guid("7b6c3929-7bce-48e8-9f60-b0df90792c5c") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0011"), new Guid("7b6c3929-7bce-48e8-9f60-b0df90792c5c") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0007"), new Guid("cca492d0-42b4-42b4-a3ec-4265a521b5f8") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0008"), new Guid("cca492d0-42b4-42b4-a3ec-4265a521b5f8") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0009"), new Guid("cca492d0-42b4-42b4-a3ec-4265a521b5f8") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0010"), new Guid("cca492d0-42b4-42b4-a3ec-4265a521b5f8") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0011"), new Guid("cca492d0-42b4-42b4-a3ec-4265a521b5f8") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0004"), new Guid("7b6c3929-7bce-48e8-9f60-b0df90792c5c") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0005"), new Guid("7b6c3929-7bce-48e8-9f60-b0df90792c5c") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0006"), new Guid("7b6c3929-7bce-48e8-9f60-b0df90792c5c") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0007"), new Guid("7b6c3929-7bce-48e8-9f60-b0df90792c5c") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0008"), new Guid("7b6c3929-7bce-48e8-9f60-b0df90792c5c") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0009"), new Guid("7b6c3929-7bce-48e8-9f60-b0df90792c5c") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0010"), new Guid("7b6c3929-7bce-48e8-9f60-b0df90792c5c") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0011"), new Guid("7b6c3929-7bce-48e8-9f60-b0df90792c5c") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0007"), new Guid("cca492d0-42b4-42b4-a3ec-4265a521b5f8") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0008"), new Guid("cca492d0-42b4-42b4-a3ec-4265a521b5f8") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0009"), new Guid("cca492d0-42b4-42b4-a3ec-4265a521b5f8") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0010"), new Guid("cca492d0-42b4-42b4-a3ec-4265a521b5f8") });

            migrationBuilder.DeleteData(
                table: "RolePermissions",
                keyColumns: new[] { "PermissionId", "RoleId" },
                keyValues: new object[] { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0011"), new Guid("cca492d0-42b4-42b4-a3ec-4265a521b5f8") });

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0007"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0008"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0009"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0010"));

            migrationBuilder.DeleteData(
                table: "Permissions",
                keyColumn: "PermissionId",
                keyValue: new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0011"));
        }
    }
}
