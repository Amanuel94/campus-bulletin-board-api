using AutoMapper;
using Board.User.Services.DTOs;
namespace Board.Application.Profiles;

public class MappingProfile: Profile{
    public MappingProfile()
    {
        CreateMap<Board.User.Services.Models.User, CreateUserDto>().ReverseMap();
        CreateMap<Board.User.Services.Models.User, GeneralUserDto>().ReverseMap();
        CreateMap<Board.User.Services.Models.User, LoginRequestDto>().ReverseMap();
        CreateMap<Board.User.Services.Models.User, LoginResponseDto>().ReverseMap();
        CreateMap<Board.User.Services.Models.User, UpdateUserDto>().ReverseMap();
        CreateMap<Board.User.Services.Models.User, UpdatePasswordDto>().ReverseMap();
    }

}