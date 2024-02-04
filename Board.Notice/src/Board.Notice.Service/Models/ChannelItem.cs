// Purpose: Model for ChannelItem that mirrors the channel item in the database.
using Board.Common.Models;

namespace Board.Notice.Service.Model;
public class ChannelItem : BaseEntity
{
    public string Name { get; set; }
    public Guid CreatorId { get; set; }

}