using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GPM_AGV_LAT_CORE.GPMMiddleware.Manergers;
using System.Net.WebSockets;
using GPM_AGV_LAT_APP.Models;

namespace GPM_AGV_LAT_APP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {

        /// <summary>
        /// 取得所有訂單
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(OrderManerger.OrderList);
        }

        [HttpGet("/order")]
        public async Task Subscribe()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                WebSocket websocket = await HttpContext.WebSockets.AcceptWebSocketAsync();

                WebsocketClientManager.SubscribeOrderCLients.Add(websocket);

                Task.Factory.StartNew(() =>
                {
                    websocket.ReceiveAsync(new ArraySegment<byte>(new byte[1024]), CancellationToken.None);
                });

                while (websocket.State == WebSocketState.Open)
                {
                    await Task.Delay(TimeSpan.FromSeconds(1));
                }
                WebsocketClientManager.SubscribeOrderCLients.Remove(websocket);
            }
        }

    }
}
