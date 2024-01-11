using Board.Common.Models;

namespace Board.Notice.Service.Model;
public class ChannelItem : BaseEntity
{
    public string Name { get; set; }
    public Guid CreatorId { get; set; }

}