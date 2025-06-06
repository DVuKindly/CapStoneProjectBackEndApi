using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UserService.API.DTOs.Requests;
using UserService.API.Services.Interfaces;

namespace UserService.API.Controllers
{
    [Route("api/user/pending-membership")]
    [ApiController]
    public class PendingMembershipController : ControllerBase
    {
        private readonly IPendingMembershipService _pendingMembershipService;

        public PendingMembershipController(IPendingMembershipService pendingMembershipService)
        {
            _pendingMembershipService = pendingMembershipService;
        }

        // 1. USER gửi yêu cầu chọn gói → tạo pending request
        [HttpPost("submit-request")]
        [Authorize(Roles = "User,guest")]
        public async Task<IActionResult> SubmitMembershipRequest([FromBody] CreatePendingMembershipRequest request)
        {
            var success = await _pendingMembershipService.CreateRequestAsync(request);
            if (!success)
                return BadRequest(new { success = false, message = "Failed to create pending request." });

            return Ok(new { success = true, message = "Membership request submitted successfully." });
        }

        // 2. STAFF xem tất cả hồ sơ pending
        [HttpGet("admin/all-requests")]
        [Authorize(Roles = "staff_onboarding")]
        public async Task<IActionResult> GetAllPendingRequests()
        {
            var result = await _pendingMembershipService.GetAllPendingAsync();
            return Ok(result);
        }

        // 3. STAFF xem hồ sơ pending theo LOCATION
        [HttpGet("admin/location/{location}")]
        [Authorize(Roles = "staff_onboarding")]
        public async Task<IActionResult> GetPendingRequestsByLocation(string location)
        {
            var result = await _pendingMembershipService.GetPendingByLocationAsync(location);
            return Ok(result);
        }

        // 4. STAFF DUYỆT hồ sơ
        [HttpPost("admin/approve")]
        [Authorize(Roles = "staff_onboarding")]
        public async Task<IActionResult> ApproveRequest([FromBody] ApprovePendingMembershipRequest request)
        {
            var success = await _pendingMembershipService.ApproveAsync(request);
            if (!success)
                return NotFound(new { success = false, message = "Request not found or already approved." });

            return Ok(new { success = true, message = "Request approved successfully." });
        }

        // 5. STAFF TỪ CHỐI hồ sơ
        [HttpPost("admin/reject")]
        [Authorize(Roles = "staff_onboarding")]
        public async Task<IActionResult> RejectRequest([FromBody] RejectPendingMembershipRequest request)
        {
            var success = await _pendingMembershipService.RejectAsync(request);
            if (!success)
                return NotFound(new { success = false, message = "Request not found or already rejected." });

            return Ok(new { success = true, message = "Request rejected successfully." });
        }

        // 6. USER XÁC NHẬN THANH TOÁN
        [HttpPost("confirm-payment/{accountId}")]
        [Authorize(Roles = "user,member")]
        public async Task<IActionResult> ConfirmMembershipPayment(Guid accountId)
        {
            var success = await _pendingMembershipService.ConfirmPaymentAsync(accountId);
            if (!success)
                return NotFound(new { success = false, message = "Request not found or already paid." });

            return Ok(new { success = true, message = "Payment confirmed successfully." });
        }

        // 7. (TÙY CHỌN) USER/STAF xem trạng thái theo accountId
        [HttpGet("status/{accountId}")]
        [Authorize(Roles = "user,member,guest,staff_onboarding")]
        public async Task<IActionResult> GetRequestStatus(Guid accountId)
        {
            var result = await _pendingMembershipService.GetByAccountIdAsync(accountId);
            if (result == null)
                return NotFound(new { success = false, message = "No pending request found for this account." });

            return Ok(result);
        }
    }
}
