//Purpose: This file is used to consume the UserDeleted event and remove the user item from the database.
using Board.Channel.Service.Model;
using Board.Common.Interfaces;
using Board.User.Contracts.Contracts;
using MassTransit;

namespace Board.Channel.Service.Consumers;

/// <summary>
/// Represents a consumer for the UserDeleted event.
/// </summary>
public class UserItemDeleted : IConsumer<UserDeleted>
{
    private readonly IGenericRepository<UserItem> _userItemRepository;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserItemDeleted"/> class.
    /// </summary>
    /// <param name="userItemRepository">The repository for user items.</param>
    public UserItemDeleted(IGenericRepository<UserItem> userItemRepository)
    {
        _userItemRepository = userItemRepository;
    }

    /// <summary>
    /// Consumes the UserDeleted event.
    /// </summary>
    /// <param name="context">The context of the consumed event.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
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