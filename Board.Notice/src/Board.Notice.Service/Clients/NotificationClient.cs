// Purpose: This file is used to communicate with the notification service. It uses the HttpClient to send a POST request to the notification service.
/// <summary>
/// Represents a client for sending notifications.
/// </summary>
public class NotificationClient
{
    private readonly HttpClient _httpClient;

    public NotificationClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    /// <summary>
    /// Sends a notification to the specified channel with the given message.
    /// </summary>
    /// <param name="channel">The id of the channel whose members need to be notified.</param>
    /// <param name="message">The message content of the notification.</param>
    public async Task SendNotification(string channel, string message)
    {
        try
        {
            Console.WriteLine("Sending notification...");
            var response = await _httpClient.PostAsJsonAsync("/api/notify", new { channelId = channel, Content = message });
            response.EnsureSuccessStatusCode();
        }
        catch (System.Exception ex)
        {
            Console.WriteLine(ex.Message);
        }
    }
}
