
using Board.Notification.Services.Hub.Interfaces;
using Microsoft.AspNetCore.SignalR;

namespace Board.Notification.Hub;

/// <summary>
/// Represents a hub for sending and receiving notifications.
/// </summary>
public class NotificationHub : Hub<INotificationClient>
{
    /// <summary>
    /// Joins the specified groups.
    /// </summary>
    /// <param name="channels">The list of channels to join.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task JoinGroup(List<string> channels)
    {
        foreach (string channel in channels)
        {
            Console.WriteLine("Joining group: " + channel);
            await Groups.AddToGroupAsync(Context.ConnectionId, channel);
        }
    }

    /// <summary>
    /// Sends a notification to the specified channel.
    /// </summary>
    /// <param name="notification">The notification to send.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    public async Task SendNotification(Services.Notification notification)
    {
        Console.WriteLine("Notification received");
        await Clients.Group(notification.ChannelId).ReceiveNotification(notification);
    }
}