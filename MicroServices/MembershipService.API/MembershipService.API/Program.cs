using MembershipService.API.Data;
using MembershipService.API.Repositories.Interfaces;
using MembershipService.API.Repositories.Implementations;
using MembershipService.API.Services.Interfaces;
using MembershipService.API.Services.Implementations;
using MembershipService.API.Mappings;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Load bi?n m�i tr??ng t? file .env (nh? c�ch b?n d�ng ? Auth)
DotNetEnv.Env.Load(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName, ".env"));

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ??ng k� DbContext
builder.Services.AddDbContext<MembershipDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default"),
        sql => sql.EnableRetryOnFailure()
    ));

// ??ng k� DI cho Ecosystem
builder.Services.AddScoped<IEcosystemRepository, EcosystemRepository>();
builder.Services.AddScoped<IEcosystemService, EcosystemService>();
builder.Services.AddAutoMapper(typeof(EcosystemProfile).Assembly);
// ??ng k� DI cho NextUService
builder.Services.AddScoped<INextUServiceRepository, NextUServiceRepository>();
builder.Services.AddScoped<INextUServiceService, NextUServiceService>();
// ??ng k� DI cho PackageDuration
builder.Services.AddScoped<IPackageDurationRepository, PackageDurationRepository>();
builder.Services.AddScoped<IPackageDurationService, PackageDurationService>();
builder.Services.AddAutoMapper(typeof(PackageDurationProfile));
// ??ng k� DI cho PackageLevel
builder.Services.AddScoped<IPackageLevelRepository, PackageLevelRepository>();
builder.Services.AddScoped<IPackageLevelService, PackageLevelService>();
builder.Services.AddAutoMapper(typeof(PackageLevelProfile));
// ??ng k� DI cho BasicPlan
builder.Services.AddScoped<IBasicPlanRepository, BasicPlanRepository>();
builder.Services.AddScoped<IBasicPlanService, BasicPlanService>();
builder.Services.AddAutoMapper(typeof(BasicPlanProfile).Assembly);
// ??ng k� DI cho ComboPlanDuration
builder.Services.AddScoped<IComboPlanDurationRepository, ComboPlanDurationRepository>();
// ??ng k� DI cho ComboPlan
builder.Services.AddScoped<IComboPlanBasicRepository, ComboPlanBasicRepository>();
builder.Services.AddScoped<IComboPlanRepository, ComboPlanRepository>();
builder.Services.AddScoped<IComboPlanService, ComboPlanService>();
builder.Services.AddAutoMapper(typeof(ComboPlanProfile).Assembly);
// ??ng k� DI cho AccommodationOption
builder.Services.AddScoped<IAccommodationOptionRepository, AccommodationOptionRepository>();
builder.Services.AddScoped<IAccommodationOptionService, AccommodationOptionService>();
builder.Services.AddAutoMapper(typeof(AccommodationOptionProfile).Assembly);
// ??ng k� DI cho RoomInstance
builder.Services.AddScoped<IRoomInstanceRepository, RoomInstanceRepository>();
builder.Services.AddScoped<IRoomInstanceService, RoomInstanceService>();
builder.Services.AddAutoMapper(typeof(RoomInstanceProfile).Assembly);
// ??ng k� DI cho BasicPlanRoom
builder.Services.AddScoped<IBasicPlanRoomRepository, BasicPlanRoomRepository>();
// ??ng k� DI cho BasicPlanRoom
builder.Services.AddScoped<IBasicPlanTypeRepository, BasicPlanTypeRepository>();
builder.Services.AddScoped<IBasicPlanTypeService, BasicPlanTypeService>();
builder.Services.AddAutoMapper(typeof(BasicPlanTypeProfile).Assembly);
// ??ng k� DI cho Bookings
builder.Services.AddScoped<IBookingRepository, BookingRepository>();
builder.Services.AddScoped<IBookingService, BookingService>();
builder.Services.AddAutoMapper(typeof(BookingProfile).Assembly);




var app = builder.Build();

// Swagger n?u l� m�i tr??ng Dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
