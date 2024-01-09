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
        var userId = identityProvider.GetUserId();
        if (userId != null)
        {
            var channel = await _channelRepository.GetAsync(requirement.ChannelId);
            if (channel != null && channel.CreatorId == userId)
            {
                context.Succeed(requirement);
            }
        }
    }
}
