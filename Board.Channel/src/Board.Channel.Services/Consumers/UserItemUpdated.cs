// Purpose: This file contains the UserItemUpdated consumer class. This class is responsible for consuming the UserUpdated message and updating the user item in the database.
using AutoMapper;
using Board.Channel.Service.Model;
using Board.Common.Interfaces;
using Board.User.Contracts.Contracts;
using MassTransit;

namespace Board.Channel.Service.Consumers;

/// <summary>
/// Represents a consumer for handling UserUpdated messages.
/// </summary>
public class UserItemUpdated : IConsumer<UserUpdated>
{
    private readonly IGenericRepository<UserItem> _userItemRepository;
    private readonly IMapper _mapper;

    /// <summary>
    /// Initializes a new instance of the <see cref="UserItemUpdated"/> class.
    /// </summary>
    /// <param name="userItemRepository">The repository for UserItem entities.</param>
    /// <param name="mapper">The mapper for mapping UserUpdated messages to UserItem entities.</param>
    public UserItemUpdated(IGenericRepository<UserItem> userItemRepository, IMapper mapper)
    {
        _userItemRepository = userItemRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Consumes a UserUpdated message and updates the corresponding UserItem entity.
    /// </summary>
    /// <param name="context">The ConsumeContext containing the UserUpdated message.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task Consume(ConsumeContext<UserUpdated> context)
    {
        var message = context.Message;
        var userItem = await _userItemRepository.GetAsync(message.Id);
        if (userItem == null)
        {
            return;
        }

        await _userItemRepository.UpdateAsync(_mapper.Map<UserItem>(message));
        return;
    }
}