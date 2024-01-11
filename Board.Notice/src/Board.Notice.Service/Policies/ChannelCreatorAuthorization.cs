using Board.Auth.Jwt;
using Board.Auth.Jwt.Interfaces;
using Board.Common.Interfaces;
using Board.Notice.Service.Model;
using Microsoft.AspNetCore.Authorization;


namespace Board.Notice.Service.Policies;
public class ChannelCreatorAuthorizationHandler : AuthorizationHandler<ChannelCreatorRequirement>
{
    private readonly IGenericRepository<ChannelItem> _channelRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IJwtService _jwtService;

    public ChannelCreatorAuthorizationHandler(IGenericRepository<ChannelItem> channelRepository, IHttpContextAccessor httpContextAccessor, IJwtService jwtService)
    {
        _channelRepository = channelRepository;
        _httpContextAccessor = httpContextAccessor;
        _jwtService = jwtService;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ChannelCreatorRequirement requirement)
    {
        var identityProvider = new IdentityProvider(_httpContextAccessor.HttpContext, _jwtService);

        if (_httpContextAccessor.HttpContext.Request.RouteValues.TryGetValue("channelId", out object? channelIdValue)
                && Guid.TryParse(channelIdValue?.ToString(), out Guid channelId))
        {
            // Console.WriteLine(identityProvider.GetUserId());
            var userId = identityProvider.GetUserId();

            requirement.ChannelId = channelId;
            Console.WriteLine("ChannelCreatorAuthorizationHandler");
            Console.WriteLine(channelId);
            Console.WriteLine(userId);
            foreach (var item in await _channelRepository.GetAllAsync())
            {
                Console.WriteLine(item.Id);
            }
            if (userId != null)
            {
                var channel = await _channelRepository.GetAsync(channelId);

                Console.WriteLine(channel);
                if (channel != null && channel.CreatorId == userId)
                {
                    context.Succeed(requirement);
                }
            }
        }

    }
}
