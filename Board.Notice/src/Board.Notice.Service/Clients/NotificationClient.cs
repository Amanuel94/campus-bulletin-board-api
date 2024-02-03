public class NotificationClient
{
    private readonly HttpClient _httpClient;

    public NotificationClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task SendNotification(string channel, string message)
    {

            try
            {
                Console.WriteLine("Sending notification");
                var response = await _httpClient.PostAsJsonAsync("/api/notify", new { channelId = channel, Content = message });
                response.EnsureSuccessStatusCode();
            }
            catch (System.Exception ex)
            {

                Console.WriteLine(ex.Message);
            }

    }
}
