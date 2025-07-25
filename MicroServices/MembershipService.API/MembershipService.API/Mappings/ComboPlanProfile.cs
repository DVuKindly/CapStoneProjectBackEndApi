﻿using AutoMapper;
using MembershipService.API.Dtos.Request;
using MembershipService.API.Dtos.Response;
using MembershipService.API.Entities;

namespace MembershipService.API.Mappings
{
    public class ComboPlanProfile : Profile
    {
        public ComboPlanProfile()
        {
            CreateMap<CreateComboPlanRequest, ComboPlan>();
            CreateMap<UpdateComboPlanRequest, ComboPlan>();

            CreateMap<ComboPlan, ComboPlanResponseDto>()
                .ForMember(dest => dest.PropertyName, opt => opt.MapFrom(src => src.Property.Name))
                .ForMember(dest => dest.BasicPlanIds, opt => opt.MapFrom(src => src.ComboPlanBasics.Select(x => x.BasicPlanId)))
                .ForMember(dest => dest.PackageDurations, opt => opt.MapFrom(src => src.ComboPlanDurations
                    .Select(d => new PackageDurationDto
                    {
                        DurationId = d.PackageDurationId,
                        DiscountRate = d.DiscountDurationRate
                    })))
                // ✅ new
                .ForMember(dest => dest.BasicPlanCategoryName, opt => opt.MapFrom(src => src.PlanCategory.Name))
                .ForMember(dest => dest.PlanLevelName, opt => opt.MapFrom(src => src.PlanLevel.Name))
                .ForMember(dest => dest.TargetAudienceName, opt => opt.MapFrom(src => src.PlanTargetAudience.Name));

        }
    }
}