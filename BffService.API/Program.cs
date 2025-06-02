using BffService.API.Services;
using DotNetEnv;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Load .env
var envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", ".env");
if (!File.Exists(envPath))
{
    Console.WriteLine($"❌ .env file not found at: {envPath}");
    throw new FileNotFoundException(".env file not found.");
}
DotNetEnv.Env.Load(envPath);

// Load JWT từ .env
var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
               ?? throw new InvalidOperationException("Missing JWT_SECRET_KEY in .env");

var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

// Setup JWT Authentication
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

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddHttpClient<AuthProxyService>((sp, client) =>
{
    var config = sp.GetRequiredService<IConfiguration>();
    var baseUrl = config["Services:AuthService"];
    if (string.IsNullOrEmpty(baseUrl))
        throw new InvalidOperationException("AuthService URL missing in appsettings.json");

    client.BaseAddress = new Uri(baseUrl);
});

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors();
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.Run();
