using MembershipService.API.Data;
using MembershipService.API.Repositories.Interfaces;
using MembershipService.API.Repositories.Implementations;
using MembershipService.API.Services.Interfaces;
using MembershipService.API.Services.Implementations;
using MembershipService.API.Mappings;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Load bi?n môi tr??ng t? file .env (nh? cách b?n dùng ? Auth)
DotNetEnv.Env.Load(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName, ".env"));

// Add services
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// ??ng ký DbContext
builder.Services.AddDbContext<MembershipDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default"),
        sql => sql.EnableRetryOnFailure()
    ));

// ??ng ký DI cho Ecosystem
builder.Services.AddScoped<IEcosystemRepository, EcosystemRepository>();
builder.Services.AddScoped<IEcosystemService, EcosystemService>();
builder.Services.AddAutoMapper(typeof(EcosystemProfile).Assembly);
// ??ng ký DI cho NextUService
builder.Services.AddScoped<INextUServiceRepository, NextUServiceRepository>();
builder.Services.AddScoped<INextUServiceService, NextUServiceService>();
// ??ng ký DI cho PackageDuration
builder.Services.AddScoped<IPackageDurationRepository, PackageDurationRepository>();
builder.Services.AddScoped<IPackageDurationService, PackageDurationService>();
builder.Services.AddAutoMapper(typeof(PackageDurationProfile));
// ??ng ký DI cho PackageLevel
builder.Services.AddScoped<IPackageLevelRepository, PackageLevelRepository>();
builder.Services.AddScoped<IPackageLevelService, PackageLevelService>();
builder.Services.AddAutoMapper(typeof(PackageLevelProfile));
// ??ng ký DI cho BasicPlan
builder.Services.AddScoped<IBasicPlanRepository, BasicPlanRepository>();
builder.Services.AddScoped<IBasicPlanService, BasicPlanService>();
builder.Services.AddAutoMapper(typeof(BasicPackageProfile).Assembly);
// ??ng ký DI cho ComboPlanDuration
builder.Services.AddScoped<IComboPlanDurationRepository, ComboPlanDurationRepository>();
// ??ng ký DI cho ComboPlan
builder.Services.AddScoped<IComboPlanBasicRepository, ComboPlanBasicRepository>();
builder.Services.AddScoped<IComboPlanRepository, ComboPlanRepository>();
builder.Services.AddScoped<IComboPlanService, ComboPlanService>();
builder.Services.AddAutoMapper(typeof(ComboPlanProfile).Assembly);




var app = builder.Build();

// Swagger n?u là môi tr??ng Dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();
