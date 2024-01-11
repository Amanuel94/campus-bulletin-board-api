
using MassTransit;
using Board.Channel.Contracts;
using Board.Common.Interfaces;
using Board.Notice.Service.Model;
using AutoMapper;

namespace Board.Notice.Service.Consumer;

public class ChannelItemCreated : IConsumer<ChannelCreated>
{

    private readonly IMapper _mapper;
    private readonly IGenericRepository<ChannelItem> _channelRepository;

    public ChannelItemCreated(IGenericRepository<ChannelItem> channelRepository, IMapper mapper)
    {
        _channelRepository = channelRepository;
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<ChannelCreated> context)
    {
        var channel = context.Message;
        var channelItem = await _channelRepository.GetAsync(channel.Id);
        if(channelItem != null)
        {
            return;
        }
        var ch = _mapper.Map<ChannelItem>(channel);
        ch.Id = channel.Id;
        // Console.WriteLine(ch.Id);
        // Console.WriteLine(channel.Id);
        await _channelRepository.CreateAsync(ch);
        return;

    }
}