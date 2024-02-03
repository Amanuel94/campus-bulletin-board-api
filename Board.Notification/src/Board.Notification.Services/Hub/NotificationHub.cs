
using Board.Notification.Services.Hub.Interfaces;
using Microsoft.AspNetCore.SignalR;
// using Board.Notification.Services;

namespace Board.Notification.Hub;

public class NotificationHub : Hub<INotificationClient>
{
    public async Task JoinGroup(List<string> channels)
    {
        foreach (string channel in channels)
        {
            await Groups.AddToGroupAsync(channel, Context.ConnectionId);
        }
    }

    public async Task LeaveGroup(string channel)
    {
        await Groups.RemoveFromGroupAsync(channel, Context.ConnectionId);
    }

    public async Task SendNotification(Services.Notification notification)
    {
        await Clients.Group(notification.ChannelId).ReceiveNotification(notification);
    }

}