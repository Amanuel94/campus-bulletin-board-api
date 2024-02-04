using Board.Common.Models;

// Purpose: Model for Notice.
namespace Board.Notice.Service.Model;

/// <summary>
/// Represents a notice in the bulletin board system.
/// </summary>
public class Notice : BaseEntity
{
    /// <summary>
    /// Gets or sets the title of the notice.
    /// </summary>
    public string Title { get; set; } = null!;

    /// <summary>
    /// Gets or sets the body of the notice.
    /// </summary>
    public string Body { get; set; } = null!;

    /// <summary>
    /// Gets or sets the date of the notice.
    /// </summary>
    public DateOnly Date { get; set; }

    /// <summary>
    /// Gets or sets the list of resources associated with the notice.
    /// </summary>
    public required List<string> Resources { get; set; }

    /// <summary>
    /// Gets or sets the list of categories associated with the notice.
    /// </summary>
    public required List<Category> Categories { get; set; }

    /// <summary>
    /// Gets or sets the importance level of the notice.
    /// </summary>
    public Importance Importance { get; set; }

    /// <summary>
    /// Gets or sets the issuer of the notice.
    /// </summary>
    public string Issuer { get; set; } = null!;

    /// <summary>
    /// Gets or sets the intended audience of the notice.
    /// </summary>
    public string Audience { get; set; } = null!;

    /// <summary>
    /// Gets or sets the ID of the channel associated with the notice.
    /// </summary>
    public Guid ChannelId { get; set; }
}
