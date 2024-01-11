using AutoMapper;
using Board.Channel.Service.Model;
using Board.Common.Interfaces;
using Board.User.Contracts.Contracts;
using MassTransit;

namespace Board.Channel.Service.Consumers;

public class UserItemCreated : IConsumer<UserCreated>
{
    private readonly IGenericRepository<UserItem> _userItemRepository;
    private readonly IMapper _mapper;
    public UserItemCreated( IGenericRepository<UserItem> userItemRepository, IMapper mapper)
    {
        _userItemRepository = userItemRepository;
        _mapper = mapper;
    }
    public async Task Consume(ConsumeContext<UserCreated> context)
    {
        var message = context.Message;
        var userItem = await _userItemRepository.GetAsync(message.Id);
        if(userItem != null)
        {
            return;
        }
        // Console.WriteLine("UserItemCreated");
        var user = _mapper.Map<UserItem>(message);
        user.Id = message.Id;
        await _userItemRepository.CreateAsync(user);
        return;

    }
}