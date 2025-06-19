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

builder.Configuration.AddEnvironmentVariables();


var apiKey = builder.Configuration.GetValue<string>("InternalApi:ApiKey");
if (string.IsNullOrEmpty(apiKey))
{
    Console.WriteLine("⚠️ API Key for InternalApi not found!");
}
else
{
    Console.WriteLine($"API Key loaded: {apiKey}");
}

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSwaggerWithBearer();

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default"),
        sql => sql.EnableRetryOnFailure()
    ));

var authServiceUrl = builder.Configuration["Services:AuthService"];
var paymentServiceUrl = builder.Configuration["Services:PaymentService"];
var membershipServiceUrl = builder.Configuration["Services:MembershipService"];


builder.Services.AddHttpClient<IAuthServiceClient, AuthServiceClient>(client =>
{
    client.BaseAddress = new Uri(authServiceUrl);
});

builder.Services.AddHttpClient<IPaymentServiceClient, PaymentServiceClient>(client =>
{
    client.BaseAddress = new Uri(paymentServiceUrl);
});

builder.Services.AddHttpClient<IMembershipServiceClient, MembershipServiceClient>(client =>
{
    client.BaseAddress = new Uri(membershipServiceUrl); 
});

builder.Services.AddScoped<IUserProfileService, UserProfileService>();
builder.Services.AddScoped<ICoachProfileService, CoachProfileService>();
builder.Services.AddScoped<IStaffProfileService, StaffProfileService>();
builder.Services.AddScoped<IPartnerProfileService, PartnerProfileService>();
builder.Services.AddScoped<ISupplierProfileService, SupplierProfileService>();
builder.Services.AddScoped<IMembershipRequestService, MembershipRequestService>();

builder.Services.AddScoped<ICoachProfileRepository, CoachProfileRepository>();


builder.Services.AddHttpContextAccessor();

var app = builder.Build();


if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// Sử dụng middleware authentication và authorization
app.UseAuthentication();
app.UseAuthorization();

// Map controllers
app.MapControllers();

// Chạy app
app.Run();
