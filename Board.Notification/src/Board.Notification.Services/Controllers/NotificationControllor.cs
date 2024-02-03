using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;

namespace Board.Notification.Services.Controllers
{

    [ApiController]
    [Route("api/notify")]
    public class NotificationsController : ControllerBase
    {
        // private HubConnection HubConnectionBuilder()
        // {
        //     return
        // }

        [HttpPost]
        public async Task<IActionResult> Notify([FromBody] Notification notification)
        {
            try
            {
                Console.WriteLine("Notification controller reached");
                var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5109/notificationHub")
                .Build();
                await connection.StartAsync();
                Console.WriteLine("Connection started");
                Console.WriteLine(notification.Content);
                Console.WriteLine(notification.ChannelId);
                await connection.InvokeAsync("SendNotification", notification);
                await connection.StopAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine("ERROR FROM NOTIFICATION CONTROLLER: " + e.Message);
                return BadRequest(e.Message);
            }
            return Ok();
        }
        // {
        //     try{
        //         var connection = HubConnectionBuilder();
        //         var notification = new Notification
        //         {
        //             ChannelId = channel,
        //             Content = message
        //         };
        //         connection.StartAsync();
        //         connection.InvokeAsync("SendNotification", notification);
        //         connection.StopAsync();
        //     }
        //     catch (Exception e)
        //     {
        //         return BadRequest(e.Message);
        //     }
        //     return Ok();
        // }


    }
}
