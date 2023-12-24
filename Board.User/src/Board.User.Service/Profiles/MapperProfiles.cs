using AutoMapper;
using Board.User.Service.DTOs;
namespace Board.User.Service.Profiles;

public class MappingProfile: Profile{
    public MappingProfile()
    {
        CreateMap<Models.User, CreateUserDto>().ReverseMap();
        CreateMap<Models.User, GeneralUserDto>().ReverseMap();
        CreateMap<Models.User, UpdateUserDto>().ReverseMap();
    }

}