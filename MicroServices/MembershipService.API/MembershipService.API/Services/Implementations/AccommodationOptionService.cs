using AutoMapper;
using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using MembershipService.API.Services.Interfaces;

namespace MembershipService.API.Services.Implementations
{
    public class AccommodationOptionService : IAccommodationOptionService
    {
        private readonly IAccommodationOptionRepository _repository;
        private readonly IMapper _mapper;

        public AccommodationOptionService(
            IAccommodationOptionRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<AccommodationOptionResponse>> GetAllAsync()
        {
            var options = await _repository.GetAllAsync();
            return options.Select(opt => _mapper.Map<AccommodationOptionResponse>(opt)).ToList();
        }

        public async Task<AccommodationOptionResponse?> GetByIdAsync(Guid id)
        {
            var option = await _repository.GetByIdAsync(id);
            return option == null ? null : _mapper.Map<AccommodationOptionResponse>(option);
        }

        public async Task<AccommodationOptionResponse> CreateAsync(CreateAccommodationOptionRequest request)
        {
            var entity = _mapper.Map<AccommodationOption>(request);
            var result = await _repository.CreateAsync(entity);
            return _mapper.Map<AccommodationOptionResponse>(result);
        }

        public async Task<AccommodationOptionResponse> UpdateAsync(Guid id, UpdateAccommodationOptionRequest request)
        {
            var existing = await _repository.GetByIdAsync(id);
            if (existing == null) throw new Exception("AccommodationOption not found");

            _mapper.Map(request, existing);
            var updated = await _repository.UpdateAsync(existing);
            return _mapper.Map<AccommodationOptionResponse>(updated);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}
