using AuthService.API.Data;
using AuthService.API.Entities;
using AuthService.API.Helpers;
using AuthService.API.Repositories;
using AuthService.API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using AuthService.API.Helpers;

var builder = WebApplication.CreateBuilder(args);

DotNetEnv.Env.Load(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName, ".env"));


builder.Services.AddControllers();
 https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.Configure<EmailSettings>(options =>
{
    options.Host = Environment.GetEnvironmentVariable("EMAIL_HOST")!;
    options.Port = int.Parse(Environment.GetEnvironmentVariable("EMAIL_PORT")!);
    options.Username = Environment.GetEnvironmentVariable("EMAIL_USERNAME")!;
    options.Password = Environment.GetEnvironmentVariable("EMAIL_PASSWORD")!;
    options.FromName = Environment.GetEnvironmentVariable("EMAIL_FROM_NAME") ?? "NextU";
});

builder.Services.AddScoped<IEmailService, EmailService>();


DotNetEnv.Env.Load(Path.Combine(Directory.GetParent(Directory.GetCurrentDirectory())!.Parent!.Parent!.FullName, ".env"));

builder.Services.AddDbContext<AuthDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("Default"),
        sql => sql.EnableRetryOnFailure()
    ));

builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IAuthService, AuthService.API.Services.AuthService>();
builder.Services.AddScoped<ITokenService, TokenService>();
builder.Services.AddScoped<IEmailService, EmailService>();
builder.Services.AddScoped<IPasswordHasher<UserAuth>, PasswordHasher<UserAuth>>();

var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
               ?? throw new InvalidOperationException("Missing JWT_SECRET_KEY in .env");

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = false,
        ValidateAudience = false,
        ValidateIssuerSigningKey = true,
        IssuerSigningKey = key,
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthentication(); 
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
