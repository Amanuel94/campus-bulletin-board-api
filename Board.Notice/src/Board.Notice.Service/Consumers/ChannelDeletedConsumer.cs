
// Purpose: consume the ChannelDeleted event and delete the channel item from the database.

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

    /// <summary>
    /// Initializes a new instance of the <see cref="ChannelItemDeleted"/> class.
    /// </summary>
    /// <param name="channelRepository">The repository for managing channel items.</param>
    /// <param name="mapper">The mapper for object mapping.</param>
    public ChannelItemDeleted(IGenericRepository<ChannelItem> channelRepository, IMapper mapper)
    {
        _channelRepository = channelRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Consumes the channel deleted event and removes the corresponding channel item.
    /// </summary>
    /// <param name="context">The consume context containing the channel deleted event.</param>
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