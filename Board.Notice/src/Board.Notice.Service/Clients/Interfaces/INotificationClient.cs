public interface INotificationClient
{
    Task SendNotification(string channel, string message);
}
