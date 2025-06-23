using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace AuthService.API.Migrations
{
    /// <inheritdoc />
    public partial class Seed_LocationRegionsAndMappings2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AuthUsers",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    EmailVerified = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    EmailVerificationToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EmailVerificationExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ResetPasswordToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ResetPasswordTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    SetPasswordToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SetPasswordTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RefreshToken = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RefreshTokenExpiry = table.Column<DateTime>(type: "datetime2", nullable: true),
                    LoginAttempt = table.Column<int>(type: "int", nullable: false),
                    IsLocked = table.Column<bool>(type: "bit", nullable: false, defaultValue: false),
                    Provider = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProviderId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP"),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "CURRENT_TIMESTAMP")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AuthUsers", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "Permissions",
                columns: table => new
                {
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionKey = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Permissions", x => x.PermissionId);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RoleName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.RoleId);
                });

            migrationBuilder.CreateTable(
                name: "UserPermissions",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPermissions", x => new { x.UserId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_UserPermissions_AuthUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AuthUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RolePermissions",
                columns: table => new
                {
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PermissionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RolePermissions", x => new { x.RoleId, x.PermissionId });
                    table.ForeignKey(
                        name: "FK_RolePermissions_Permissions_PermissionId",
                        column: x => x.PermissionId,
                        principalTable: "Permissions",
                        principalColumn: "PermissionId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RolePermissions_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserRoles",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserRoles", x => new { x.UserId, x.RoleId });
                    table.ForeignKey(
                        name: "FK_UserRoles_AuthUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AuthUsers",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "RoleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Permissions",
                columns: new[] { "PermissionId", "Description", "PermissionKey" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"), "Gán role", "assign_role" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0002"), "Gán quyền cho role", "assign_permission_to_role" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0003"), "Gán quyền cho user", "assign_permission_to_user" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0004"), "Truy cập dashboard admin", "access_admin_dashboard" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0005"), "Tạo nhà cung cấp", "create_supplier" },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0006"), "Tạo đối tác", "create_partner" }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "RoleId", "Description", "RoleKey", "RoleName" },
                values: new object[,]
                {
                    { new Guid("05387973-58b6-48d1-9cc4-11874d712149"), "Duyệt nội dung và package", "manager", "Manager" },
                    { new Guid("78454018-bec6-459d-85e2-4265a521b5f8"), "Thành viên trả phí", "member", "Member" },
                    { new Guid("7b6c3929-7bce-48e8-9f60-b0df90792c5c"), "Tạo các role khác trong khu vực", "admin", "Admin khu vực" },
                    { new Guid("833d3494-42b4-2222-afc2-4265a521c5f8"), "Đăng nội dung", "staff_content", "Staff Content" },
                    { new Guid("833d5492-afc2-42b4-afc2-8265a521b5f8"), "Nhà cung cấp nội dung", "supplier", "Supplier" },
                    { new Guid("833d5494-3559-4826-a3ec-58b0b14c7c81"), "Hướng dẫn viên / mentor", "coaching", "Coaching" },
                    { new Guid("cca492d0-42b4-42b4-a3ec-4265a521b5f8"), "Toàn quyền, tạo admin khu vực", "super_admin", "Super Admin" },
                    { new Guid("cca492d0-4de1-42b4-afc2-bb613eeb219b"), "Đối tác nội dung", "partner", "Partner" },
                    { new Guid("d9f65410-6925-4351-a5ad-4aaaf5df0305"), "Tạo event hoạt động Co-Living", "staff_service", "Staff Service" },
                    { new Guid("ed3a2ae2-f05b-47e6-949f-28fb477d5b24"), "Khách đăng nhập", "user", "User" },
                    { new Guid("f5d32824-116a-4f97-ae91-73c7d84c6486"), "Tạo package, duyệt user-member", "staff_onboarding", "Staff Onboarding" }
                });

            migrationBuilder.InsertData(
                table: "RolePermissions",
                columns: new[] { "PermissionId", "RoleId" },
                values: new object[,]
                {
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0001"), new Guid("cca492d0-42b4-42b4-a3ec-4265a521b5f8") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0002"), new Guid("cca492d0-42b4-42b4-a3ec-4265a521b5f8") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0003"), new Guid("cca492d0-42b4-42b4-a3ec-4265a521b5f8") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0004"), new Guid("cca492d0-42b4-42b4-a3ec-4265a521b5f8") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0005"), new Guid("cca492d0-42b4-42b4-a3ec-4265a521b5f8") },
                    { new Guid("aaaaaaaa-aaaa-aaaa-aaaa-aaaaaaaa0006"), new Guid("cca492d0-42b4-42b4-a3ec-4265a521b5f8") }
                });

            migrationBuilder.CreateIndex(
                name: "IX_RolePermissions_PermissionId",
                table: "RolePermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPermissions_PermissionId",
                table: "UserPermissions",
                column: "PermissionId");

            migrationBuilder.CreateIndex(
                name: "IX_UserRoles_RoleId",
                table: "UserRoles",
                column: "RoleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RolePermissions");

            migrationBuilder.DropTable(
                name: "UserPermissions");

            migrationBuilder.DropTable(
                name: "UserRoles");

            migrationBuilder.DropTable(
                name: "Permissions");

            migrationBuilder.DropTable(
                name: "AuthUsers");

            migrationBuilder.DropTable(
                name: "Roles");
        }
    }
}
