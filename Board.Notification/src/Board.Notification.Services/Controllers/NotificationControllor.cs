using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Client;

/// <summary>
/// Notifies the clients by sending a notification through SignalR.
/// </summary>
/// <param name="notification">The notification to be sent.</param>
/// <returns>An IActionResult representing the result of the operation.</returns>
namespace Board.Notification.Services.Controllers
{

    [ApiController]
    [Route("api/notify")]
    public class NotificationsController : ControllerBase
    {

        [HttpPost]
        public async Task<IActionResult> Notify([FromBody] Notification notification)
        {
            try
            {
                var connection = new HubConnectionBuilder()
                .WithUrl("http://localhost:5109/notificationHub")
                .Build();
                await connection.StartAsync();
                await connection.InvokeAsync("SendNotification", notification);
                await connection.StopAsync();
            }
            catch (Exception e)
            {
                return BadRequest(e.Message);
            }
            return Ok();
        }


    }
}
