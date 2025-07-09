using AutoMapper;
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
        private readonly IMapper _mapper;
        private readonly MembershipDbContext _context;

        public BasicPlanService(
            IBasicPlanRepository basicPlanRepo,
            IComboPlanDurationRepository durationRepo,
            IBasicPlanRoomRepository basicPlanRoomRepo,
            IMapper mapper,
            MembershipDbContext context)
        {
            _basicPlanRepo = basicPlanRepo;
            _durationRepo = durationRepo;
            _basicPlanRoomRepo = basicPlanRoomRepo;
            _mapper = mapper;
            _context = context;
        }

        public async Task<BasicPlanResponseDto> CreateAsync(CreateBasicPlanRequest request)
        {
            //Console.WriteLine($"Request: {JsonConvert.SerializeObject(request)}");
            var basicPlan = _mapper.Map<BasicPlan>(request);
            basicPlan.Code = Guid.NewGuid().ToString("N").Substring(0, 10).ToUpper();
            var createdPlan = await _basicPlanRepo.AddAsync(basicPlan);

            if (request.Rooms != null && request.Rooms.Any())
            {
                foreach (var room in request.Rooms)
                {
                    var roomEntity = _mapper.Map<BasicPlanRoom>(room);
                    roomEntity.BasicPlanId = createdPlan.Id;
                    //var roomEntity = new BasicPlanRoom
                    //{
                    //    BasicPlanId = createdPlan.Id,
                    //    AccommodationOptionId = room.AccomodationId,
                    //    RoomInstanceId = room.RoomInstanceId,  // đổi tên
                    //    NightsIncluded = room.NightsIncluded,
                    //    CustomPricePerNight = room.CustomPricePerNight,
                    //    TotalPrice = room.TotalPrice
                    //};
                    await _basicPlanRoomRepo.AddAsync(roomEntity);
                }
            }
            if (request.PackageDurations != null && request.PackageDurations.Any())
            {
                var planDurations = request.PackageDurations
                    .Select(dto =>
                    {
                        var entity = _mapper.Map<ComboPlanDuration>(dto);
                        entity.BasicPlanId = createdPlan.Id;
                        return entity;
                    })
                    .ToList();

                await _durationRepo.AddRangeAsync(planDurations);
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
                .Include(p => p.Location)
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
                    LocationId = p.LocationId ?? Guid.Empty,
                    LocationName = p.Location?.Name ?? "Không xác định",
                    PackageDurationValue = firstDuration?.PackageDuration?.Value ?? 0,
                    PackageDurationUnit = firstDuration?.PackageDuration?.Unit.ToString() ?? string.Empty,
                    DurationDescription = firstDuration?.PackageDuration?.Description ?? string.Empty,
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
        public async Task<bool> IsRoomBelongToPlanAsync(Guid planId, Guid roomInstanceId)
        {
            // 1. Nếu gói đã có RoomInstanceId cụ thể → kiểm tra trùng khớp
            var hasFixedRoom = await _context.BasicPlanRooms
                .AnyAsync(x => x.BasicPlanId == planId && x.RoomInstanceId == roomInstanceId);

            if (hasFixedRoom)
                return true;

            // 2. Nếu gói không gán Room → cho phép chọn phòng từ option hợp lệ
            var allowedOptionIds = await _context.BasicPlanRooms
                .Where(x => x.BasicPlanId == planId && x.RoomInstanceId == null)
                .Select(x => x.AccommodationOptionId)
                .ToListAsync();

            if (!allowedOptionIds.Any())
                return false;

            return await _context.Rooms.AnyAsync(r =>
                r.Id == roomInstanceId &&
                allowedOptionIds.Contains(r.AccommodationOptionId));
        }




    }
}
