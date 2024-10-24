using AutoMapper;
using MyWebAPI.DataTransferObject;
using MyWebAPI.DataTransferObject.ReturnDtos;
using MyWebAPI.Models;

namespace MyWebAPI.Mappers;

public class UserMappingProfile : Profile
{
    public UserMappingProfile()
    {
        CreateMap<User, UserDto>();
        CreateMap<CreateUserDto, User>()
            .ForMember(
                dest => dest.Account,
                opt => opt.MapFrom(
                        origin => new Account
                        {
                            DisplayName = origin.AccountDisplayName
                        }
                    )
            );
        CreateMap<UpdateUserDto, User>();
    }
}
