
using AutoMapper;
using Board.Channel.Service.DTOs;


namespace Board.Channel.Service.Profiles;
public class MapperProfile : Profile
{
    public MapperProfile()
    {
        CreateMap<Model.Channel, GeneralChannelDto>().ReverseMap();
        CreateMap<CreateChannelDto, Model.Channel>().ReverseMap();
        CreateMap<UpdateChannelDto, Model.Channel>().ReverseMap();
    }
}
