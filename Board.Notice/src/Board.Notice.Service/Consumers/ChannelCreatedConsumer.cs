
// Purpose: This file is used to consume the ChannelCreated event and create a new ChannelItem in the database.

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

    /// <summary>
    /// Initializes a new instance of the <see cref="ChannelItemCreated"/> class.
    /// </summary>
    /// <param name="channelRepository">The channel repository.</param>
    /// <param name="mapper">The mapper.</param>
    public ChannelItemCreated(IGenericRepository<ChannelItem> channelRepository, IMapper mapper)
    {
        _channelRepository = channelRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Consumes the channel created event and creates a new channel item.
    /// </summary>
    /// <param name="context">The consume context.</param>
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
        await _channelRepository.CreateAsync(ch);
        return;
    }
}