using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserService.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate1 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Cities",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Interests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Interests", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PersonalityTraits",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PersonalityTraits", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Skills",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Skills", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    CityId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Locations_Cities_CityId1",
                        column: x => x.CityId1,
                        principalTable: "Cities",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Propertys",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    Description = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LocationId1 = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Propertys", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Propertys_Locations_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Locations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Propertys_Locations_LocationId1",
                        column: x => x.LocationId1,
                        principalTable: "Locations",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "LocationMappings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    RegionName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    MembershipLocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false, defaultValueSql: "GETUTCDATE()")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LocationMappings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LocationMappings_Propertys_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Propertys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PendingThirdPartyRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RoleType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    SubmittedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ApprovedByManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ProfileDataJson = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingThirdPartyRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PendingThirdPartyRequests_Propertys_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Propertys",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "UserProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    DOB = table.Column<DateTime>(type: "datetime2", nullable: true),
                    AvatarUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Interests = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PersonalityTraits = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Introduction = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CvUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    SocialLinks = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RoleType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    IsCompleted = table.Column<bool>(type: "bit", nullable: false),
                    VerifiedByAdmin = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    OnboardingStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    CityId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserProfiles_Cities_CityId",
                        column: x => x.CityId,
                        principalTable: "Cities",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_UserProfiles_Propertys_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Propertys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "CoachProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CoachType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Specialty = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ModuleInCharge = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Region = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ExperienceYears = table.Column<int>(type: "int", nullable: true),
                    Bio = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Certifications = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    LinkedInUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CoachProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CoachProfiles_UserProfiles_AccountId",
                        column: x => x.AccountId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ManagerProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FullName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManagerProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ManagerProfiles_Propertys_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Propertys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_ManagerProfiles_UserProfiles_AccountId",
                        column: x => x.AccountId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PartnerProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OrganizationName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    PartnerType = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ContractUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    IsActivated = table.Column<bool>(type: "bit", nullable: false),
                    ActivatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedByAdminId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RepresentativeName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RepresentativePhone = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    RepresentativeEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Industry = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartnerProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartnerProfiles_Propertys_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Propertys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_PartnerProfiles_UserProfiles_AccountId",
                        column: x => x.AccountId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "PendingMembershipRequests",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    RequestedPackageName = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    AddOnsFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Interests = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PersonalityTraits = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    Introduction = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CvUrl = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: true),
                    MessageToStaff = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Status = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    StaffNote = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    ApprovedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    PaymentTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentTransactionId = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    PackageType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PackageDurationValue = table.Column<int>(type: "int", nullable: true),
                    PackageDurationUnit = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ExpireAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    RequireBooking = table.Column<bool>(type: "bit", nullable: false),
                    RoomInstanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PaymentNote = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PaymentProofUrl = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    PropertyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PendingMembershipRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PendingMembershipRequests_Propertys_PropertyId",
                        column: x => x.PropertyId,
                        principalTable: "Propertys",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_PendingMembershipRequests_UserProfiles_AccountId",
                        column: x => x.AccountId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StaffProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StaffGroup = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Department = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Level = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Address = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    ManagerId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Note = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    JoinedDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StaffProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StaffProfiles_Propertys_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Propertys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_StaffProfiles_UserProfiles_AccountId",
                        column: x => x.AccountId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SupplierProfiles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CompanyName = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContactPerson = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    ContactPhone = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    ContactEmail = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    Description = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true),
                    WebsiteUrl = table.Column<string>(type: "nvarchar(255)", maxLength: 255, nullable: true),
                    Industry = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: true),
                    TaxCode = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: true),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SupplierProfiles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SupplierProfiles_Propertys_LocationId",
                        column: x => x.LocationId,
                        principalTable: "Propertys",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_SupplierProfiles_UserProfiles_AccountId",
                        column: x => x.AccountId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserInterests",
                columns: table => new
                {
                    UserProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    InterestId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserInterests", x => new { x.UserProfileId, x.InterestId });
                    table.ForeignKey(
                        name: "FK_UserInterests_Interests_InterestId",
                        column: x => x.InterestId,
                        principalTable: "Interests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserInterests_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPersonalityTraits",
                columns: table => new
                {
                    UserProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonalityTraitId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPersonalityTraits", x => new { x.UserProfileId, x.PersonalityTraitId });
                    table.ForeignKey(
                        name: "FK_UserPersonalityTraits_PersonalityTraits_PersonalityTraitId",
                        column: x => x.PersonalityTraitId,
                        principalTable: "PersonalityTraits",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPersonalityTraits_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserSkills",
                columns: table => new
                {
                    UserProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    SkillId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserSkills", x => new { x.UserProfileId, x.SkillId });
                    table.ForeignKey(
                        name: "FK_UserSkills_Skills_SkillId",
                        column: x => x.SkillId,
                        principalTable: "Skills",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserSkills_UserProfiles_UserProfileId",
                        column: x => x.UserProfileId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Memberships",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackageType = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PackageName = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    AddOnsFee = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PurchasedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    StartDate = table.Column<DateTime>(type: "datetime2", nullable: true),
                    ExpireAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentStatus = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PackageDurationValue = table.Column<int>(type: "int", nullable: true),
                    PackageDurationUnit = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentTransactionId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PaymentTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    PaymentNote = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LocationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UsedForRoleUpgrade = table.Column<bool>(type: "bit", nullable: false),
                    PlanSource = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoomInstanceId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    BookingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserProfileId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PendingRequestId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: true),
                    CreatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UpdatedBy = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memberships", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Memberships_PendingMembershipRequests_PendingRequestId",
                        column: x => x.PendingRequestId,
                        principalTable: "PendingMembershipRequests",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.SetNull);
                    table.ForeignKey(
                        name: "FK_Memberships_UserProfiles_AccountId",
                        column: x => x.AccountId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Feedbacks",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PackageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OverallRating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(2000)", maxLength: 2000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MembershipId = table.Column<Guid>(type: "uniqueidentifier", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Feedbacks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Feedbacks_Memberships_MembershipId",
                        column: x => x.MembershipId,
                        principalTable: "Memberships",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Feedbacks_UserProfiles_AccountId",
                        column: x => x.AccountId,
                        principalTable: "UserProfiles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FeedbackDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FeedbackId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ServiceType = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    ServiceTargetId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Rating = table.Column<int>(type: "int", nullable: false),
                    Comment = table.Column<string>(type: "nvarchar(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeedbackDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FeedbackDetails_Feedbacks_FeedbackId",
                        column: x => x.FeedbackId,
                        principalTable: "Feedbacks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Cities",
                columns: new[] { "Id", "CreatedAt", "Description", "Name" },
                values: new object[] { new Guid("10000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Thủ đô Việt Nam", "Hà Nội" });

            migrationBuilder.InsertData(
                table: "Interests",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("20000000-0000-0000-0000-000000000001"), "Adventure travel" },
                    { new Guid("20000000-0000-0000-0000-000000000002"), "Alternative energy" },
                    { new Guid("20000000-0000-0000-0000-000000000003"), "Alternative medicine" },
                    { new Guid("20000000-0000-0000-0000-000000000004"), "Animal welfare" },
                    { new Guid("20000000-0000-0000-0000-000000000005"), "Astronomy" },
                    { new Guid("20000000-0000-0000-0000-000000000006"), "Athletics" },
                    { new Guid("20000000-0000-0000-0000-000000000007"), "Backpacking" },
                    { new Guid("20000000-0000-0000-0000-000000000008"), "Badminton" },
                    { new Guid("20000000-0000-0000-0000-000000000009"), "Baseball" },
                    { new Guid("20000000-0000-0000-0000-000000000010"), "Basketball" },
                    { new Guid("20000000-0000-0000-0000-000000000011"), "Beer tasting" },
                    { new Guid("20000000-0000-0000-0000-000000000012"), "Bicycling" },
                    { new Guid("20000000-0000-0000-0000-000000000013"), "Board games" },
                    { new Guid("20000000-0000-0000-0000-000000000014"), "Bowling" },
                    { new Guid("20000000-0000-0000-0000-000000000015"), "Brunch" },
                    { new Guid("20000000-0000-0000-0000-000000000016"), "Camping" },
                    { new Guid("20000000-0000-0000-0000-000000000017"), "Clubbing" },
                    { new Guid("20000000-0000-0000-0000-000000000018"), "Comedy" },
                    { new Guid("20000000-0000-0000-0000-000000000019"), "Conservation" },
                    { new Guid("20000000-0000-0000-0000-000000000020"), "Cooking" },
                    { new Guid("20000000-0000-0000-0000-000000000021"), "Crafts" },
                    { new Guid("20000000-0000-0000-0000-000000000022"), "DIY – Do it Yourself" },
                    { new Guid("20000000-0000-0000-0000-000000000023"), "Dancing" },
                    { new Guid("20000000-0000-0000-0000-000000000024"), "Dining out" },
                    { new Guid("20000000-0000-0000-0000-000000000025"), "Diving" },
                    { new Guid("20000000-0000-0000-0000-000000000026"), "Drinking" },
                    { new Guid("20000000-0000-0000-0000-000000000027"), "Education technology" },
                    { new Guid("20000000-0000-0000-0000-000000000028"), "Entrepreneurship" },
                    { new Guid("20000000-0000-0000-0000-000000000029"), "Environmental awareness" },
                    { new Guid("20000000-0000-0000-0000-000000000030"), "Fencing" },
                    { new Guid("20000000-0000-0000-0000-000000000031"), "Film" },
                    { new Guid("20000000-0000-0000-0000-000000000032"), "Finance technology" },
                    { new Guid("20000000-0000-0000-0000-000000000033"), "Fishing" },
                    { new Guid("20000000-0000-0000-0000-000000000034"), "Fitness" },
                    { new Guid("20000000-0000-0000-0000-000000000035"), "Frisbee" },
                    { new Guid("20000000-0000-0000-0000-000000000036"), "Gaming" },
                    { new Guid("20000000-0000-0000-0000-000000000037"), "Golf" },
                    { new Guid("20000000-0000-0000-0000-000000000038"), "Happy hour" },
                    { new Guid("20000000-0000-0000-0000-000000000039"), "Healing" },
                    { new Guid("20000000-0000-0000-0000-000000000040"), "Hiking" },
                    { new Guid("20000000-0000-0000-0000-000000000041"), "History" },
                    { new Guid("20000000-0000-0000-0000-000000000042"), "Holistic health" },
                    { new Guid("20000000-0000-0000-0000-000000000043"), "Horse riding" },
                    { new Guid("20000000-0000-0000-0000-000000000044"), "Human rights" },
                    { new Guid("20000000-0000-0000-0000-000000000045"), "Hunting" },
                    { new Guid("20000000-0000-0000-0000-000000000046"), "Ice skating" },
                    { new Guid("20000000-0000-0000-0000-000000000047"), "Innovation" },
                    { new Guid("20000000-0000-0000-0000-000000000048"), "International travel" },
                    { new Guid("20000000-0000-0000-0000-000000000049"), "Internet startups" },
                    { new Guid("20000000-0000-0000-0000-000000000050"), "Investing" },
                    { new Guid("20000000-0000-0000-0000-000000000051"), "Karaoke" },
                    { new Guid("20000000-0000-0000-0000-000000000052"), "Kayaking" },
                    { new Guid("20000000-0000-0000-0000-000000000053"), "Languages" },
                    { new Guid("20000000-0000-0000-0000-000000000054"), "Literature" },
                    { new Guid("20000000-0000-0000-0000-000000000055"), "Local culture" },
                    { new Guid("20000000-0000-0000-0000-000000000056"), "Marketing" },
                    { new Guid("20000000-0000-0000-0000-000000000057"), "Martial arts" },
                    { new Guid("20000000-0000-0000-0000-000000000058"), "Meditation" },
                    { new Guid("20000000-0000-0000-0000-000000000059"), "Mountain biking" },
                    { new Guid("20000000-0000-0000-0000-000000000060"), "Music" },
                    { new Guid("20000000-0000-0000-0000-000000000061"), "Natural parks" },
                    { new Guid("20000000-0000-0000-0000-000000000062"), "Networking" },
                    { new Guid("20000000-0000-0000-0000-000000000063"), "Neuroscience" },
                    { new Guid("20000000-0000-0000-0000-000000000064"), "Nightlife" },
                    { new Guid("20000000-0000-0000-0000-000000000065"), "Nutrition" },
                    { new Guid("20000000-0000-0000-0000-000000000066"), "Outdoor adventure" },
                    { new Guid("20000000-0000-0000-0000-000000000067"), "Outdoor sports" },
                    { new Guid("20000000-0000-0000-0000-000000000068"), "Painting" },
                    { new Guid("20000000-0000-0000-0000-000000000069"), "Photography" }
                });

            migrationBuilder.InsertData(
                table: "PersonalityTraits",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("30000000-0000-0000-0000-000000000001"), "Introvert" },
                    { new Guid("30000000-0000-0000-0000-000000000002"), "Optimistic" },
                    { new Guid("30000000-0000-0000-0000-000000000003"), "Extrovert" },
                    { new Guid("30000000-0000-0000-0000-000000000004"), "Realistic" },
                    { new Guid("30000000-0000-0000-0000-000000000005"), "Ambitious" },
                    { new Guid("30000000-0000-0000-0000-000000000006"), "Easygoing" },
                    { new Guid("30000000-0000-0000-0000-000000000007"), "Thoughtful" },
                    { new Guid("30000000-0000-0000-0000-000000000008"), "Energetic" },
                    { new Guid("30000000-0000-0000-0000-000000000009"), "Creative" },
                    { new Guid("30000000-0000-0000-0000-000000000010"), "Reliable" },
                    { new Guid("30000000-0000-0000-0000-000000000011"), "Adventurous" },
                    { new Guid("30000000-0000-0000-0000-000000000012"), "Compassionate" }
                });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("40000000-0000-0000-0000-000000000001"), "A/B testing" },
                    { new Guid("40000000-0000-0000-0000-000000000002"), "AI" },
                    { new Guid("40000000-0000-0000-0000-000000000003"), "API development" },
                    { new Guid("40000000-0000-0000-0000-000000000004"), "Accounting" },
                    { new Guid("40000000-0000-0000-0000-000000000005"), "Administrative support" },
                    { new Guid("40000000-0000-0000-0000-000000000006"), "Advertising" },
                    { new Guid("40000000-0000-0000-0000-000000000007"), "Affiliate marketing" },
                    { new Guid("40000000-0000-0000-0000-000000000008"), "Android development" },
                    { new Guid("40000000-0000-0000-0000-000000000009"), "Animators" },
                    { new Guid("40000000-0000-0000-0000-000000000010"), "Audio production" },
                    { new Guid("40000000-0000-0000-0000-000000000011"), "Back-end development" },
                    { new Guid("40000000-0000-0000-0000-000000000012"), "Blogging" },
                    { new Guid("40000000-0000-0000-0000-000000000013"), "Bookkeeping" },
                    { new Guid("40000000-0000-0000-0000-000000000014"), "Brand strategy" },
                    { new Guid("40000000-0000-0000-0000-000000000015"), "Branding" },
                    { new Guid("40000000-0000-0000-0000-000000000016"), "Business development" },
                    { new Guid("40000000-0000-0000-0000-000000000017"), "CRM management" },
                    { new Guid("40000000-0000-0000-0000-000000000018"), "Communication" },
                    { new Guid("40000000-0000-0000-0000-000000000019"), "Community management" },
                    { new Guid("40000000-0000-0000-0000-000000000020"), "Content" },
                    { new Guid("40000000-0000-0000-0000-000000000021"), "Content marketing" },
                    { new Guid("40000000-0000-0000-0000-000000000022"), "Copyediting" },
                    { new Guid("40000000-0000-0000-0000-000000000023"), "Copywriting" },
                    { new Guid("40000000-0000-0000-0000-000000000024"), "Creative writing" }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "CityId", "CityId1", "CreatedAt", "Description", "Name" },
                values: new object[] { new Guid("20000000-0000-0000-0000-000000000001"), new Guid("10000000-0000-0000-0000-000000000001"), null, new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), null, "Hoàng Cầu" });

            migrationBuilder.InsertData(
                table: "Propertys",
                columns: new[] { "Id", "CreatedAt", "Description", "LocationId", "LocationId1", "Name" },
                values: new object[,]
                {
                    { new Guid("30000000-0000-0000-0000-000000000001"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Khu vực trụ sở chính Hoàng Cầu 1", new Guid("20000000-0000-0000-0000-000000000001"), null, "Hoàng Cầu Cơ sở 1" },
                    { new Guid("30000000-0000-0000-0000-000000000002"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Khu vực trụ sở chính Hoàng Cầu 2", new Guid("20000000-0000-0000-0000-000000000001"), null, "Hoàng Cầu Cơ sở 2" },
                    { new Guid("30000000-0000-0000-0000-000000000003"), new DateTime(2024, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Khu vực trụ sở chính Hoàng Cầu 3", new Guid("20000000-0000-0000-0000-000000000001"), null, "Hoàng Cầu Cơ sở 3" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CoachProfiles_AccountId",
                table: "CoachProfiles",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_FeedbackDetails_FeedbackId",
                table: "FeedbackDetails",
                column: "FeedbackId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_AccountId",
                table: "Feedbacks",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Feedbacks_MembershipId",
                table: "Feedbacks",
                column: "MembershipId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationMappings_PropertyId_MembershipLocationId",
                table: "LocationMappings",
                columns: new[] { "PropertyId", "MembershipLocationId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Locations_CityId",
                table: "Locations",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_CityId1",
                table: "Locations",
                column: "CityId1");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerProfiles_AccountId",
                table: "ManagerProfiles",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_ManagerProfiles_LocationId",
                table: "ManagerProfiles",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_AccountId",
                table: "Memberships",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_Memberships_PendingRequestId",
                table: "Memberships",
                column: "PendingRequestId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerProfiles_AccountId",
                table: "PartnerProfiles",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PartnerProfiles_LocationId",
                table: "PartnerProfiles",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_PendingMembershipRequests_AccountId",
                table: "PendingMembershipRequests",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_PendingMembershipRequests_PropertyId",
                table: "PendingMembershipRequests",
                column: "PropertyId");

            migrationBuilder.CreateIndex(
                name: "IX_PendingThirdPartyRequests_LocationId",
                table: "PendingThirdPartyRequests",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Propertys_LocationId",
                table: "Propertys",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Propertys_LocationId1",
                table: "Propertys",
                column: "LocationId1");

            migrationBuilder.CreateIndex(
                name: "IX_StaffProfiles_AccountId",
                table: "StaffProfiles",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_StaffProfiles_LocationId",
                table: "StaffProfiles",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierProfiles_AccountId",
                table: "SupplierProfiles",
                column: "AccountId");

            migrationBuilder.CreateIndex(
                name: "IX_SupplierProfiles_LocationId",
                table: "SupplierProfiles",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserInterests_InterestId",
                table: "UserInterests",
                column: "InterestId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPersonalityTraits_PersonalityTraitId",
                table: "UserPersonalityTraits",
                column: "PersonalityTraitId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_AccountId",
                table: "UserProfiles",
                column: "AccountId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_CityId",
                table: "UserProfiles",
                column: "CityId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProfiles_LocationId",
                table: "UserProfiles",
                column: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_UserSkills_SkillId",
                table: "UserSkills",
                column: "SkillId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CoachProfiles");

            migrationBuilder.DropTable(
                name: "FeedbackDetails");

            migrationBuilder.DropTable(
                name: "LocationMappings");

            migrationBuilder.DropTable(
                name: "ManagerProfiles");

            migrationBuilder.DropTable(
                name: "PartnerProfiles");

            migrationBuilder.DropTable(
                name: "PendingThirdPartyRequests");

            migrationBuilder.DropTable(
                name: "StaffProfiles");

            migrationBuilder.DropTable(
                name: "SupplierProfiles");

            migrationBuilder.DropTable(
                name: "UserInterests");

            migrationBuilder.DropTable(
                name: "UserPersonalityTraits");

            migrationBuilder.DropTable(
                name: "UserSkills");

            migrationBuilder.DropTable(
                name: "Feedbacks");

            migrationBuilder.DropTable(
                name: "Interests");

            migrationBuilder.DropTable(
                name: "PersonalityTraits");

            migrationBuilder.DropTable(
                name: "Skills");

            migrationBuilder.DropTable(
                name: "Memberships");

            migrationBuilder.DropTable(
                name: "PendingMembershipRequests");

            migrationBuilder.DropTable(
                name: "UserProfiles");

            migrationBuilder.DropTable(
                name: "Propertys");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Cities");
        }
    }
}
