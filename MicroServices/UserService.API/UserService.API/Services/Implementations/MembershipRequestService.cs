using Microsoft.EntityFrameworkCore;
using SharedKernel.DTOsChung.Request;
using UserService.API.Data;
using UserService.API.DTOs.Requests;
using UserService.API.DTOs.Responses;
using UserService.API.Entities;
using UserService.API.Services.Implementations;
using UserService.API.Services.Interfaces;

public class MembershipRequestService : IMembershipRequestService   
{
    private readonly UserDbContext _db;
    private readonly IAuthServiceClient _authServiceClient;

    public MembershipRequestService(UserDbContext db , IAuthServiceClient authServiceClient)
    {
        _db = db;
        _authServiceClient = authServiceClient;
    }

    public async Task<BaseResponse> ApproveMembershipRequestAsync(Guid staffAccountId, ApproveMembershipRequestDto dto)
    {
        var staffLocationId = await _db.StaffProfiles
            .Where(s => s.AccountId == staffAccountId)
            .Select(s => s.LocationId)
            .FirstOrDefaultAsync();

        var request = await _db.PendingMembershipRequests
            .Include(r => r.UserProfile)
            .FirstOrDefaultAsync(r => r.Id == dto.RequestId && r.LocationId == staffLocationId);

        if (request == null)
        {
            return new BaseResponse { Success = false, Message = "Không tìm thấy yêu cầu hoặc bạn không có quyền duyệt yêu cầu này." };
        }

        if (request.Status != "Pending")
        {
            return new BaseResponse { Success = false, Message = "Yêu cầu này đã được xử lý." };
        }

        request.Status = "PendingPayment"; // Hoặc "Approved" nếu không cần chờ thanh toán
        request.ApprovedAt = DateTime.UtcNow;
        request.StaffNote = dto.StaffNote;

        // ✅ Cập nhật UserProfile
        var user = request.UserProfile!;
        user.OnboardingStatus = "PendingPayment"; // Hoặc "Approved"
        user.LocationId = request.LocationId;

        await _db.SaveChangesAsync();

        return new BaseResponse { Success = true, Message = "Duyệt yêu cầu thành công." };
    }


    public async Task<BaseResponse> RejectMembershipRequestAsync(Guid staffAccountId, RejectMembershipRequestDto dto)
    {
        var staff = await _db.StaffProfiles
            .Where(s => s.AccountId == staffAccountId)
            .Select(s => new { s.AccountId, s.LocationId })
            .FirstOrDefaultAsync();

        if (staff == null || staff.LocationId == null)
            return new BaseResponse { Success = false, Message = "Không tìm thấy thông tin nhân viên hoặc khu vực." };

        var request = await _db.PendingMembershipRequests
            .Include(r => r.UserProfile)
            .FirstOrDefaultAsync(r => r.Id == dto.RequestId && r.Status == "Pending");

        if (request == null || request.LocationId != staff.LocationId)
            return new BaseResponse { Success = false, Message = "Yêu cầu không hợp lệ hoặc đã được xử lý." };

        // Cập nhật trạng thái và ghi chú
        request.Status = "Rejected";
        request.StaffNote = dto.Reason;
        request.ApprovedAt = DateTime.UtcNow;

        // Đồng thời cập nhật trạng thái trong hồ sơ người dùng
        if (request.UserProfile != null)
        {
            request.UserProfile.OnboardingStatus = "Rejected";
        }

        await _db.SaveChangesAsync();

        return new BaseResponse { Success = true, Message = "Yêu cầu đã bị từ chối và trạng thái người dùng được cập nhật." };
    }


    public async Task<List<PendingMembershipRequestDto>> GetPendingRequestsForStaffAsync(Guid staffAccountId)
    {
        // Lấy locationId của staff từ bảng StaffProfiles
        var staffLocationId = await _db.StaffProfiles
            .Where(s => s.AccountId == staffAccountId)
            .Select(s => s.LocationId)
            .FirstOrDefaultAsync();

        if (staffLocationId == null)
            return new List<PendingMembershipRequestDto>();

        // Lấy các request có cùng LocationId và đang Pending
        var pendingRequests = await _db.PendingMembershipRequests
            .Where(r => r.LocationId == staffLocationId && r.Status == "Pending")
            .Include(r => r.UserProfile)
            .Include(r => r.LocationRegion)
            .ToListAsync();

        var result = new List<PendingMembershipRequestDto>();

        foreach (var r in pendingRequests)
        {
            var userProfile = r.UserProfile;

            // Khởi tạo ExtendedProfileDto từ UserProfile
            var extendedProfile = new ExtendedProfileDto
            {
                Gender = userProfile?.Gender,
                DOB = userProfile?.DOB,
                AvatarUrl = userProfile?.AvatarUrl,
                SocialLinks = userProfile?.SocialLinks,
                Address = userProfile?.Address,
                RoleType = userProfile?.RoleType
            };

            result.Add(new PendingMembershipRequestDto
            {
                RequestId = r.Id,
                FullName = userProfile?.FullName,
               
                RequestedPackageName = r.RequestedPackageName,
                MessageToStaff = r.MessageToStaff,
                LocationName = r.LocationRegion?.Name,
                CreatedAt = r.CreatedAt,
                Interests = r.Interests,
                PersonalityTraits = r.PersonalityTraits,
                Introduction = r.Introduction,
                CvUrl = r.CvUrl,
                ExtendedProfile = extendedProfile
            });
        }

        return result;
    }




    public async Task<BaseResponse> SubmitRequestAsync(Guid accountId, MembershipRequestDto dto)
    {
        var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.AccountId == accountId);
        if (user == null)
        {
            return new BaseResponse
            {
                Success = false,
                Message = "Không tìm thấy hồ sơ người dùng."
            };
        }

        if (!IsUserProfileCompleted(user))
        {
            return new BaseResponse
            {
                Success = false,
                Message = "Vui lòng hoàn tất hồ sơ cá nhân trước khi gửi yêu cầu. Để đội ngũ chúng tôi nhanh chóng có được thông tin của bạn để hỗ trợ"
            };
        }
       
        var locationExists = await _db.LocationRegions.AnyAsync(r => r.Id == dto.LocationId);
        if (!locationExists)
        {
            return new BaseResponse
            {
                Success = false,
                Message = "Khu vực bạn chọn không hợp lệ."
            };
        }
        var request = new PendingMembershipRequest
        {
            Id = Guid.NewGuid(),
            AccountId = accountId,
            PackageId = dto.PackageId,
            RequestedPackageName = dto.RequestedPackageName,
            LocationId = dto.LocationId,
            Interests = user.Interests,
            PersonalityTraits = user.PersonalityTraits,
            Introduction = user.Introduction,
            CvUrl = user.CvUrl,
            MessageToStaff = dto.MessageToStaff,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        _db.PendingMembershipRequests.Add(request);
        await _db.SaveChangesAsync();

        return new BaseResponse
        {
            Success = true,
            Message = "Gửi yêu cầu thành công. Vui lòng chờ duyệt từ đội ngũ hỗ trợ."
        };
    }

    private bool IsUserProfileCompleted(UserProfile user)
    {
        return !string.IsNullOrWhiteSpace(user.FullName)
            && !string.IsNullOrWhiteSpace(user.Phone)
            && !string.IsNullOrWhiteSpace(user.Gender)
            && user.DOB.HasValue
            && !string.IsNullOrWhiteSpace(user.AvatarUrl)
            && !string.IsNullOrWhiteSpace(user.Interests)
              && !string.IsNullOrWhiteSpace(user.Address)
            && !string.IsNullOrWhiteSpace(user.PersonalityTraits)
            && !string.IsNullOrWhiteSpace(user.Introduction)
            && !string.IsNullOrWhiteSpace(user.CvUrl);
    }

    public async Task<PendingMembershipRequestDto?> GetRequestDetailAsync(Guid requestId, Guid staffAccountId)
    {
        var staffLocationId = await _db.StaffProfiles
            .Where(s => s.AccountId == staffAccountId)
            .Select(s => s.LocationId)
            .FirstOrDefaultAsync();

        var request = await _db.PendingMembershipRequests
            .Include(r => r.UserProfile)
            .Include(r => r.LocationRegion)
            .FirstOrDefaultAsync(r => r.Id == requestId && r.LocationId == staffLocationId);

        if (request == null) return null;

        return new PendingMembershipRequestDto
        {
            RequestId = request.Id,
            FullName = request.UserProfile?.FullName,
           
            RequestedPackageName = request.RequestedPackageName,
            MessageToStaff = request.MessageToStaff,
            CreatedAt = request.CreatedAt,
            LocationName = request.LocationRegion?.Name,
            Interests = request.Interests,
            PersonalityTraits = request.PersonalityTraits,
            Introduction = request.Introduction,
            CvUrl = request.CvUrl,
            ExtendedProfile = new ExtendedProfileDto
            {
                Gender = request.UserProfile?.Gender,
                DOB = request.UserProfile?.DOB,
                AvatarUrl = request.UserProfile?.AvatarUrl,
                SocialLinks = request.UserProfile?.SocialLinks,
                Address = request.UserProfile?.Address,
                RoleType = request.UserProfile?.RoleType
            }
        };
    }

    public async Task<List<PendingMembershipRequestDto>> GetRequestHistoryAsync(Guid accountId)
    {
        var requests = await _db.PendingMembershipRequests
            .Where(r => r.AccountId == accountId)
            .Include(r => r.LocationRegion)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.AccountId == accountId);

        return requests.Select(r => new PendingMembershipRequestDto
        {
            RequestId = r.Id,
            FullName = user?.FullName,
           
            RequestedPackageName = r.RequestedPackageName,
            MessageToStaff = r.MessageToStaff,
            CreatedAt = r.CreatedAt,
            LocationName = r.LocationRegion?.Name,
            Interests = r.Interests,
            PersonalityTraits = r.PersonalityTraits,
            Introduction = r.Introduction,
            CvUrl = r.CvUrl,
            ExtendedProfile = new ExtendedProfileDto
            {
                Gender = user?.Gender,
                DOB = user?.DOB,
                AvatarUrl = user?.AvatarUrl,
                SocialLinks = user?.SocialLinks,
                Address = user?.Address,
                RoleType = user?.RoleType
            }
        }).ToList();
    }
    public async Task<MembershipRequestSummaryDto?> GetMembershipRequestSummaryAsync(Guid requestId)
    {
        var request = await _db.PendingMembershipRequests
            .Include(r => r.UserProfile)
            .FirstOrDefaultAsync(r => r.Id == requestId);

        if (request == null || request.Status != "PendingPayment")
            return null;

        return new MembershipRequestSummaryDto
        {
            MembershipRequestId = request.Id,
            AccountId = request.AccountId,
            PackageId = request.PackageId,
            RequestedPackageName = request.RequestedPackageName,
            Amount = 0, // sẽ gọi từ MembershipPackageService để lấy
            Status = request.Status
        };
    }






    public async Task<BaseResponse> MarkRequestAsPaidAndApprovedAsync(MarkPaidRequestDto dto)
    {
        var request = await _db.PendingMembershipRequests
            .Include(r => r.UserProfile)
            .FirstOrDefaultAsync(r => r.Id == dto.RequestId);

        if (request == null || request.Status != "PendingPayment")
        {
            return new BaseResponse
            {
                Success = false,
                Message = "Yêu cầu không tồn tại hoặc chưa đủ điều kiện cập nhật."
            };
        }

        // Cập nhật trạng thái và thông tin thanh toán
        request.Status = "Approved";
        request.PaymentStatus = "Paid";
        request.PaymentTime = DateTime.UtcNow;
        request.PaymentMethod = dto.PaymentMethod;
        request.PaymentTransactionId = dto.PaymentTransactionId;
        request.PaymentNote = dto.PaymentNote;
        request.PaymentProofUrl = dto.PaymentProofUrl;

        if (request.UserProfile != null)
        {
            request.UserProfile.OnboardingStatus = "Approved";
            request.UserProfile.RoleType = "member";
        }

        await _db.SaveChangesAsync();

        bool promoted = false;
        try
        {
            promoted = await _authServiceClient.PromoteUserToMemberAsync(request.AccountId);
        }
        catch (Exception ex)
        {
            // Bạn nên inject ILogger để log chi tiết lỗi ra để dễ debug
            promoted = false;
        }

        if (!promoted)
        {
            return new BaseResponse
            {
                Success = false,
                Message = "Cập nhật thanh toán OK, nhưng không thể cập nhật vai trò trong AuthService."
            };
        }

        return new BaseResponse
        {
            Success = true,
            Message = "Thanh toán và duyệt yêu cầu thành công, vai trò user đã được cập nhật."
        };
    }







}
