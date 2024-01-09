namespace Board.Channel.Service.DTOs
{
    public class CreateChannelDto : IChannelDto
    {
    public Guid CreatorId { get; set; } = new Guid();
    public List<Guid> Members { get; set; } = new();
    public Dictionary<string, DateTime> JoinDates { get; set; } = new();
    public Dictionary<string, DateTime> LeaveDates { get; set; } = new();
    }
}