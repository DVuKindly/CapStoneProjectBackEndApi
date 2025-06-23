// Program.cs
using AuthService.API.Data;
using AuthService.API.Entities;
using AuthService.API.Extensions;
using AuthService.API.Helpers;
using AuthService.API.Repositories;
using AuthService.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

var envPath = Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName, ".env");
DotNetEnv.Env.Load(envPath);

// 2. Thêm biến môi trường vào IConfiguration
builder.Configuration.AddEnvironmentVariables();
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();


var internalApiKey = builder.Configuration["InternalApi:ApiKey"];
if (string.IsNullOrEmpty(internalApiKey))
{
    Console.WriteLine("⚠️ API Key for InternalApi not found or empty!");
}
else
{
    Console.WriteLine($"✅ API Key loaded: {internalApiKey}");
}

// Setup JWT + Swagger
builder.Services.AddJwtAuthentication(builder.Configuration);
builder.Services.AddSwaggerWithBearer();

// Auth DB
builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default"),
        sql => sql.EnableRetryOnFailure()
    ));

// Dependency Injection
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService.API.Services.AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IPasswordHasher<UserAuth>, PasswordHasher<UserAuth>>();
builder.Services.AddScoped<IUserServiceClient, UserServiceClient>();
builder.Services.AddHttpContextAccessor();

builder.Services.Configure<EmailSettings>(options =>
{
    options.Host = Environment.GetEnvironmentVariable("EMAIL_HOST")!;
    options.Port = int.Parse(Environment.GetEnvironmentVariable("EMAIL_PORT")!);
    options.Username = Environment.GetEnvironmentVariable("EMAIL_USERNAME")!;
    options.Password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD")!;
    options.FromName = Environment.GetEnvironmentVariable("EMAIL_FROM_NAME") ?? "NextU";
});

builder.Services.Configure<JwtSettings>(options =>
{
    options.SecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY") ?? throw new Exception("Missing JWT_SECRET_KEY in .env");
    options.Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "AuthService";
    options.Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "AllMicroservices";
    options.AccessTokenMinutes = int.Parse(Environment.GetEnvironmentVariable("JWT_ACCESS_TOKEN_MINUTES") ?? "30");
    options.RefreshTokenDays = int.Parse(Environment.GetEnvironmentVariable("JWT_REFRESH_TOKEN_DAYS") ?? "7");
});



var userServiceUrl = builder.Configuration["Services:UserService"];
builder.Services.AddHttpClient<IUserServiceClient, UserServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5000"); // ✅
    client.DefaultRequestHeaders.Add("X-Internal-Call", "true");
});




var app = builder.Build();

// Seed admin user
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<AuthDbContext>();
    var hasher = services.GetRequiredService<IPasswordHasher<UserAuth>>();
    var userServiceClient = services.GetRequiredService<IUserServiceClient>();
    await DbInitializer.SeedAsync(context, hasher, userServiceClient);
}



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();


























