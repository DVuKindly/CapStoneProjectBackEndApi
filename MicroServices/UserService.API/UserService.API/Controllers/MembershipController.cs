using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserService.API.Data;
using UserService.API.DTOs.Requests;
using UserService.API.Services.Implementations;
using UserService.API.Services.Interfaces;

[ApiController]
[Route("api/user/memberships")]

public class MembershipController : ControllerBase
{
    private readonly IMembershipRequestService _membershipRequestService;
    private readonly IMembershipService _membershipService;
    private readonly UserDbContext _db;

    public MembershipController(IMembershipRequestService membershipRequestService, IMembershipService membershipService, UserDbContext db )
    {
        _membershipRequestService = membershipRequestService;
        _membershipService = membershipService;
        _db = db;
    }

    [HttpPost("requestMember")]
    [Authorize]
    public async Task<IActionResult> SubmitMembershipRequest([FromBody] MembershipRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var accountId = GetAccountIdFromToken();
        var result = await _membershipRequestService.SubmitRequestAsync(accountId, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }


    [HttpGet("pending-requests")]
    [Authorize(Roles = "staff_onboarding")]
    public async Task<IActionResult> GetPendingRequestsForStaff()
    {
        var staffId = GetAccountIdFromToken();
        var result = await _membershipRequestService.GetPendingRequestsForStaffAsync(staffId);
        return Ok(result);
    }

    [HttpPost("approveRequest")]
    [Authorize(Roles = "staff_onboarding")]
    public async Task<IActionResult> ApproveMembershipRequest([FromBody] ApproveMembershipRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var staffId = GetAccountIdFromToken();
        var result = await _membershipRequestService.ApproveMembershipRequestAsync(staffId, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }

    [HttpPost("rejectRequest")]
    [Authorize(Roles = "staff_onboarding")]
    public async Task<IActionResult> RejectMembershipRequest([FromBody] RejectMembershipRequestDto dto)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var staffId = GetAccountIdFromToken();
        var result = await _membershipRequestService.RejectMembershipRequestAsync(staffId, dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }


    [HttpGet("request-detail/{id}")]
    [Authorize(Roles = "staff_onboarding")]
    public async Task<IActionResult> GetRequestDetail(Guid id)
    {
        var staffId = GetAccountId();
        var result = await _membershipRequestService.GetRequestDetailAsync(id, staffId);
        if (result == null)
            return NotFound();
        return Ok(result);
    }

    [HttpGet("history")]
    [Authorize]
    public async Task<IActionResult> GetMyRequestHistory()
    {
        var accountId = GetAccountId();
        var result = await _membershipRequestService.GetRequestHistoryAsync(accountId);
        return Ok(result);
    }

    [HttpGet("payment-summary/{requestId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetMembershipRequestSummary(Guid requestId)
    {
        try
        {
            var result = await _membershipRequestService.GetMembershipRequestSummaryAsync(requestId);
            if (result == null)
                return NotFound("Không tìm thấy request hợp lệ hoặc không ở trạng thái chờ thanh toán.");

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            // Log lỗi nếu cần
            Console.WriteLine("❌ Lỗi GetMembershipRequestSummary: " + ex.Message);
            return StatusCode(500, new { message = "Đã xảy ra lỗi nội bộ khi xử lý yêu cầu thanh toán." });
        }
    }






    [HttpPost("mark-paid")]
    [AllowAnonymous]
    public async Task<IActionResult> MarkMembershipRequestAsPaid([FromBody] MarkPaidRequestDto dto)
    {
        var result = await _membershipRequestService.MarkRequestAsPaidAndApprovedAsync(dto);
        return result.Success ? Ok(result) : BadRequest(result);
    }


    [HttpPost("mark-paid-membership")]
    public async Task<IActionResult> MarkMembershipAsPaid([FromBody] MarkPaidRequestDto dto)
    {
        var result = await _membershipService.MarkMembershipAsPaidAsync(dto);
        return result ? Ok("✅ Cập nhật thanh toán thành công.") : BadRequest("❌ Không tìm thấy bản ghi Membership.");
    }
    [HttpGet("summary-direct/{membershipId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetDirectMembershipSummary(Guid membershipId)
    {
        try
        {
            var result = await _membershipService.GetMembershipSummaryAsync(membershipId);
            if (result == null)
                return NotFound("Không tìm thấy Membership hoặc không ở trạng thái chờ thanh toán.");

            return Ok(result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            Console.WriteLine("❌ Lỗi GetDirectMembershipSummary: " + ex.Message);
            return StatusCode(500, new { message = "Đã xảy ra lỗi nội bộ khi xử lý yêu cầu thanh toán." });
        }
    }

    [HttpGet("my-packages")]
    [Authorize]
    public async Task<IActionResult> GetMyPurchasedMemberships()
    {
        var accountId = GetAccountIdFromToken();
        var result = await _membershipService.GetUserMembershipsAsync(accountId);
        return Ok(result);
    }


    private Guid GetAccountId()
    {
        var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(idClaim, out var guid) ? guid : Guid.Empty;
    }

    private Guid GetAccountIdFromToken()
    {
        var claim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.TryParse(claim, out var id) ? id : Guid.Empty;
    }
}
