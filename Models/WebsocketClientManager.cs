using GPM_AGV_LAT_CORE.GPMMiddleware.Manergers.Order;
using System.Net.WebSockets;
using System.Text;

namespace GPM_AGV_LAT_APP.Models
{
    public class WebsocketClientManager
    {

        public static List<WebSocket> SubscribeOrderCLients = new List<WebSocket>();

        internal static void BrocastOrder(object? sender, clsHostOrder newOrder)
        {
            Brocast(SubscribeOrderCLients, System.Text.Json.JsonSerializer.Serialize(newOrder));
        }


        private static void Brocast(List<WebSocket> clients, string message)
        {
            byte[] message_byte = Encoding.ASCII.GetBytes(message);
            foreach (var client in clients)
            {
                if (client.State != WebSocketState.Open)
                    continue;
                client.SendAsync(new ArraySegment<byte>(message_byte), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }
    }
}
