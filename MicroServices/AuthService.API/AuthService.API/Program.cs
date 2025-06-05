using AuthService.API.Data;
using AuthService.API.Entities;
using AuthService.API.Helpers;
using AuthService.API.Repositories;
using AuthService.API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;
using DotNetEnv;

var builder = WebApplication.CreateBuilder(args);

// Load .env
DotNetEnv.Env.Load(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName, ".env"));

// Thêm các dịch vụ cần thiết
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// Cấu hình JWT Authentication
var jwt = new JwtSettings
{
    SecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")!,
    Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "AuthService",
    Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "AllMicroservices",
    AccessTokenMinutes = int.Parse(Environment.GetEnvironmentVariable("JWT_ACCESS_TOKEN_MINUTES") ?? "30"),
    RefreshTokenDays = int.Parse(Environment.GetEnvironmentVariable("JWT_REFRESH_TOKEN_DAYS") ?? "7")
};

// Đăng ký JWT Settings
builder.Services.Configure<JwtSettings>(opts =>
{
    opts.SecretKey = jwt.SecretKey;
    opts.Issuer = jwt.Issuer;
    opts.Audience = jwt.Audience;
    opts.AccessTokenMinutes = jwt.AccessTokenMinutes;
    opts.RefreshTokenDays = jwt.RefreshTokenDays;
});

// Cấu hình JWT Authentication
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
        .AddJwtBearer(options =>
        {
            options.RequireHttpsMetadata = false;
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = jwt.Issuer,
                ValidateAudience = true,
                ValidAudience = jwt.Audience,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                ClockSkew = TimeSpan.Zero,
                NameClaimType = ClaimTypes.NameIdentifier,
                RoleClaimType = ClaimTypes.Role
            };
        });


builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "AuthService.API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter 'Bearer {token}'",
        Name = "Authorization",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.ApiKey,
        Scheme = "Bearer"
    });
    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            Array.Empty<string>()
        }
    });
});

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default"),
        sql => sql.EnableRetryOnFailure()
    ));

// Cấu hình các dịch vụ
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService.API.Services.AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IPasswordHasher<UserAuth>, PasswordHasher<UserAuth>>();
builder.Services.AddScoped<IUserServiceClient, UserServiceClient>();

builder.Services.Configure<EmailSettings>(options =>
{
    options.Host = Environment.GetEnvironmentVariable("EMAIL_HOST")!;
    options.Port = int.Parse(Environment.GetEnvironmentVariable("EMAIL_PORT")!);
    options.Username = Environment.GetEnvironmentVariable("EMAIL_USERNAME")!;
    options.Password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD")!;
    options.FromName = Environment.GetEnvironmentVariable("EMAIL_FROM_NAME") ?? "NextU";
});

builder.Services.AddHttpClient<IUserServiceClient, UserServiceClient>(client =>
{
    client.BaseAddress = new Uri("http://localhost:5005");
});


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
