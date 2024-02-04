// Purpose: Model for Channel. This model is used to represent a channel in the application. It contains the properties of a channel and the methods to manipulate the channel.
using Board.Common.Models;

namespace Board.Channel.Service.Model;

public class Channel : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Description { get; set; } = null!;
    public byte[] Logo { get; set; } = null!;
    public Guid CreatorId { get; set; }
    public List<Guid> Members { get; set; } = new();
    public Dictionary<string, DateTime> JoinDates { get; set; }
    public Dictionary<string, DateTime> LeaveDates { get; set; }

}