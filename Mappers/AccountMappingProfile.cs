using AutoMapper;
using MyWebAPI.DataTransferObject.ReturnDtos;
using MyWebAPI.Models;

namespace MyWebAPI.Mappers;

public class AccountMappingProfile : Profile
{
    public AccountMappingProfile()
    {
        CreateMap<Account, AccountDto>()
            .ForMember(
                dest => dest.Products,
                opt => opt.MapFrom(
                        origin => origin.Products.Select(ap => ap.Product)
                    )
            );
    }
}
