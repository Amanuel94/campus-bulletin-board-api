using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;

namespace Board.Notification.Services.Controllers
{

    [ApiController]
    [Route("api/notifications")]
    public class NotificationsController : ControllerBase
    {
        private HubConnection HubConnectionBuilder()
        {
            return new HubConnectionBuilder()
                .WithUrl("http://localhost:5109/notificationHub")
                .Build();
        }

        [HttpPost]
        public IActionResult Notify(Guid channel, string message)
        {
            try{
                var connection = HubConnectionBuilder();
                var notification = new Notification
                {
                    ChannelId = channel,
                    Content = message
                };
                connection.StartAsync();
                connection.InvokeAsync("SendNotification", notification);
                connection.StopAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }


    }
}
