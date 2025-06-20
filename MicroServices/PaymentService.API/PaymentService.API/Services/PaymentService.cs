using PaymentService.API.Data;
using PaymentService.API.DTOs.Requests;
using PaymentService.API.DTOs.Response;
using PaymentService.API.Entities;
using System.Net.Http.Json;


using SharedDto = SharedKernel.DTOsChung.Request.MembershipRequestSummaryDto;

namespace PaymentService.API.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly PaymentDbContext _db;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public PaymentService(PaymentDbContext db, HttpClient httpClient, IConfiguration config)
        {
            _db = db;
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<BaseResponse> CreatePaymentRequestAsync(Guid accountId, CreatePaymentRequestDto dto)
        {
            if (dto.MembershipRequestId == Guid.Empty)
                return new BaseResponse { Success = false, Message = "MembershipRequestId không hợp lệ." };

            // 1. Gọi sang UserService để lấy thông tin yêu cầu thành viên
            var userServiceBaseUrl = _config["UserService:BaseUrl"];
            var summary = await _httpClient.GetFromJsonAsync<SharedDto>(
                $"{userServiceBaseUrl}/api/membership/payment-summary/{dto.MembershipRequestId}");

            if (summary == null || summary.Status != "PendingPayment")
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Không thể tạo thanh toán vì request không hợp lệ hoặc đã thanh toán."
                };
            }

            // 2. Tạo bản ghi PaymentRequest
            var paymentRequest = new PaymentRequest
            {
                AccountId = summary.AccountId,
                MembershipRequestId = summary.MembershipRequestId,
                Amount = summary.Amount,
                PaymentMethod = dto.PaymentMethod,
                ReturnUrl = dto.ReturnUrl,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow.AddMinutes(30)
            };

            _db.PaymentRequests.Add(paymentRequest);
            await _db.SaveChangesAsync();

            // 3. Tạo đường dẫn redirect giả lập cho payment gateway
            var fakeRedirectUrl = $"{dto.PaymentMethod}-pay.com/pay?requestCode={paymentRequest.RequestCode}";

            return new BaseResponse
            {
                Success = true,
                Message = "Khởi tạo yêu cầu thanh toán thành công.",
                Data = new
                {
                    paymentRequestId = paymentRequest.Id,
                    redirectUrl = fakeRedirectUrl
                }
            };
        }
    }
}
