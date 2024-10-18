using AutoMapper;
using MyWebAPI.DataTransferObject;
using MyWebAPI.DataTransferObject.ReturnDtos;
using MyWebAPI.Models;

namespace MyWebAPI;

public class MyMappingProfile : Profile
{
    public MyMappingProfile()
    {
        CreateMap<Addon, AddonDto>();
        CreateMap<CreateAddonDto, Addon>()
            .ForMember(dest => dest.Product, opt => opt.Ignore());
        CreateMap<UpdateAddonDto, Addon>()
            .ForMember(dest => dest.ProductId, opt => opt.Ignore());
    }
}
