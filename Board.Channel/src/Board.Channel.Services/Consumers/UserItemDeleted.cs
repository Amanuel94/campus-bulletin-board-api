using Board.Channel.Service.Model;
using Board.Common.Interfaces;
using Board.User.Contracts.Contracts;
using MassTransit;

namespace Board.Channel.Service.Consumers;

public class UserItemDeleted : IConsumer<UserDeleted>
{
    private readonly IGenericRepository<UserItem> _userItemRepository;
    public UserItemDeleted( IGenericRepository<UserItem> userItemRepository)
    {
        _userItemRepository = userItemRepository;
    }
    public async Task Consume(ConsumeContext<UserDeleted> context)
    {
        var message = context.Message;
        var userItem = await _userItemRepository.GetAsync(message.Id);
        if(userItem == null)
        {
            return;
        }

        await _userItemRepository.RemoveAsync(userItem);
        return;

    }
}