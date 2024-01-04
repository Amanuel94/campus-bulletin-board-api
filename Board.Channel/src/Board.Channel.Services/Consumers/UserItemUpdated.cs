using AutoMapper;
using Board.Channel.Service.Model;
using Board.Common.Interfaces;
using Board.User.Contracts.Contracts;
using MassTransit;

namespace Board.Channel.Service.Consumers;

public class UserItemUpdated : IConsumer<UserUpdated>
{
    private readonly IGenericRepository<UserItem> _userItemRepository;
    private readonly IMapper _mapper;
    public UserItemUpdated( IGenericRepository<UserItem> userItemRepository, IMapper mapper)
    {
        _userItemRepository = userItemRepository;
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<UserUpdated> context)
    {
        var message = context.Message;
        var userItem = await _userItemRepository.GetAsync(message.Id);
        if(userItem == null)
        {
            return;
        }

        await _userItemRepository.UpdateAsync(_mapper.Map<UserItem>(message));
        return;

    }
}