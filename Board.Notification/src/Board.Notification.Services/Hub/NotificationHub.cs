
using Board.Notification.Services.Hub.Interfaces;
using Microsoft.AspNetCore.SignalR;
// using Board.Notification.Services;

namespace Board.Notification.Hub;

public class NotificationHub : Hub<INotificationClient>
{
    public async Task JoinGroup(List<Guid> channels)
    {
        foreach (var channel in channels)
        {
            await Groups.AddToGroupAsync(channel.ToString(), Context.ConnectionId);
        }
    }

    public async Task LeaveGroup(Guid channel)
    {
        await Groups.RemoveFromGroupAsync(channel.ToString(), Context.ConnectionId);
    }

    public async Task SendNotification(Services.Notification notification)
    {
        await Clients.Group(notification.ChannelId.ToString()).ReceiveNotification(notification);
    }

}