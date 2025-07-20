using AutoMapper;
using MembershipService.API.Data;
using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using MembershipService.API.Services.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace MembershipService.API.Services.Implementations
{
    public class RoomInstanceService : IRoomInstanceService
    {
        private readonly IRoomInstanceRepository _repository;
        private readonly IMapper _mapper;
        private readonly MembershipDbContext _db;

        public RoomInstanceService(IRoomInstanceRepository repository, IMapper mapper , MembershipDbContext db)
        {
            _repository = repository;
            _mapper = mapper;
            _db = db;
        }

        public async Task<List<RoomInstanceResponse>> GetByAccommodationOptionIdAsync(Guid optionId)
        {
            var entities = await _repository.GetByAccommodationOptionIdAsync(optionId);
            return _mapper.Map<List<RoomInstanceResponse>>(entities);
        }

        public async Task<List<RoomInstanceResponse>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<RoomInstanceResponse>>(entities);
        }

        public async Task<List<RoomInstanceResponse>> GetByPropertyIdAsync(Guid PropertyId)
        {
            var entities = await _repository.GetByPropertyIdAsync(PropertyId);
            return _mapper.Map<List<RoomInstanceResponse>>(entities);
        }

        public async Task<RoomInstanceResponse?> GetByIdAsync(Guid id)
        {
            var entity = await _repository.GetByIdAsync(id);
            return entity != null ? _mapper.Map<RoomInstanceResponse>(entity) : null;
        }

        public async Task<RoomInstanceResponse> CreateAsync(CreateRoomInstanceRequest request)
        {
            //  Check trùng RoomCode trong cùng AccommodationOptionId
            if (await _repository.IsRoomCodeDuplicateAsync(request.AccommodationOptionId, request.RoomCode))
            {
                throw new InvalidOperationException($"Room code '{request.RoomCode}' already exists.");
            }

            // Map sang entity và tạo
            var entity = _mapper.Map<RoomInstance>(request);
            var created = await _repository.CreateAsync(entity);

            // Load lại từ DB với Include đầy đủ
            var fullEntity = await _repository.GetByIdWithNavigationAsync(created.Id);

            return _mapper.Map<RoomInstanceResponse>(fullEntity);
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

        public async Task<decimal> GetAddOnFeeAsync(Guid roomInstanceId)
        {
            var room = await _db.Rooms.FindAsync(roomInstanceId);
            if (room == null) throw new Exception("Room not found.");
            return room.AddOnFee ?? 0;
        }


    }
}