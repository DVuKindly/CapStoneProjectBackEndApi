using AutoMapper;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;
using MembershipService.API.Repositories.Interfaces;
using MembershipService.API.Services.Interfaces;

namespace MembershipService.API.Services.Implementations
{
    public class RoomSizeOptionService : IRoomSizeOptionService
    {
        private readonly IRoomSizeOptionRepository _repository;
        private readonly IMapper _mapper;
        public RoomSizeOptionService(IRoomSizeOptionRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<List<SimpleOptionRoomDto>> GetAllAsync()
        {
            var entities = await _repository.GetAllAsync();
            return _mapper.Map<List<SimpleOptionRoomDto>>(entities);
        }
    }

}
