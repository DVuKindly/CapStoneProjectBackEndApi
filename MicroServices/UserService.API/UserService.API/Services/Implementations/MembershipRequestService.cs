using Microsoft.EntityFrameworkCore;
using SharedKernel.DTOsChung.Request;
using System.Numerics;
using UserService.API.Constants;
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
    private readonly IPaymentServiceClient _paymentServiceClient;

    public MembershipRequestService(UserDbContext db , IAuthServiceClient authServiceClient, IMembershipServiceClient membershipServiceClient, IPaymentServiceClient paymentServiceClient)
    {
        _membershipServiceClient = membershipServiceClient;
        _db = db;
        _authServiceClient = authServiceClient;
        _paymentServiceClient = paymentServiceClient;
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
        var packageType = dto.PackageType?.ToLower();
        if (packageType != "basic" && packageType != "combo")
            return BaseResponse.Fail("Loại gói không hợp lệ. Chỉ được chọn 'basic' hoặc 'combo'.");

        var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.AccountId == accountId);
        if (user == null)
            return BaseResponse.Fail("Không tìm thấy hồ sơ người dùng.");

        if (!IsUserProfileCompleted(user))
            return BaseResponse.Fail("Vui lòng hoàn tất hồ sơ cá nhân trước khi gửi yêu cầu.");

        // Kiểm tra tồn tại yêu cầu cũ cho cùng package
        var existingRequest = await _db.PendingMembershipRequests
            .Where(r => r.AccountId == accountId &&
                        r.PackageId == dto.PackageId &&
                        (r.Status == "Pending" || r.Status == "PendingPayment" || r.Status == "Completed"))
            .OrderByDescending(r => r.CreatedAt)
            .FirstOrDefaultAsync();

        if (existingRequest != null)
        {
            if (existingRequest.Status == "Completed" || existingRequest.Status == "Pending")
                return BaseResponse.Fail("Bạn đã gửi hoặc đã mua gói này rồi. Không thể gửi lại yêu cầu.");

            if (existingRequest.Status == "PendingPayment")
            {
                var elapsed = DateTime.UtcNow - existingRequest.CreatedAt;
                if (elapsed.TotalMinutes < 15)
                {
                    return BaseResponse.Fail("Bạn đã tạo yêu cầu mua gói này và chưa thanh toán. Vui lòng hoàn tất thanh toán hoặc đợi 15 phút để gửi lại.");
                }
                else
                {
                    _db.PendingMembershipRequests.Remove(existingRequest);
                    await _db.SaveChangesAsync();
                }
            }
        }

        // Gói BASIC
        if (packageType == "basic")
        {
            var plan = await _membershipServiceClient.GetBasicPlanByIdAsync(dto.PackageId);
            if (plan == null)
                return BaseResponse.Fail("Không tìm thấy gói Basic bạn đã chọn.");

            if (plan.VerifyBuy) // Mua trực tiếp
            {
                var request = new PendingMembershipRequest
                {
                    Id = Guid.NewGuid(),
                    AccountId = accountId,
                    PackageId = plan.Id,
                    RequestedPackageName = plan.Name,
                    Amount = plan.Price,
                    LocationId = plan.LocationId ?? Guid.Empty,
                    Interests = user.Interests,
                    PersonalityTraits = user.PersonalityTraits,
                    Introduction = user.Introduction,
                    CvUrl = user.CvUrl,
                    MessageToStaff = dto.MessageToStaff,
                    Status = "PendingPayment",
                    CreatedAt = DateTime.UtcNow,
                    PackageType = "basic"
                };

                _db.PendingMembershipRequests.Add(request);
                await _db.SaveChangesAsync();

                if (string.IsNullOrWhiteSpace(dto.RedirectUrl))
                    return BaseResponse.Fail("Thiếu RedirectUrl để chuyển hướng sau thanh toán.");

                var paymentDto = new CreatePaymentRequestDto
                {
                    RequestId = request.Id,
                    AccountId = accountId,
                    PackageId = plan.Id,
                    Amount = plan.Price,
                    PackageType = "basic",
                    PaymentMethod = "VNPAY",
                    RedirectUrl = dto.RedirectUrl,
                };

                var paymentResponse = await _paymentServiceClient.CreatePaymentRequestAsync(paymentDto);
                if (!paymentResponse.Success)
                    return BaseResponse.Fail("Tạo thanh toán thất bại: " + paymentResponse.Message);

                return BaseResponse.Ok("Yêu cầu đã được tạo và chuyển sang thanh toán.", new
                {
                    IsDirectPurchase = true,
                    RequestId = request.Id,
                    PaymentUrl = paymentResponse.Data
                });
            }
            else // Cần duyệt thủ công
            {
                var request = new PendingMembershipRequest
                {
                    Id = Guid.NewGuid(),
                    AccountId = accountId,
                    PackageId = plan.Id,
                    RequestedPackageName = plan.Name,
                    Amount = plan.Price,
                    LocationId = plan.LocationId ?? Guid.Empty,
                    Interests = user.Interests,
                    PersonalityTraits = user.PersonalityTraits,
                    Introduction = user.Introduction,
                    CvUrl = user.CvUrl,
                    MessageToStaff = dto.MessageToStaff,
                    Status = "Pending",
                    CreatedAt = DateTime.UtcNow,
                    PackageType = "basic"
                };

                _db.PendingMembershipRequests.Add(request);
                await _db.SaveChangesAsync();

                return BaseResponse.Ok("Yêu cầu của bạn đã được gửi. Vui lòng chờ xét duyệt.", new
                {
                    IsDirectPurchase = false,
                    RequestId = request.Id
                });
            }
        }

        // Gói COMBO
        if (packageType == "combo")
        {
            var combo = await _membershipServiceClient.GetComboPlanByIdAsync(dto.PackageId);
            if (combo == null)
                return BaseResponse.Fail("Không tìm thấy gói Combo bạn đã chọn.");

            var request = new PendingMembershipRequest
            {
                Id = Guid.NewGuid(),
                AccountId = accountId,
                PackageId = combo.Id,
                RequestedPackageName = combo.Name,
                Amount = combo.TotalPrice,
                LocationId = combo.LocationId ?? Guid.Empty,
                Interests = user.Interests,
                PersonalityTraits = user.PersonalityTraits,
                Introduction = user.Introduction,
                CvUrl = user.CvUrl,
                MessageToStaff = dto.MessageToStaff,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
                PackageType = "combo"
            };

            _db.PendingMembershipRequests.Add(request);
            await _db.SaveChangesAsync();

            return BaseResponse.Ok("Yêu cầu mua gói Combo đã được gửi. Vui lòng chờ xét duyệt.", new
            {
                IsDirectPurchase = false,
                RequestId = request.Id
            });
        }

        return BaseResponse.Fail("Xảy ra lỗi không xác định.");
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
        var requests = await _db.PendingMembershipRequests
            .Where(r => r.AccountId == accountId)
            .OrderByDescending(r => r.CreatedAt)
            .ToListAsync();

        var user = await _db.UserProfiles.FirstOrDefaultAsync(u => u.AccountId == accountId);

        var packageIds = requests
            .Where(r => r.PackageId.HasValue)
            .Select(r => r.PackageId!.Value)
            .Distinct()
            .ToList();

        var plans = await _membershipServiceClient.GetBasicPlansByIdsAsync(packageIds);
        var planDict = plans.ToDictionary(p => p.Id, p => p);

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
                LocationName = plan?.LocationName,
                Interests = r.Interests,
                PersonalityTraits = r.PersonalityTraits,
                Introduction = r.Introduction,
                CvUrl = r.CvUrl,
                PackageType = r.PackageType,
                Status = r.Status,
                Amount = (decimal)r.Amount!,

                PaymentStatus = r.PaymentStatus,
                PaymentMethod = r.PaymentMethod,
                PaymentTime = r.PaymentTime,
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
        // 1️⃣ Ưu tiên tìm trong bảng PendingMembershipRequests
        var request = await _db.PendingMembershipRequests
            .AsNoTracking()
            .FirstOrDefaultAsync(r => r.Id == requestId);

        if (request != null)
        {
            if (request.Status != "PendingPayment")
                return null;

            // ✅ Validate loại gói
            var planType = request.PackageType?.ToLowerInvariant();
            if (string.IsNullOrWhiteSpace(planType) || (planType != "basic" && planType != "combo"))
                throw new ArgumentException($"Loại gói không hợp lệ: {request.PackageType}");

            // ✅ Gọi MembershipService để lấy giá gói từ hệ thống gốc
            decimal amount = await _membershipServiceClient.GetPlanPriceAsync(request.PackageId!.Value, planType);

            return new MembershipRequestSummaryDto
            {
                MembershipRequestId = request.Id,
                AccountId = request.AccountId,
                PackageId = request.PackageId,
                RequestedPackageName = request.RequestedPackageName,
                Amount = amount,
                Status = request.Status
            };
        }

        // 2️⃣ Nếu không có request, thử tra trong bảng Memberships (trường hợp thanh toán trực tiếp)
        var membership = await _db.Memberships
            .AsNoTracking()
            .FirstOrDefaultAsync(m => m.Id == requestId);

        if (membership != null)
        {
            return new MembershipRequestSummaryDto
            {
                MembershipRequestId = membership.Id,
                AccountId = membership.AccountId,
                PackageId = membership.PackageId,
                RequestedPackageName = membership.PackageName,
                Amount = membership.Amount,
                Status = "PendingPayment" // gán mặc định để Payment xử lý
            };
        }

        // 3️⃣ Không tìm thấy hợp lệ
        return null;
    }











    public async Task<BaseResponse> MarkRequestAsPaidAndApprovedAsync(MarkPaidRequestDto dto)
    {
        if (dto == null || dto.RequestId == Guid.Empty)
            return BaseResponse.Fail("Dữ liệu không hợp lệ.");

        var request = await _db.PendingMembershipRequests
            .Include(r => r.UserProfile)
            .FirstOrDefaultAsync(r => r.Id == dto.RequestId);

        if (request == null)
            return BaseResponse.Fail("Không tìm thấy yêu cầu đăng ký.");

        if (request.Status == "Completed")
            return BaseResponse.Fail("Yêu cầu đã được xử lý trước đó.");

        // Cập nhật thông tin thanh toán
        request.PaymentStatus = "Paid";
        request.PaymentMethod = dto.PaymentMethod ?? "Unknown";
        request.PaymentTransactionId = dto.PaymentTransactionId;
        request.PaymentTime = DateTime.UtcNow;
        request.PaymentNote = dto.PaymentNote;
        request.Status = "Completed";

        // Cập nhật trạng thái onboarding bất kể trạng thái cũ là gì, chuyển sang "ApprovedMember"
        if (request.UserProfile != null)
        {
            request.UserProfile.OnboardingStatus = "ApprovedMember";

            // Kiểm tra role hiện tại, nếu chưa phải member thì nâng role
            if (request.UserProfile.RoleType != "member")
            {
                request.UserProfile.RoleType = "member";

                try
                {
                    var promoted = await _authServiceClient.PromoteUserToMemberAsync(request.AccountId);
                    if (!promoted)
                    {
                        // Log cảnh báo, không fail request
                        Console.WriteLine("⚠️ Đã xác nhận thanh toán. Tuy nhiên không thể nâng vai trò. Vui lòng liên hệ hỗ trợ.");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"🚨 Lỗi khi nâng vai trò: {ex.Message}");
                }
            }
            else
            {
                // Nếu user đã là member rồi thì không nâng nữa
                Console.WriteLine("✅ User đã là member, bỏ qua nâng role.");
            }
        }

        var membership = new Membership
        {
            Id = Guid.NewGuid(),
            AccountId = request.AccountId,
            PackageId = request.PackageId ?? Guid.Empty,
            PackageName = request.RequestedPackageName ?? "Gói không tên",
            PackageType = request.PackageType?.ToLower() ?? "basic",
            Amount = request.Amount ?? 0,
            LocationId = request.LocationId ?? Guid.Empty,
            PurchasedAt = DateTime.UtcNow,
            UsedForRoleUpgrade = request.PackageType?.ToLower() == "combo",

            // Thông tin thanh toán
            PaymentMethod = request.PaymentMethod,
            PaymentStatus = request.PaymentStatus,
            PaymentNote = request.PaymentNote,
            PaymentTime = request.PaymentTime,
            PaymentTransactionId = request.PaymentTransactionId,
        };

        // Lấy thông tin thời hạn từ gói (basic hoặc combo)
        if (membership.PackageType == "basic")
        {
            var basicPlan = await _membershipServiceClient.GetBasicPlanByIdAsync(membership.PackageId);
            if (basicPlan != null)
            {
                membership.PackageDurationValue = basicPlan.PackageDurationValue;
                membership.PackageDurationUnit = basicPlan.PackageDurationUnit;
            }
        }
        else if (membership.PackageType == "combo")
        {
            var comboPlan = await _membershipServiceClient.GetComboPlanByIdAsync(membership.PackageId);
            if (comboPlan != null)
            {
                membership.PackageDurationValue = comboPlan.PackageDurationValue;
                membership.PackageDurationUnit = comboPlan.PackageDurationUnit;
            }
        }

        // Tính ngày hết hạn
        if (membership.PackageDurationValue.HasValue && !string.IsNullOrEmpty(membership.PackageDurationUnit))
        {
            membership.ExpireAt = CalculateExpireDate(membership.PurchasedAt,
                membership.PackageDurationValue.Value,
                membership.PackageDurationUnit);
        }

        _db.Memberships.Add(membership);

        // Lưu các thay đổi
        await _db.SaveChangesAsync();

        return BaseResponse.Ok("Đã xác nhận thanh toán và cập nhật thành công.", new
        {
            MembershipId = membership.Id,
            PackageName = membership.PackageName,
            PackageType = membership.PackageType,
            Upgraded = membership.UsedForRoleUpgrade
        });
    }



    private DateTime CalculateExpireDate(DateTime start, int value, string unit)
    {
        return unit.ToLower() switch
        {
            "day" => start.AddDays(value),
            "month" => start.AddMonths(value),
            "year" => start.AddYears(value),
            _ => start
        };
    }

}
