using Board.Common.Models;

namespace Board.Notice.Service.Model;

public class Notice : BaseEntity
{
    public string Title { get; set; } = null!;
    public string Body { get; set; } = null!;
    public DateOnly Date { get; set; }
    public required List<string> Resources { get; set; }
    public required List<Category> Categories { get; set; }
    public Importance Importance { get; set; }
    public string Issuer {get; set;} = null!;
    public string Audience {get; set;} = null!;
    public Guid ChannelId {get; set;}
}
