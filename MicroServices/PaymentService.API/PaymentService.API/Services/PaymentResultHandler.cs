namespace PaymentService.API.Services
{
    public class PaymentResultHandler : IPaymentResultHandler
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public PaymentResultHandler(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task HandleSuccessfulPaymentAsync(Guid membershipRequestId)
        {
            var userServiceUrl = _config["UserService:BaseUrl"]; // ex: http://localhost:5005

            var response = await _httpClient.PostAsync($"{userServiceUrl}/api/membership/mark-paid/{membershipRequestId}", null);

            if (!response.IsSuccessStatusCode)
            {
                // log hoặc retry
                Console.WriteLine($"❌ Call về UserService thất bại: {response.StatusCode}");
            }
        }
    }

}
