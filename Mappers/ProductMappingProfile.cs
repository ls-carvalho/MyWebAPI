using AutoMapper;
using MyWebAPI.DataTransferObject;
using MyWebAPI.DataTransferObject.ReturnDtos;
using MyWebAPI.Models;

namespace MyWebAPI.Mappers;

public class ProductMappingProfile : Profile
{
    public ProductMappingProfile()
    {
        CreateMap<Product, ProductDto>()
            .ForMember(
                dest => dest.Addons,
                opt => opt.MapFrom(
                    origin => origin.Addons.ToList()
                )
            );
        CreateMap<CreateProductDto, Product>();
    }
}
