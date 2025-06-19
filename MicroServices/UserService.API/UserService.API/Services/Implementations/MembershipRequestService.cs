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
    private readonly IMembershipServiceClient _membershipServiceClient;

    public MembershipRequestService(UserDbContext db , IAuthServiceClient authServiceClient, IMembershipServiceClient membershipServiceClient)
    {
        _membershipServiceClient = membershipServiceClient;
        _db = db;
        _authServiceClient = authServiceClient;
    }

    public async Task<BaseResponse> ApproveMembershipRequestAsync(Guid staffAccountId, ApproveMembershipRequestDto dto)
    {
        // 1. Lấy LocationRegionId của nhân viên
        var staffRegionId = await _db.StaffProfiles
            .Where(s => s.AccountId == staffAccountId)
            .Select(s => s.LocationId)
            .FirstOrDefaultAsync();

        if (staffRegionId == null)
        {
            return new BaseResponse
            {
                Success = false,
                Message = "Không tìm thấy khu vực của nhân viên."
            };
        }

        // 2. Lấy danh sách MembershipLocationId được ánh xạ từ LocationRegionId
        var mappedMembershipLocationIds = await _db.LocationMappings
            .Where(m => m.LocationRegionId == staffRegionId)
            .Select(m => m.MembershipLocationId)
            .ToListAsync();

        if (!mappedMembershipLocationIds.Any())
        {
            return new BaseResponse
            {
                Success = false,
                Message = "Nhân viên không được phân quyền duyệt yêu cầu khu vực nào."
            };
        }

        // 3. Tìm yêu cầu hợp lệ trong danh sách ánh xạ
        var request = await _db.PendingMembershipRequests
            .Include(r => r.UserProfile)
            .FirstOrDefaultAsync(r =>
                r.Id == dto.RequestId &&
                r.LocationId.HasValue &&
                mappedMembershipLocationIds.Contains(r.LocationId.Value));

        if (request == null)
        {
            return new BaseResponse
            {
                Success = false,
                Message = "Không tìm thấy yêu cầu hoặc bạn không có quyền duyệt yêu cầu này."
            };
        }

        // 4. Kiểm tra trạng thái yêu cầu
        if (request.Status != "Pending")
        {
            return new BaseResponse
            {
                Success = false,
                Message = "Yêu cầu này đã được xử lý."
            };
        }

        // 5. Cập nhật trạng thái
        request.Status = "PendingPayment"; // hoặc "Approved" nếu bỏ qua thanh toán
        request.ApprovedAt = DateTime.UtcNow;
        request.StaffNote = dto.StaffNote;

        // 6. Đồng bộ trạng thái hồ sơ user
        var user = request.UserProfile!;
        user.OnboardingStatus = "PendingPayment"; // hoặc "Approved"
        if (request.LocationId.HasValue)
        {
            var regionId = await _db.LocationMappings
                .Where(m => m.MembershipLocationId == request.LocationId.Value)
                .Select(m => m.LocationRegionId)
                .FirstOrDefaultAsync();

            if (regionId != Guid.Empty)
            {
                user.LocationId = regionId;
            }
        }

        await _db.SaveChangesAsync();

        return new BaseResponse
        {
            Success = true,
            Message = "Duyệt yêu cầu thành công."
        };
    }



    public async Task<BaseResponse> RejectMembershipRequestAsync(Guid staffAccountId, RejectMembershipRequestDto dto)
    {
        // 1. Lấy LocationRegionId của staff
        var staffRegionId = await _db.StaffProfiles
            .Where(s => s.AccountId == staffAccountId)
            .Select(s => s.LocationId)
            .FirstOrDefaultAsync();

        if (staffRegionId == Guid.Empty)
            return new BaseResponse { Success = false, Message = "Không tìm thấy thông tin khu vực của nhân viên." };

        // 2. Lấy danh sách MembershipLocationId mà staff khu vực đó có quyền duyệt
        var mappedMembershipLocationIds = await _db.LocationMappings
            .Where(m => m.LocationRegionId == staffRegionId)
            .Select(m => m.MembershipLocationId)
            .ToListAsync();

        if (mappedMembershipLocationIds == null || !mappedMembershipLocationIds.Any())
            return new BaseResponse { Success = false, Message = "Bạn không được phân quyền duyệt bất kỳ khu vực nào." };

        // 3. Lấy request phù hợp với quyền khu vực
        var request = await _db.PendingMembershipRequests
            .Include(r => r.UserProfile)
            .FirstOrDefaultAsync(r =>
                r.Id == dto.RequestId &&
                r.Status == "Pending" &&
                r.LocationId.HasValue &&
                mappedMembershipLocationIds.Contains(r.LocationId.Value));

        if (request == null)
            return new BaseResponse { Success = false, Message = "Không tìm thấy yêu cầu hợp lệ trong khu vực của bạn." };

        // 4. Cập nhật trạng thái từ chối
        request.Status = "Rejected";
        request.StaffNote = dto.Reason;
        request.ApprovedAt = DateTime.UtcNow;

        // 5. Cập nhật trạng thái người dùng
        if (request.UserProfile != null)
        {
            request.UserProfile.OnboardingStatus = "Rejected";
        }

        await _db.SaveChangesAsync();

        return new BaseResponse
        {
            Success = true,
            Message = "Yêu cầu đã bị từ chối và trạng thái người dùng được cập nhật."
        };
    }




    public async Task<List<PendingMembershipRequestDto>> GetPendingRequestsForStaffAsync(Guid staffAccountId)
    {
        // 1. Lấy LocationRegionId của Staff đang đăng nhập
        var staffRegionId = await _db.StaffProfiles
            .Where(s => s.AccountId == staffAccountId)
            .Select(s => s.LocationId)
            .FirstOrDefaultAsync();

        if (staffRegionId == null)
            return new List<PendingMembershipRequestDto>();

        // 2. Lấy danh sách MembershipLocationId được ánh xạ từ LocationRegionId
        var mappedMembershipLocationIds = await _db.LocationMappings
            .Where(m => m.LocationRegionId == staffRegionId)
            .Select(m => m.MembershipLocationId)
            .ToListAsync();

        if (mappedMembershipLocationIds == null || !mappedMembershipLocationIds.Any())
            return new List<PendingMembershipRequestDto>();

        // 3. Lấy danh sách request ở các MembershipLocationId đó
        var pendingRequests = await _db.PendingMembershipRequests
            .Where(r => mappedMembershipLocationIds.Contains(r.LocationId!.Value) && r.Status == "Pending")
            .Include(r => r.UserProfile)
            .ToListAsync();

        if (!pendingRequests.Any())
            return new List<PendingMembershipRequestDto>();

        // 4. Lấy tất cả PackageId duy nhất để gọi MembershipService
        var packageIds = pendingRequests
            .Where(r => r.PackageId.HasValue)
            .Select(r => r.PackageId!.Value)
            .Distinct()
            .ToList();

        var plans = await _membershipServiceClient.GetBasicPlansByIdsAsync(packageIds);
        var planDict = plans.ToDictionary(p => p.Id, p => p);

        // 5. Mapping sang DTO
        var result = pendingRequests.Select(r =>
        {
            var user = r.UserProfile;
            planDict.TryGetValue(r.PackageId ?? Guid.Empty, out var plan);

            return new PendingMembershipRequestDto
            {
                RequestId = r.Id,
                FullName = user?.FullName,
                RequestedPackageName = r.RequestedPackageName,
                MessageToStaff = r.MessageToStaff,
                CreatedAt = r.CreatedAt,
                LocationName = plan?.LocationName, // ✅ từ MembershipService
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
            };
        }).ToList();

        return result;
    }






    public async Task<BaseResponse> SubmitRequestAsync(Guid accountId, MembershipRequestDto dto)
    {
        // 1. Lấy user profile từ DB
        var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.AccountId == accountId);
        if (user == null)
        {
            return new BaseResponse
            {
                Success = false,
                Message = "Không tìm thấy hồ sơ người dùng."
            };
        }

        // 2. Kiểm tra hồ sơ cá nhân đã hoàn thiện chưa
        if (!IsUserProfileCompleted(user))
        {
            return new BaseResponse
            {
                Success = false,
                Message = "Vui lòng hoàn tất hồ sơ cá nhân trước khi gửi yêu cầu. Để đội ngũ chúng tôi nhanh chóng có được thông tin của bạn để hỗ trợ."
            };
        }

        // 3. Gọi MembershipService.API để lấy thông tin gói
        var plan = await _membershipServiceClient.GetBasicPlanByIdAsync(dto.PackageId);
        if (plan == null)
        {
            return new BaseResponse
            {
                Success = false,
                Message = "Không tìm thấy gói bạn đã chọn hoặc gói không hợp lệ."
            };
        }

        // 4. Tạo yêu cầu membership mới với thông tin snapshot từ MembershipService
        var request = new PendingMembershipRequest
        {
            Id = Guid.NewGuid(),
            AccountId = accountId,
            PackageId = dto.PackageId,
            RequestedPackageName = plan.LocationName,
            Amount = plan.Price,
            LocationId = plan.LocationId,
            Interests = user.Interests,
            PersonalityTraits = user.PersonalityTraits,
            Introduction = user.Introduction,
            CvUrl = user.CvUrl,
            MessageToStaff = dto.MessageToStaff,
            Status = "Pending",
            CreatedAt = DateTime.UtcNow
        };

        // 5. Lưu vào DB
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
        // 1. Lấy LocationRegionId của Staff (UserService)
        var staffRegionId = await _db.StaffProfiles
            .Where(s => s.AccountId == staffAccountId)
            .Select(s => s.LocationId)
            .FirstOrDefaultAsync();

        if (staffRegionId == null) return null;

        // 2. Lấy tất cả MembershipLocationId ánh xạ với LocationRegionId
        var mappedMembershipLocationIds = await _db.LocationMappings
            .Where(m => m.LocationRegionId == staffRegionId)
            .Select(m => m.MembershipLocationId)
            .ToListAsync();

        if (!mappedMembershipLocationIds.Any()) return null;

        // 3. Tìm request có requestId và LocationId nằm trong danh sách ánh xạ
        var request = await _db.PendingMembershipRequests
            .Include(r => r.UserProfile)
            .FirstOrDefaultAsync(r => r.Id == requestId && r.LocationId.HasValue && mappedMembershipLocationIds.Contains(r.LocationId.Value));

        if (request == null) return null;

        var user = request.UserProfile;

        // 4. Lấy thông tin gói từ MembershipService để hiển thị tên location
        string? locationName = null;
        if (request.PackageId.HasValue)
        {
            var plan = await _membershipServiceClient.GetBasicPlanByIdAsync(request.PackageId.Value);
            locationName = plan?.LocationName;
        }

        // 5. Mapping sang DTO
        return new PendingMembershipRequestDto
        {
            RequestId = request.Id,
            FullName = user?.FullName,
            RequestedPackageName = request.RequestedPackageName,
            MessageToStaff = request.MessageToStaff,
            CreatedAt = request.CreatedAt,
            LocationName = locationName, // Lấy từ MembershipService
            Interests = request.Interests,
            PersonalityTraits = request.PersonalityTraits,
            Introduction = request.Introduction,
            CvUrl = request.CvUrl,
            ExtendedProfile = new ExtendedProfileDto
            {
                Gender = user?.Gender,
                DOB = user?.DOB,
                AvatarUrl = user?.AvatarUrl,
                SocialLinks = user?.SocialLinks,
                Address = user?.Address,
                RoleType = user?.RoleType
            }
        };
    }




    public async Task<List<PendingMembershipRequestDto>> GetRequestHistoryAsync(Guid accountId)
    {
        // 1. Lấy toàn bộ request của user
        var requests = await _db.PendingMembershipRequests
            .Where(r => r.AccountId == accountId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        // 2. Lấy thông tin user 1 lần
        var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.AccountId == accountId);

        // 3. Tạo danh sách PackageId để gọi MembershipService 1 lần (nếu có)
        var packageIds = requests
            .Where(r => r.PackageId.HasValue)
            .Select(r => r.PackageId!.Value)
            .Distinct()
            .ToList();

        var plans = await _membershipServiceClient.GetBasicPlansByIdsAsync(packageIds);
        var planDict = plans.ToDictionary(p => p.Id, p => p);

        // 4. Duyệt và mapping kết quả
        var result = requests.Select(r =>
        {
            planDict.TryGetValue(r.PackageId ?? Guid.Empty, out var plan);

            return new PendingMembershipRequestDto
            {
                RequestId = r.Id,
                FullName = user?.FullName,
                RequestedPackageName = r.RequestedPackageName,
                MessageToStaff = r.MessageToStaff,
                CreatedAt = r.CreatedAt,
                LocationName = plan?.LocationName, // ⬅️ Lấy từ MembershipService
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
            };
        }).ToList();

        return result;
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
