using AutoMapper;
using MembershipService.API.Common.Constants;
using MembershipService.API.Data;
using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Implementations;
using MembershipService.API.Repositories.Interfaces;
using MembershipService.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace MembershipService.API.Services.Implementations
{
    public class BasicPlanService : IBasicPlanService
    {
        private readonly IBasicPlanRepository _basicPlanRepo;
        private readonly IComboPlanDurationRepository _durationRepo;
        private readonly IBasicPlanRoomRepository _basicPlanRoomRepo;
        private readonly IBasicPlanTypeRepository _basicPlanTypeRepo;
        private readonly IBasicPlanEntitlementRepository _basicPlanEntitlementRepo;
        private readonly IMapper _mapper;
        private readonly MembershipDbContext _context;

        public BasicPlanService(IBasicPlanRepository basicPlanRepo, IComboPlanDurationRepository durationRepo, IBasicPlanRoomRepository basicPlanRoomRepo, IBasicPlanTypeRepository basicPlanTypeRepo, IBasicPlanEntitlementRepository basicPlanEntitlementRepo, IMapper mapper, MembershipDbContext context)
        {
            _basicPlanRepo = basicPlanRepo;
            _durationRepo = durationRepo;
            _basicPlanRoomRepo = basicPlanRoomRepo;
            _basicPlanTypeRepo = basicPlanTypeRepo;
            _basicPlanEntitlementRepo = basicPlanEntitlementRepo;
            _mapper = mapper;
            _context = context;
        }

        public async Task<BasicPlanResponseDto> CreateAsync(CreateBasicPlanRequest request)
        {
            // Lấy BasicPlanType để phân loại theo Code
            var planType = await _basicPlanTypeRepo.GetByIdAsync(request.BasicPlanTypeId);
            if (planType == null)
                throw new InvalidOperationException("Invalid BasicPlanType.");

            var isAccommodation = planType.Code == BasicPlanTypeCodes.Accommodation;

            // Tạo BasicPlan entity
            var basicPlan = _mapper.Map<BasicPlan>(request);
            basicPlan.Code = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();

            var createdPlan = await _basicPlanRepo.AddAsync(basicPlan);
            if (createdPlan == null || createdPlan.Id == Guid.Empty)
                throw new InvalidOperationException("Failed to create BasicPlan.");

            // Xử lý loại Accommodation
            if (isAccommodation)
            {
                if (request.Accomodations == null || !request.Accomodations.Any())
                    throw new InvalidOperationException("Accommodation plan must include at least one room.");

                foreach (var roomOption in request.Accomodations)
                {
                    var newRoom = new BasicPlanRoom
                    {
                        BasicPlanId = createdPlan.Id,
                        AccommodationOptionId = roomOption.AccomodationId
                    };

                    var createdRoomOption = await _basicPlanRoomRepo.AddAsync(newRoom);
                    if (createdRoomOption == null)
                        throw new InvalidOperationException("Failed to add room to BasicPlan.");
                }

                if (request.Entitlements != null && request.Entitlements.Any())
                    throw new InvalidOperationException("Accommodation plan cannot include entitlements.");
            }
            else // Loại dịch vụ hằng ngày
            {
                if (request.Entitlements == null || !request.Entitlements.Any())
                    throw new InvalidOperationException("Entitlement-based plan must include at least one entitlement.");

                foreach (var entitlement in request.Entitlements)
                {
                    var newEntitlement = new BasicPlanEntitlement
                    {
                        BasicPlanId = createdPlan.Id,
                        EntitlementRuleId = entitlement.EntitlementRuleId,
                        Quantity = entitlement.Quantity
                    };

                    var createdEnt = await _basicPlanEntitlementRepo.AddAsync(newEntitlement);
                    if (createdEnt == null)
                        throw new InvalidOperationException("Failed to add entitlement to BasicPlan.");
                }

                if (request.Accomodations != null && request.Accomodations.Any())
                    throw new InvalidOperationException("Entitlement-based plan cannot include room options.");
            }

            // Xử lý Duration (áp dụng chung)
            if (request.PackageDurations != null && request.PackageDurations.Any())
            {
                var durations = request.PackageDurations
                    .Select(dto =>
                    {
                        var duration = _mapper.Map<ComboPlanDuration>(dto);
                        duration.BasicPlanId = createdPlan.Id;
                        return duration;
                    })
                    .ToList();

                await _durationRepo.AddRangeAsync(durations);
            }

            return _mapper.Map<BasicPlanResponseDto>(createdPlan);
        }

        public async Task<BasicPlanResponseDto> UpdateAsync(Guid id, UpdateBasicPlanRequest request)
        {
            var existingPlan = await _basicPlanRepo.GetByIdAsync(id);
            if (existingPlan == null) throw new Exception("BasicPlan not found");

            existingPlan.Name = request.Name;
            existingPlan.Description = request.Description;
            existingPlan.VerifyBuy = request.VerifyBuy;
            existingPlan.BasicPlanCategoryId = request.BasicPlanCategoryId;
            existingPlan.PlanLevelId = request.PlanLevelId;
            existingPlan.TargetAudienceId = request.TargetAudienceId;

            var updated = await _basicPlanRepo.UpdateAsync(existingPlan);
            return _mapper.Map<BasicPlanResponseDto>(updated);
        }

        public async Task<BasicPlanResponseDto> GetByIdAsync(Guid id)
        {
            var basicPlan = await _basicPlanRepo.GetByIdAsync(id);
            if (basicPlan == null)
                throw new KeyNotFoundException($"Không tìm thấy BasicPlan với Id = {id}");

            return _mapper.Map<BasicPlanResponseDto>(basicPlan);
        }

        public async Task<List<BasicPlanResponseDto>> GetByTypeIdAsync(Guid typeId)
        {
            return await _basicPlanRepo.GetByTypeIdAsync(typeId);
        }

        public async Task<List<BasicPlanResponseDto>> GetAllAsync()
        {
            var plans = await _basicPlanRepo.GetAllAsync();
            return _mapper.Map<List<BasicPlanResponseDto>>(plans);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _basicPlanRepo.DeleteAsync(id);
        }

        public async Task<List<BasicPlanResponseDto>> GetByIdsAsync(List<Guid> ids)
        {
            if (ids == null || ids.Count == 0)
                return new List<BasicPlanResponseDto>();

            var plans = await _context.BasicPlans
                .Where(p => ids.Contains(p.Id))
                .Include(p => p.Property)
                .Include(p => p.ComboPlanDurations)
                    .ThenInclude(d => d.PackageDuration)
                .ToListAsync();

            var result = plans.Select(p =>
            {
                var firstDuration = p.ComboPlanDurations?
                    .Where(d => d.PackageDuration != null)
                    .OrderBy(d => d.PackageDuration.Value)
                    .FirstOrDefault();

                return new BasicPlanResponseDto
                {
                    Id = p.Id,
                    Code = p.Code ?? string.Empty,
                    Name = p.Name ?? string.Empty,
                    Description = p.Description ?? string.Empty,
                    Price = p.Price,
                    PropertyId = p.PropertyId ?? Guid.Empty,
                    PropertyName = p.Property?.Name ?? "Không xác định",
                    //PackageDurationValue = firstDuration?.PackageDuration?.Value ?? 0,
                    //PackageDurationUnit = firstDuration?.PackageDuration?.Unit.ToString() ?? string.Empty,
                    //DurationDescription = firstDuration?.PackageDuration?.Description ?? string.Empty,
                    PlanSource = "basic"
                };
            }).ToList();

            return result;
        }

        public async Task<decimal> CalculateDynamicPriceFromRoomIdsAsync(List<Guid> roomIds)
        {
            if (roomIds == null || !roomIds.Any())
                return 0;

            var rooms = await _context.Rooms
                .Where(r => roomIds.Contains(r.Id))
                .Include(r => r.AccommodationOption)
                .ToListAsync();

            decimal total = 0;

            foreach (var room in rooms)
            {
                var defaultPrice = room.AccommodationOption?.PricePerNight ?? 0;
                total += defaultPrice;
            }

            return total;
        }

        public async Task<DurationDto?> GetPlanDurationAsync(Guid planId)
        {
            var plan = await _context.BasicPlans
                .Where(p => p.Id == planId)
                .Include(p => p.ComboPlanDurations)
                    .ThenInclude(d => d.PackageDuration)
                .FirstOrDefaultAsync();

            var duration = plan?.ComboPlanDurations?
                .Where(d => d.PackageDuration != null)
                .OrderBy(d => d.PackageDuration.Value)
                .FirstOrDefault();

            if (duration == null || duration.PackageDuration == null)
                return null;

            return new DurationDto
            {
                Value = duration.PackageDuration.Value,
                Unit = duration.PackageDuration.Unit.ToString()
            };
        }
        public async Task<bool> IsRoomBelongToPlanAsync(Guid planId, Guid roomId)
        {
            // 1. Lấy danh sách AccommodationOptionId của gói
            var accommodationOptionIds = await _context.BasicPlanRooms
                .Where(x => x.BasicPlanId == planId)
                .Select(x => x.AccommodationOptionId)
                .ToListAsync();

            if (!accommodationOptionIds.Any())
                return false;

            // 2. Kiểm tra xem Room có nằm trong số các option này không
            return await _context.Rooms
                .AnyAsync(r => r.Id == roomId && accommodationOptionIds.Contains(r.AccommodationOptionId));
        }




    }
}
