using AutoMapper;
using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;

namespace MembershipService.API.Mappings
{
    public class AccommodationOptionProfile : Profile
    {
        public AccommodationOptionProfile()
        {
            // Entity → Response DTO
            CreateMap<AccommodationOption, AccommodationOptionResponse>()
                .ForMember(dest => dest.RoomTypeName,
                    opt => opt.MapFrom(src => src.RoomType != null ? src.RoomType.Name : null))
                .ForMember(dest => dest.PropertyName,
                    opt => opt.MapFrom(src => src.Property != null ? src.Property.Name : null))
                .ForMember(dest => dest.NextUServiceName,
                    opt => opt.MapFrom(src => src.NextUService != null ? src.NextUService.Name : null));

            // Create DTO → Entity
            CreateMap<CreateAccommodationOptionRequest, AccommodationOption>();

            // Update DTO → Entity
            CreateMap<UpdateAccommodationOptionRequest, AccommodationOption>();
        }
    }
}
