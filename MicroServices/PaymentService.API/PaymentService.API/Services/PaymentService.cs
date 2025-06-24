using PaymentService.API.Data;
using PaymentService.API.DTOs.Requests;
using PaymentService.API.DTOs.Response;
using PaymentService.API.Entities;
using PaymentService.API.Helpers;
using System.Globalization;
using System.Net.Http.Json;
using SharedDto = SharedKernel.DTOsChung.Request.MembershipRequestSummaryDto;

namespace PaymentService.API.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly PaymentDbContext _db;
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly IHttpContextHelper _httpContextHelper;

        public PaymentService(
            PaymentDbContext db,
            HttpClient httpClient,
            IConfiguration config,
            IHttpContextHelper httpContextHelper)
        {
            _db = db;
            _httpClient = httpClient;
            _config = config;
            _httpContextHelper = httpContextHelper;
        }

        public async Task<BaseResponse> CreatePaymentRequestAsync(Guid accountId, CreatePaymentRequestDto dto)
        {
            if (dto.RequestId == Guid.Empty)
                return new BaseResponse { Success = false, Message = "RequestId không hợp lệ." };

            var userServiceBaseUrl = _config["UserService:BaseUrl"];
            SharedDto? summary = null;

            try
            {
                if (dto.IsDirectMembership)
                {
                    var summaryUrl = $"{userServiceBaseUrl}/api/user/memberships/summary-direct/{dto.RequestId}";
                    summary = await _httpClient.GetFromJsonAsync<SharedDto>(summaryUrl);
                }
                else
                {
                    var summaryUrl = $"{userServiceBaseUrl}/api/user/memberships/payment-summary/{dto.RequestId}";
                    summary = await _httpClient.GetFromJsonAsync<SharedDto>(summaryUrl);
                }
            }
            catch (Exception ex)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = $"Lỗi khi gọi UserService: {ex.Message}"
                };
            }

            if (summary == null || summary.Status != "PendingPayment")
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Không thể tạo thanh toán vì yêu cầu không hợp lệ hoặc đã thanh toán."
                };
            }

            var amountVnd = (long)(summary.Amount * 100);
            if (amountVnd <= 0)
            {
                return new BaseResponse
                {
                    Success = false,
                    Message = "Số tiền thanh toán không hợp lệ."
                };
            }

            string txnRef = Guid.NewGuid().ToString("N").Substring(0, 20);
            string returnUrl = !string.IsNullOrWhiteSpace(dto.ReturnUrl)
                ? dto.ReturnUrl
                : _config["VNPay:ReturnUrl"] ?? throw new InvalidOperationException("Thiếu cấu hình VNPay:ReturnUrl");

            var paymentRequest = new PaymentRequest
            {
                AccountId = summary.AccountId,
                MembershipRequestId = summary.MembershipRequestId, // Dù là Membership hay Request, vẫn gán vào đây
                Amount = summary.Amount,
                PaymentMethod = dto.PaymentMethod,
                ReturnUrl = returnUrl,
                RequestCode = txnRef,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
                ExpireAt = DateTime.UtcNow.AddMinutes(30)
            };

            _db.PaymentRequests.Add(paymentRequest);
            await _db.SaveChangesAsync();

            // Cấu hình VNPay
            var vnPay = new VnPayLibrary();
            vnPay.AddRequestData("vnp_Version", VnPayLibrary.VERSION);
            vnPay.AddRequestData("vnp_Command", "pay");
            vnPay.AddRequestData("vnp_TmnCode", _config["VNPay:TmnCode"]);
            vnPay.AddRequestData("vnp_Amount", amountVnd.ToString(CultureInfo.InvariantCulture));
            vnPay.AddRequestData("vnp_CurrCode", "VND");
            vnPay.AddRequestData("vnp_TxnRef", paymentRequest.RequestCode);
            vnPay.AddRequestData("vnp_OrderInfo", $"Thanh toán gói {summary.RequestedPackageName}");
            vnPay.AddRequestData("vnp_OrderType", "other");
            vnPay.AddRequestData("vnp_Locale", "vn");
            vnPay.AddRequestData("vnp_CreateDate", DateTime.UtcNow.AddHours(7).ToString("yyyyMMddHHmmss"));
            vnPay.AddRequestData("vnp_IpAddr", _httpContextHelper.GetClientIpAddress() ?? "127.0.0.1");
            vnPay.AddRequestData("vnp_ReturnUrl", returnUrl);

            var ipnUrl = _config["VNPay:IpnUrl"];
            if (!string.IsNullOrWhiteSpace(ipnUrl))
                vnPay.AddRequestData("vnp_IpnUrl", ipnUrl);

            var vnpUrl = _config["VNPay:BaseUrl"];
            var hashSecret = _config["VNPay:HashSecret"];
            bool encodeHash = bool.Parse(_config["VNPay:HashEncodeUrl"] ?? "true");

            string redirectUrl = vnPay.CreateRequestUrl(vnpUrl, hashSecret, encodeHash);

            return new BaseResponse
            {
                Success = true,
                Message = "Khởi tạo yêu cầu thanh toán thành công.",
                Data = new
                {
                    paymentRequestId = paymentRequest.Id,
                    redirectUrl = redirectUrl,
                    returnUrl = returnUrl
                }
            };
        }



    }
}