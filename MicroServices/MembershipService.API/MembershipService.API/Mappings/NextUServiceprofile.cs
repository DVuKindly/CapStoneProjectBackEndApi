using AutoMapper;
using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;

namespace MembershipService.API.Mappings
{
    public class NextUServiceprofile : Profile
    {
        public NextUServiceprofile()
        {
            CreateMap<CreateNextUServiceRequest, NextUService>();
            CreateMap<UpdateNextUServiceRequest, NextUService>();
            CreateMap<NextUService, NextUServiceResponseDto>()
                .ForMember(dest => dest.EcosystemName, opt => opt.MapFrom(src => src.Ecosystem.Name))
                .ForMember(dest => dest.LocationName, opt => opt.MapFrom(src => src.Location != null ? src.Location.Name : ""))
                .ForMember(dest => dest.MediaGalleryId, opt => opt.MapFrom(src => src.MediaGallery.Select(m => m.Id).ToList()));
        }
    }
    
}
