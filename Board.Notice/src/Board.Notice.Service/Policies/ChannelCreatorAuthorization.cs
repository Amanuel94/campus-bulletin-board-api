
// Summary: Authorization handler for checking if the user is the creator of the channel.

using Board.Auth.Jwt;
using Board.Auth.Jwt.Interfaces;
using Board.Common.Interfaces;
using Board.Notice.Service.Model;
using Microsoft.AspNetCore.Authorization;


namespace Board.Notice.Service.Policies
{
    /// <summary>
    /// Authorization handler for checking if the user is the creator of a channel.
    /// </summary>
    public class ChannelCreatorAuthorizationHandler : AuthorizationHandler<ChannelCreatorRequirement>
    {
        private readonly IGenericRepository<ChannelItem> _channelRepository;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IJwtService _jwtService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChannelCreatorAuthorizationHandler"/> class.
        /// </summary>
        /// <param name="channelRepository">The repository for accessing channel data.</param>
        /// <param name="httpContextAccessor">The accessor for accessing the HTTP context.</param>
        /// <param name="jwtService">The service for handling JWT tokens.</param>
        public ChannelCreatorAuthorizationHandler(IGenericRepository<ChannelItem> channelRepository, IHttpContextAccessor httpContextAccessor, IJwtService jwtService)
        {
            _channelRepository = channelRepository;
            _httpContextAccessor = httpContextAccessor;
            _jwtService = jwtService;
        }

        /// <summary>
        /// Handles the authorization requirement asynchronously.
        /// </summary>
        /// <param name="context">The authorization context.</param>
        /// <param name="requirement">The authorization requirement.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, ChannelCreatorRequirement requirement)
        {
            var identityProvider = new IdentityProvider(_httpContextAccessor.HttpContext!, _jwtService);

            if (_httpContextAccessor.HttpContext!.Request.RouteValues.TryGetValue("channelId", out object? channelIdValue)
                    && Guid.TryParse(channelIdValue?.ToString(), out Guid channelId))
            {
                var userId = identityProvider.GetUserId();
                requirement.ChannelId = channelId;
                if (userId != null)
                {
                    var channel = await _channelRepository.GetAsync(channelId);
                    if (channel != null && channel.CreatorId == userId)
                    {
                        context.Succeed(requirement);
                    }
                }
            }
        }
    }
}
