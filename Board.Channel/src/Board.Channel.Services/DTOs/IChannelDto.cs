// using Board.Channel.Service.Model;

namespace Board.Channel.Service.DTOs;
public abstract class IChannelDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public byte[] Logo { get; set; } = null!;
    // public ChannelType Type { get; set; }
    public Guid CreatorId { get; set; }
    public List<Guid> Members { get; set; } = new();
    public Dictionary<string, DateTime> JoinDates { get; set; }
    public Dictionary<string, DateTime> LeaveDates { get; set; }
}