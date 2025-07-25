using MembershipService.API.Data;
using MembershipService.API.Repositories.Interfaces;
using MembershipService.API.Repositories.Implementations;
using MembershipService.API.Services.Interfaces;
using MembershipService.API.Services.Implementations;
using MembershipService.API.Mappings;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// 1. Load environment variables from .env file
var envPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName, ".env");
DotNetEnv.Env.Load(envPath);

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. DbContext configuration for Membership database
builder.Services.AddDbContext<MembershipDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default"),
        sql => sql.EnableRetryOnFailure()
    ));

// 3. Azure Blob Storage Configuration from .env
var azureBlobStorageConnectionString = Environment.GetEnvironmentVariable("AZURE_BLOB_STORAGE_CONNECTION_STRING");
var azureBlobStorageContainerName = Environment.GetEnvironmentVariable("AZURE_BLOB_STORAGE_CONTAINER_NAME");

builder.Services.AddScoped<IBlobService, BlobService>();

// 4. Add dependency injection (DI) for MembershipService repositories and services
builder.Services.AddScoped<IEcosystemRepository, EcosystemRepository>();
builder.Services.AddScoped<IEcosystemService, EcosystemService>();
builder.Services.AddAutoMapper(typeof(EcosystemProfile).Assembly);

builder.Services.AddScoped<INextUServiceRepository, NextUServiceRepository>();
builder.Services.AddScoped<INextUServiceService, NextUServiceService>();

builder.Services.AddScoped<IPackageDurationRepository, PackageDurationRepository>();
builder.Services.AddScoped<IPackageDurationService, PackageDurationService>();
builder.Services.AddAutoMapper(typeof(PackageDurationProfile));

builder.Services.AddScoped<IBasicPlanRepository, BasicPlanRepository>();
builder.Services.AddScoped<IBasicPlanService, BasicPlanService>();
builder.Services.AddAutoMapper(typeof(BasicPlanProfile).Assembly);

builder.Services.AddScoped<IComboPlanDurationRepository, ComboPlanDurationRepository>();
builder.Services.AddScoped<IComboPlanBasicRepository, ComboPlanBasicRepository>();
builder.Services.AddScoped<IComboPlanRepository, ComboPlanRepository>();
builder.Services.AddScoped<IComboPlanService, ComboPlanService>();
builder.Services.AddAutoMapper(typeof(ComboPlanProfile).Assembly);

builder.Services.AddScoped<IAccommodationOptionRepository, AccommodationOptionRepository>();
builder.Services.AddScoped<IAccommodationOptionService, AccommodationOptionService>();
builder.Services.AddAutoMapper(typeof(AccommodationOptionProfile).Assembly);

builder.Services.AddScoped<IRoomInstanceRepository, RoomInstanceRepository>();
builder.Services.AddScoped<IRoomInstanceService, RoomInstanceService>();
builder.Services.AddAutoMapper(typeof(RoomInstanceProfile).Assembly);

builder.Services.AddScoped<IBasicPlanRoomRepository, BasicPlanRoomRepository>();
builder.Services.AddScoped<IBasicPlanEntitlementRepository, BasicPlanEntitlementRepository>();

builder.Services.AddScoped<IBasicPlanTypeRepository, BasicPlanTypeRepository>();
builder.Services.AddScoped<IBasicPlanTypeService, BasicPlanTypeService>();
builder.Services.AddAutoMapper(typeof(BasicPlanTypeProfile).Assembly);

builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddAutoMapper(typeof(BookingProfile).Assembly);

builder.Services.AddScoped<IEntitlementRuleRepository, EntitlementRuleRepository>();
builder.Services.AddScoped<IEntitlementRuleService, EntitlementRuleService>();
builder.Services.AddAutoMapper(typeof(EntitlementRuleProfile).Assembly);

builder.Services.AddScoped<IMediaRepository, MediaRepository>();
builder.Services.AddScoped<IMediaService, MediaService>();
builder.Services.AddScoped<IBlobService, BlobService>();

// 5. PlanOptions Configuration
builder.Services.AddScoped<IPlanCategoryRepository, PlanCategoryRepository>();
builder.Services.AddScoped<IPlanCategoryService, PlanCategoryService>();

builder.Services.AddScoped<IPlanLevelRepository, PlanLevelRepository>();
builder.Services.AddScoped<IPlanLevelService, PlanLevelService>();

builder.Services.AddScoped<IPlanTargetAudienceRepository, PlanTargetAudienceRepository>();
builder.Services.AddScoped<IPlanTargetAudienceService, PlanTargetAudienceService>();

builder.Services.AddAutoMapper(typeof(PlanCateProfile).Assembly);

// 6. RoomOptions Configuration
builder.Services.AddScoped<IRoomSizeOptionRepository, RoomSizeOptionRepository>();
builder.Services.AddScoped<IRoomSizeOptionService, RoomSizeOptionService>();

builder.Services.AddScoped<IRoomViewOptionRepository, RoomViewOptionRepository>();
builder.Services.AddScoped<IRoomViewOptionService, RoomViewOptionService>();

builder.Services.AddScoped<IRoomFloorOptionRepository, RoomFloorOptionRepository>();
builder.Services.AddScoped<IRoomFloorOptionService, RoomFloorOptionService>();

builder.Services.AddScoped<IBedTypeOptionRepository, BedTypeOptionRepository>();
builder.Services.AddScoped<IBedTypeOptionService, BedTypeOptionService>();

builder.Services.AddAutoMapper(typeof(RoomOptionsProfile).Assembly);

// Build the application
var app = builder.Build();

// Swagger in development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
