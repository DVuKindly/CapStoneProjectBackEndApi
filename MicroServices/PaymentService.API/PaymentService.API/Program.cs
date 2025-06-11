using Microsoft.EntityFrameworkCore;
using PaymentService.API.Data;
using PaymentService.API.Services;
using Pay = PaymentService.API.Services.PaymentService;

namespace PaymentService.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add DBContext
            builder.Services.AddDbContext<PaymentDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("Default"),
                    sql => sql.EnableRetryOnFailure()
                ));

            // ✅ Cấu hình HttpClient để gọi sang UserService
            builder.Services.AddHttpClient<IPaymentService, Pay>(client =>
            {
                var baseUrl = builder.Configuration["UserService:BaseUrl"];
                client.BaseAddress = new Uri(baseUrl!); // ! để tránh warning null
            });

            builder.Services.AddHttpClient<IPaymentResultHandler, PaymentResultHandler>();

            var app = builder.Build();

            // Configure middleware
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();

            app.MapControllers();

            app.Run();
        }
    }
}
