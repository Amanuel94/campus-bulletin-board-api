// Purpose: This file contains the ChannelCreated class which is used to represent the event of a channel being created.
namespace Board.Channel.Contracts;
public class ChannelCreated
{
    public Guid Id {get; set;}
    public Guid CreatorId{get; set;}

}
