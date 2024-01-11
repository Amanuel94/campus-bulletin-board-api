using Board.Notice.Service.Model;

namespace Board.Notice.Service.DTOs;

public class CreateNoticeDto
{
    public string Title { get; set; }
    public string Body { get; set; }
    public DateOnly Date { get; set; }
    public required List<string> Resources { get; set; }
    public required List<Category> Categories { get; set; }
    public Importance Importance { get; set; }
    public string Issuer {get; set;} = null!;
    public string Audience {get; set;} = null!;
    public Guid ChannelId {get; set;} = Guid.Empty;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ModifiedAt { get; set; } = DateTime.UtcNow;
}