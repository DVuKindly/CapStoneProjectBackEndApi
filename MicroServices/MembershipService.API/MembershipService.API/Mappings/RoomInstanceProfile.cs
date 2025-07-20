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
                .ForMember(dest => dest.RoomTypeName, opt => opt.MapFrom(src => src.AccommodationOption.RoomType.Name))
                .ForMember(dest => dest.PropertyId, opt => opt.MapFrom(src => src.AccommodationOption.Property.Id))
                .ForMember(dest => dest.PropertyName, opt => opt.MapFrom(src => src.AccommodationOption.Property.Name))
                .ForMember(dest => dest.LocationId, opt => opt.MapFrom(src => src.AccommodationOption.Property.Location.Id))
                .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.AccommodationOption.Property.Location.Name))
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.AccommodationOption.Property.Location.City.Id))
                .ForMember(dest => dest.CityName, opt => opt.MapFrom(src => src.AccommodationOption.Property.Location.City.Name))

                // Optional fields
                .ForMember(dest => dest.RoomSizeName, opt => opt.MapFrom(src => src.RoomSizeOption != null ? src.RoomSizeOption.Name : null))
                .ForMember(dest => dest.RoomViewName, opt => opt.MapFrom(src => src.RoomViewOption != null ? src.RoomViewOption.Name : null))
                .ForMember(dest => dest.RoomFloorName, opt => opt.MapFrom(src => src.RoomFloorOption != null ? src.RoomFloorOption.Name : null))
                .ForMember(dest => dest.BedTypeName, opt => opt.MapFrom(src => src.BedTypeOption != null ? src.BedTypeOption.Name : null));

        }
    }
}
