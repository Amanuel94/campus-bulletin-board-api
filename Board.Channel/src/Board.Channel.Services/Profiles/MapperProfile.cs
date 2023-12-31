
using AutoMapper;
using Board.Channel.Service.DTOs;
using Board.Channel.Service.Model;
using Board.User.Contracts.Contracts;

namespace Board.Channel.Service.Profiles;
public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Model.Channel, GeneralChannelDto>().ReverseMap();
        CreateMap<CreateChannelDto, Model.Channel>().ReverseMap();
        CreateMap<UpdateChannelDto, Model.Channel>().ReverseMap();

        CreateMap<UserCreated, UserItem>().ReverseMap();
        CreateMap<UserUpdated, UserItem>().ReverseMap();
    }
}
