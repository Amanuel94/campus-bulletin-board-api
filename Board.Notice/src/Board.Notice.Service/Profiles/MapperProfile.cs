using AutoMapper;
using Board.Channel.Contracts;
using Board.Notice.Service.DTOs;

namespace Board.Notice.Service.Profiles
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Model.Notice, GeneralNoticeDto>().ReverseMap();
            CreateMap<Model.Notice, CreateNoticeDto>().ReverseMap();
            CreateMap<Model.Notice, UpdateNoticeDto>().ReverseMap();

            CreateMap<Model.ChannelItem, ChannelCreated>().ReverseMap();
        }
    }
}
