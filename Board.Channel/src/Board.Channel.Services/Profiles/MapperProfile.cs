
using AutoMapper;
using Board.Channel.Service.DTOs;
using Board.Channel.Service.Model;
using Board.User.Contracts.Contracts;
using Board.Channel.Contracts;

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
        CreateMap<MemberDto, UserItem>().ReverseMap();

        CreateMap<ChannelCreated, Model.Channel>().ReverseMap();
        CreateMap<ChannelDeleted, Model.Channel>().ReverseMap();
    }
}
