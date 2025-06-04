using AuthService.API.Helpers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Security.Claims;
using System.Text;

namespace AuthService.API.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration config)
    {
        var jwt = new JwtSettings
        {
            SecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")!,
            Issuer = Environment.GetEnvironmentVariable("JWT_ISSUER") ?? "AuthService",
            Audience = Environment.GetEnvironmentVariable("JWT_AUDIENCE") ?? "AllMicroservices",
            AccessTokenMinutes = int.Parse(Environment.GetEnvironmentVariable("JWT_ACCESS_TOKEN_MINUTES") ?? "30"),
            RefreshTokenDays = int.Parse(Environment.GetEnvironmentVariable("JWT_REFRESH_TOKEN_DAYS") ?? "7")
        };

        services.Configure<JwtSettings>(opts =>
        {
            opts.SecretKey = jwt.SecretKey;
            opts.Issuer = jwt.Issuer;
            opts.Audience = jwt.Audience;
            opts.AccessTokenMinutes = jwt.AccessTokenMinutes;
            opts.RefreshTokenDays = jwt.RefreshTokenDays;
        });

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.SecretKey));

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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

        return services;
    }

    public static IServiceCollection AddSwaggerWithBearer(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
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

        return services;
    }
}
