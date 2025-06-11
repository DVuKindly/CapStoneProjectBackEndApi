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

// Load .env
DotNetEnv.Env.Load(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName, ".env"));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

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

var userServiceUrl = builder.Configuration["Services:UserService"];

builder.Services.AddHttpClient<IUserServiceClient, UserServiceClient>(client =>
{
    client.BaseAddress = new Uri(userServiceUrl);
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
