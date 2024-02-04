// Purpose: This file contains the UserItemCreated consumer class. This class is used to consume the UserCreated event and create a new UserItem in the database.

using AutoMapper;
using Board.Channel.Service.Model;
using Board.Common.Interfaces;
using Board.User.Contracts.Contracts;
using MassTransit;

namespace Board.Channel.Service.Consumers;

/// <summary>
/// Represents a consumer for the UserCreated event.
/// </summary>
public class UserItemCreated : IConsumer<UserCreated>
{
    private readonly IGenericRepository<UserItem> _userItemRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserItemCreated"/> class.
    /// </summary>
    /// <param name="userItemRepository">The repository for user items.</param>
    /// <param name="mapper">The mapper used for object mapping.</param>
    public UserItemCreated(IGenericRepository<UserItem> userItemRepository, IMapper mapper)
    {
        _userItemRepository = userItemRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Consumes the UserCreated event and performs necessary actions.
    /// </summary>
    /// <param name="context">The context containing the UserCreated event message.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Consume(ConsumeContext<UserCreated> context)
    {
        var message = context.Message;
        var userItem = await _userItemRepository.GetAsync(message.Id);
        if (userItem != null)
        {
            return;
        }

        var user = _mapper.Map<UserItem>(message);
        user.Id = message.Id;
        await _userItemRepository.CreateAsync(user);
        return;
    }
}