
using MassTransit;
using Board.Channel.Contracts;
using Board.Common.Interfaces;
using Board.Notice.Service.Model;
using AutoMapper;

namespace Board.Notice.Service.Consumer;

public class ChannelItemDeleted : IConsumer<ChannelDeleted>
{

    private readonly IMapper _mapper;
    private readonly IGenericRepository<ChannelItem> _channelRepository;

    public ChannelItemDeleted(IGenericRepository<ChannelItem> channelRepository, IMapper mapper)
    {
        _channelRepository = channelRepository;
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<ChannelDeleted> context)
    {
        var channel = context.Message;
        var channelItem = await _channelRepository.GetAsync(channel.Id);
        if(channelItem != null)
        {
            await _channelRepository.RemoveAsync(channelItem);
        }

    }
}