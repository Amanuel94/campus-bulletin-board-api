namespace Board.Notification.Services.Hub.Interfaces
{
    public interface INotificationClient
    {
        Task ReceiveNotification(string? message);
    }
}