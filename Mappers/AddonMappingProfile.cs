using AutoMapper;
using MyWebAPI.DataTransferObject;
using MyWebAPI.DataTransferObject.ReturnDtos;
using MyWebAPI.Models;

namespace MyWebAPI.Mappers;

public class AddonMappingProfile : Profile
{
    public AddonMappingProfile()
    {
        CreateMap<Addon, AddonDto>();
        CreateMap<CreateAddonDto, Addon>()
            .ForMember(dest => dest.Product, opt => opt.Ignore());
        CreateMap<UpdateAddonDto, Addon>()
            .ForMember(dest => dest.Product, opt => opt.Ignore())
            .ForMember(dest => dest.ProductId, opt => opt.Ignore());
        CreateMap<Addon, AddonWithoutProductIdDto>();
    }
}
