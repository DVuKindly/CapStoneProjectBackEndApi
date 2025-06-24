using Microsoft.EntityFrameworkCore;
using PaymentService.API.Data;
using PaymentService.API.Services;
using PaymentService.API.Services.Interfaces;
using System.Text.Json.Serialization;

namespace PaymentService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // 👇 Configure logging (optional, bổ sung để debug)
            builder.Logging.ClearProviders();
            builder.Logging.AddConsole();

            // ✅ Add Controllers with JSON config to avoid circular ref
            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
                    options.JsonSerializerOptions.PropertyNamingPolicy = null; // Optional: giữ đúng tên
                });

            // ✅ Add Swagger
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // ✅ Add EF Core
            builder.Services.AddDbContext<PaymentDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("Default"),
                    sql => sql.EnableRetryOnFailure()
                ));

            // ✅ Register Services
            builder.Services.AddScoped<IPaymentService, PaymentService.API.Services.PaymentService>();
            builder.Services.AddScoped<IPaymentWebhookService, PaymentWebhookService>();
            builder.Services.AddHttpClient<IPaymentResultHandler, PaymentResultHandler>(); // gọi về UserService
            builder.Services.AddHttpClient<IUserServiceClient, UserServiceClient>((provider, client) =>
            {
                var config = provider.GetRequiredService<IConfiguration>();
                var baseUrl = config["UserService:BaseUrl"]; 
                client.BaseAddress = new Uri(baseUrl);
                client.DefaultRequestHeaders.Add("X-Internal-Call", "true");
            });
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<IHttpContextHelper, HttpContextHelper>();


            var app = builder.Build();

            // ✅ Swagger only in development
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // ⚙️ Middleware
            app.UseAuthorization();
            app.MapControllers();

            try
            {
                app.Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ Application failed to start: {ex.Message}");
                throw;
            }
        }
    }
}
