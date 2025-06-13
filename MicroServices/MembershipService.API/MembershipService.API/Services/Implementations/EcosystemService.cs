using AutoMapper;
using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using MembershipService.API.Services.Interfaces;

namespace MembershipService.API.Services.Implementations
{
    public class EcosystemService : IEcosystemService
    {
        private readonly IEcosystemRepository _repository;
        private readonly IMapper _mapper;

        public EcosystemService(IEcosystemRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<EcosystemResponseDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<EcosystemResponseDto>>(entities);
        }

        public async Task<EcosystemResponseDto?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity == null ? null : _mapper.Map<EcosystemResponseDto>(entity);
        }

        public async Task<EcosystemResponseDto> CreateAsync(EcosystemRequestDto dto)
        {
            var entity = _mapper.Map<Ecosystem>(dto);
            entity.Id = Guid.NewGuid();
            await _repository.AddAsync(entity);
            return _mapper.Map<EcosystemResponseDto>(entity);
        }

        public async Task<bool> UpdateAsync(Guid id, EcosystemRequestDto dto)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return false;

            _mapper.Map(dto, entity);
            await _repository.UpdateAsync(entity);
            return true;
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) return false;

            await _repository.DeleteAsync(entity);
            return true;
        }

    }
}
