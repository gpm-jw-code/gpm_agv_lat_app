using GPM_AGV_LAT_CORE.GPMMiddleware.Manergers.Order;
using Microsoft.AspNetCore.SignalR;
namespace GPM_AGV_LAT_APP.SingalRHubs
{
    public class AGVSOrderHub : Hub
    {


        public AGVSOrderHub()
        {

        }

        public override Task OnDisconnectedAsync(Exception? exception)
        {
            return base.OnDisconnectedAsync(exception);
        }


        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public async Task SendMessage(string id, string message)
        {
            await Clients.All.SendAsync("ReceiveMessage", id, message);
        }


    }
}
