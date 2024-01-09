using Microsoft.AspNetCore.Authorization;

namespace Board.Notice.Service.Policies
{
    public class ChannelCreatorRequirement : IAuthorizationRequirement
    {
        public Guid ChannelId { get; }
        public ChannelCreatorRequirement(Guid channelId)
        {
            ChannelId = channelId;
        }
    }
}