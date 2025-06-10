using Microsoft.EntityFrameworkCore;
using UserService.API.Data;
using UserService.API.Extensions;
using UserService.API.Repositories.Implementations;
using UserService.API.Repositories.Interfaces;

using DotNetEnv;
using UserService.API.Services.Implementations;
using UserService.API.Services.Interfaces;
using UserService.API.Services.CleanData;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName, ".env"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSwaggerWithBearer();

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default"),
        sql => sql.EnableRetryOnFailure()
    ));
builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<ICoachProfileService, CoachProfileService>();
builder.Services.AddScoped<IStaffProfileService, StaffProfileService>();
builder.Services.AddScoped<IPartnerProfileService, PartnerProfileService>();
builder.Services.AddScoped<ISupplierProfileService,SupplierProfileService>();
builder.Services.AddScoped<IMembershipRequestService, MembershipRequestService>();
//builder.Services.AddHostedService<RejectedCleanupService>();
builder.Services.AddHttpContextAccessor();




builder.Services.AddScoped<ICoachProfileRepository, CoachProfileRepository>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
