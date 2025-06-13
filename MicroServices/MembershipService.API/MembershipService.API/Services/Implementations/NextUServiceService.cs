using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using MembershipService.API.Services.Interfaces;
using static MembershipService.API.Dtos.Request.NextUServiceRequestDto;

namespace MembershipService.API.Services.Implementations
{
    public class NextUServiceService : INextUServiceService
    {
        private readonly INextUServiceRepository _repository;

        public NextUServiceService(INextUServiceRepository repository)
        {
            _repository = repository;
        }

        public async Task<NextUServiceResponseDto> CreateAsync(CreateNextUServiceRequest request)
        {
            var ecosystem = await _repository.GetEcosystemByIdAsync(request.EcosystemId);
            if (ecosystem == null)
                throw new Exception("Ecosystem not found");

            var service = new NextUService
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                UnitType = request.UnitType,
                EcosystemId = request.EcosystemId,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = "system"
            };

            var result = await _repository.AddAsync(service);

            return new NextUServiceResponseDto
            {
                Id = result.Id,
                Name = result.Name,
                UnitType = result.UnitType,
                EcosystemId = result.EcosystemId,
                EcosystemName = ecosystem.Name
            };
        }

        public async Task<NextUServiceResponseDto> GetByIdAsync(Guid id)
        {
            var result = await _repository.GetByIdAsync(id);
            if (result == null) return null;

            return new NextUServiceResponseDto
            {
                Id = result.Id,
                Name = result.Name,
                UnitType = result.UnitType,
                EcosystemId = result.EcosystemId,
                EcosystemName = result.Ecosystem?.Name
            };
        }

        public async Task<List<NextUServiceResponseDto>> GetAllAsync()
        {
            var data = await _repository.GetAllAsync();
            return data.Select(x => new NextUServiceResponseDto
            {
                Id = x.Id,
                Name = x.Name,
                UnitType = x.UnitType,
                EcosystemId = x.EcosystemId,
                EcosystemName = x.Ecosystem?.Name
            }).ToList();
        }

        public async Task<NextUServiceResponseDto> UpdateAsync(Guid id, UpdateNextUServiceRequest request)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) throw new Exception("Service not found");

            existing.Name = request.Name;
            existing.UnitType = request.UnitType;
            existing.EcosystemId = request.EcosystemId;
            existing.UpdatedAt = DateTime.UtcNow;

            var result = await _repository.UpdateAsync(existing);
            var ecosystem = await _repository.GetEcosystemByIdAsync(result.EcosystemId);

            return new NextUServiceResponseDto
            {
                Id = result.Id,
                Name = result.Name,
                UnitType = result.UnitType,
                EcosystemId = result.EcosystemId,
                EcosystemName = ecosystem?.Name
            };
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
