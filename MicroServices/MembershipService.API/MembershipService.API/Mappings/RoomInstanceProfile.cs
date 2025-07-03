using AutoMapper;
using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;

namespace MembershipService.API.Mappings
{
    public class RoomInstanceProfile : Profile
    {
        public RoomInstanceProfile()
        {
            CreateMap<CreateRoomInstanceRequest, RoomInstance>();
            CreateMap<UpdateRoomInstanceRequest, RoomInstance>();

            CreateMap<RoomInstance, RoomInstanceResponse>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.Floor, opt => opt.MapFrom(src => src.Floor.ToString()))
                .ForMember(dest => dest.RoomTypeName, opt => opt.MapFrom(src => src.AccommodationOption.RoomType.Name));
        }
    }
}
