// Purpose: This file contains the mapping profiles for the user service.

using AutoMapper;
using Board.User.Contracts.Contracts;
using Board.User.Service.DTOs;
namespace Board.User.Service.Profiles;

public class MappingProfile: Profile{
    public MappingProfile()
    {
        CreateMap<Models.User, CreateUserDto>().ReverseMap();
        CreateMap<Models.User, GeneralUserDto>().ReverseMap();
        CreateMap<Models.User, UpdateUserDto>().ReverseMap();

        CreateMap<Models.User,  UserCreated>().ReverseMap();
        CreateMap<Models.User,  UserUpdated>().ReverseMap();
        CreateMap<Models.User,  UserDeleted>().ReverseMap();
    }

}