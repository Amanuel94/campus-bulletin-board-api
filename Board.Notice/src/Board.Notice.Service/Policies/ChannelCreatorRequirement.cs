// Purpose: Policy for checking if the user is the creator of the channel.
using Microsoft.AspNetCore.Authorization;

namespace Board.Notice.Service.Policies
{
    public class ChannelCreatorRequirement : IAuthorizationRequirement
    {
        public Guid ChannelId { get; set; }

        public ChannelCreatorRequirement(Guid channelId)
        {
            ChannelId = channelId;
        }
    }
}