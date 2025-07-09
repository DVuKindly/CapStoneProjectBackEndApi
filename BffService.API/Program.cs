using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// === Load .env ===
var envPath = Path.Combine(Directory.GetCurrentDirectory(), "..", ".env");
if (!File.Exists(envPath))
{
    Console.WriteLine($"❌ .env file not found at: {envPath}");
    throw new FileNotFoundException(".env file not found.");
}
DotNetEnv.Env.Load(envPath);

// === JWT Secret Key ===
var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET_KEY")
               ?? throw new InvalidOperationException("Missing JWT_SECRET_KEY in .env");
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSecret));

// === CORS ===
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend3000", policy =>
    {
        policy.WithOrigins("http://localhost:3000", "http://localhost:3111")
              .AllowAnyHeader()
              .AllowAnyMethod()
              .AllowCredentials();
    });
});

builder.Services.AddHttpContextAccessor();

// === JWT Authentication ===
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

// === Register Proxy HttpClients ===
builder.Services.AddHttpClient("AuthService", client =>
{
    client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("AUTHSERVICE_URL") ?? "http://localhost:5001");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddHttpClient("UserService", client =>
{
    client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("USERSERVICE_URL") ?? "http://localhost:5005");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddHttpClient("PaymentService", client =>
{
    client.BaseAddress = new Uri(Environment.GetEnvironmentVariable("PAYMENTSERVICE_URL") ?? "http://localhost:5010");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

builder.Services.AddHttpClient("MembershipService", client =>
{
    client.BaseAddress = new Uri("http://localhost:5003");
    client.DefaultRequestHeaders.Add("Accept", "application/json");
});

// === Controllers + Reverse Proxy ===
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddReverseProxy()
    .LoadFromConfig(builder.Configuration.GetSection("ReverseProxy"));

// === Swagger + Bearer Auth ===
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "BffService.API", Version = "v1" });
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Description = "Enter 'Bearer' [space] and then your token",
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

var app = builder.Build();

// === Middleware Pipeline ===
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "BffService API v1");
        c.RoutePrefix = "swagger";
    });
}

app.UseCors("AllowFrontend3000");

app.Use(async (context, next) =>
{
    var path = context.Request.Path;
    var token = context.Request.Headers["Authorization"].FirstOrDefault();
    Console.WriteLine($"➡️ {path} | Token: {token}");
    await next();
});

app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.MapReverseProxy();

app.Run();
