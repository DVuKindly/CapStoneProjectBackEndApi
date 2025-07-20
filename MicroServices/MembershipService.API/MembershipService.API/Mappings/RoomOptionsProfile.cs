using AutoMapper;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;

namespace MembershipService.API.Mappings
{
    public class RoomOptionsProfile : Profile
    {
        public RoomOptionsProfile()
        {
            CreateMap<RoomSizeOption, SimpleOptionRoomDto>();
            CreateMap<RoomViewOption, SimpleOptionRoomDto>();
            CreateMap<RoomFloorOption, SimpleOptionRoomDto>();
            CreateMap<BedTypeOption, SimpleOptionRoomDto>();

        }

    }
}
