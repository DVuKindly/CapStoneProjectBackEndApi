using AutoMapper;
using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using MembershipService.API.Services.Interfaces;

namespace MembershipService.API.Services.Implementations
{
    public class EntitlementRuleService : IEntitlementRuleService
    {
        private readonly IEntitlementRuleRepository _repo;
        private readonly IMapper _mapper;

        public EntitlementRuleService(IEntitlementRuleRepository repo, IMapper mapper)
        {
            _repo = repo;
            _mapper = mapper;
        }

        public async Task<List<EntitlementRuleDto>> GetAllAsync()
            => _mapper.Map<List<EntitlementRuleDto>>(await _repo.GetAllAsync());

        public async Task<EntitlementRuleDto> GetByIdAsync(Guid id)
        {
            var entity = await _repo.GetByIdAsync(id);
            if (entity == null) throw new KeyNotFoundException("EntitlementRule không tồn tại");
            return _mapper.Map<EntitlementRuleDto>(entity);
        }

        public async Task<EntitlementRuleDto> CreateAsync(CreateEntitlementRuleDto dto)
        {
            if (dto.Price < 0 || dto.CreditAmount <= 0)
                throw new ArgumentException("Giá và credit phải > 0");

            var entity = _mapper.Map<EntitlementRule>(dto);
            var created = await _repo.AddAsync(entity);
            return _mapper.Map<EntitlementRuleDto>(created);
        }

        public async Task<EntitlementRuleDto> UpdateAsync(UpdateEntitlementRuleDto dto)
        {
            var existing = await _repo.GetByIdAsync(dto.Id);
            if (existing == null) throw new KeyNotFoundException("EntitlementRule không tồn tại");

            _mapper.Map(dto, existing);
            var success = await _repo.UpdateAsync(existing);
            if (!success) throw new Exception("Cập nhật thất bại");
            return _mapper.Map<EntitlementRuleDto>(existing);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var success = await _repo.DeleteAsync(id);
            if (!success) throw new Exception("Xoá thất bại hoặc không tìm thấy");
            return true;
        }

        public async Task<List<EntitlementRuleDto>> GetByBasicPlanIdAsync(Guid basicPlanId)
        {
            var rules = await _repo.GetByBasicPlanIdAsync(basicPlanId);
            return _mapper.Map<List<EntitlementRuleDto>>(rules);
        }

        public async Task<decimal> GetTotalEntitlementPriceAsync(Guid basicPlanId)
        {
            return await _repo.GetTotalEntitlementPriceAsync(basicPlanId);
        }


    }
}
