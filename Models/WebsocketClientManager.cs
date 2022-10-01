using GPM_AGV_LAT_CORE.GPMMiddleware;
using GPM_AGV_LAT_CORE.GPMMiddleware.Manergers.Order;
using GPM_AGV_LAT_CORE.Logger;
using System.Net.WebSockets;
using System.Text;

namespace GPM_AGV_LAT_APP.Models
{
    public class WebsocketClientManager
    {
        public enum GROUP
        {
            Order, Log, MessageHSLog
        }

        public static List<WebSocket> SubscribeOrderCLients = new List<WebSocket>();
        public static List<WebSocket> SubscribeLogCLients = new List<WebSocket>();
        public static List<WebSocket> SubscribeMessageHandShakeLogCLients = new List<WebSocket>();


        internal static void BrocastOrder(object? sender, clsHostExecuting newOrder)
        {
            Brocast(SubscribeOrderCLients, System.Text.Json.JsonSerializer.Serialize(newOrder));
        }

        internal static void BrocastLog(object? sender, LoggerInstance.LogItem logItem)
        {
            Brocast(SubscribeLogCLients, System.Text.Json.JsonSerializer.Serialize(logItem));
        }

        internal static void BrocastHandShakeLog(object? sender, MessageHandShakeLogger.HandshakeLogItem logItem)
        {
            Brocast(SubscribeMessageHandShakeLogCLients, System.Text.Json.JsonSerializer.Serialize(logItem));
        }


        internal static async Task CreatLogConnection(HttpContext context)
        {
            await CreateWebSocketConnection(context, GROUP.Log);
        }
        internal static async Task CreateMessageHSLogConnection(HttpContext context)
        {
            await CreateWebSocketConnection(context, GROUP.MessageHSLog);
        }


        private static async Task CreateWebSocketConnection(HttpContext context, GROUP group)
        {
            await Task.Run(async () =>
            {

                if (context.WebSockets.IsWebSocketRequest)
                {
                    var websocketsGroup = GetClientsGroup(group);
                    WebSocket websocket = await context.WebSockets.AcceptWebSocketAsync();

                    websocketsGroup.Add(websocket);

                    Task.Factory.StartNew(() =>
                    {
                        websocket.ReceiveAsync(new ArraySegment<byte>(new byte[1024]), CancellationToken.None);
                    });

                    while (websocket.State == WebSocketState.Open)
                    {
                        await Task.Delay(TimeSpan.FromSeconds(1));
                    }
                    websocketsGroup.Remove(websocket);
                }
            });
        }

        private static List<WebSocket> GetClientsGroup(GROUP group)
        {
            switch (group)
            {
                case GROUP.Order:
                    return SubscribeOrderCLients;
                case GROUP.Log:
                    return SubscribeLogCLients;
                case GROUP.MessageHSLog:
                    return SubscribeMessageHandShakeLogCLients;
                default:
                    return new List<WebSocket>();
            }
        }

        /// <summary>
        /// 廣播訊息
        /// </summary>
        /// <param name="clients"></param>
        /// <param name="message"></param>
        private static void Brocast(List<WebSocket> clients, string message)
        {
            byte[] message_byte = Encoding.ASCII.GetBytes(message);
            foreach (var client in clients)
            {
                if (client == null)
                    continue;
                if (client.State != WebSocketState.Open)
                    continue;
                client.SendAsync(new ArraySegment<byte>(message_byte), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }


    }
}
