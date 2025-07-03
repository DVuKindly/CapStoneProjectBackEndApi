using AutoMapper;
using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using MembershipService.API.Services.Interfaces;

namespace MembershipService.API.Services.Implementations
{
    public class RoomInstanceService : IRoomInstanceService
    {
        private readonly IRoomInstanceRepository _repository;
        private readonly IMapper _mapper;

        public RoomInstanceService(IRoomInstanceRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<RoomInstanceResponse>> GetByAccommodationOptionIdAsync(Guid optionId)
        {
            var entities = await _repository.GetByAccommodationOptionIdAsync(optionId);
            return _mapper.Map<List<RoomInstanceResponse>>(entities);
        }

        public async Task<List<RoomInstanceResponse>> GetByBasicPlanIdAsync(Guid planId)
        {
            var entities = await _repository.GetByBasicPlanIdAsync(planId);
            return _mapper.Map<List<RoomInstanceResponse>>(entities);
        }

        public async Task<RoomInstanceResponse?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity != null ? _mapper.Map<RoomInstanceResponse>(entity) : null;
        }

        public async Task<RoomInstanceResponse> CreateAsync(CreateRoomInstanceRequest request)
        {
            var entity = _mapper.Map<RoomInstance>(request);
            var created = await _repository.CreateAsync(entity);
            return _mapper.Map<RoomInstanceResponse>(created);
        }

        public async Task<RoomInstanceResponse> UpdateAsync(Guid id, UpdateRoomInstanceRequest request)
        {
            var entity = await _repository.GetByIdAsync(id);
            if (entity == null) throw new Exception("RoomInstance not found");

            _mapper.Map(request, entity);
            var updated = await _repository.UpdateAsync(entity);
            return _mapper.Map<RoomInstanceResponse>(updated);
        }

        public async Task<bool> DeleteAsync(Guid id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
}