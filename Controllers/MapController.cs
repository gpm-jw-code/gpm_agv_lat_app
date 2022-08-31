using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GPM_AGV_LAT_CORE.AGVC;
using System.Text;

namespace GPM_AGV_LAT_APP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MapController : ControllerBase
    {
        [HttpGet("/map")]
        public async Task Get(string agv_id)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                List<IAGVC> agvList = new List<IAGVC>();
                if (agv_id != "all")
                {
                    var agv = AGVCManager.AGVCList.FirstOrDefault(agv => agv.ID == agv_id);
                    if (agv == null)
                        return;
                    agvList.Add(agv);
                }
                else
                {
                    agvList = AGVCManager.AGVCList;
                }

                var wsClient = await HttpContext.WebSockets.AcceptWebSocketAsync();
                _ = Task.Factory.StartNew(() => wsClient.ReceiveAsync(new ArraySegment<byte>(new byte[128]),
                    CancellationToken.None));

                while (wsClient.State == System.Net.WebSockets.WebSocketState.Open)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(200));
                    byte[] data = Encoding.ASCII.GetBytes(System.Text.Json.JsonSerializer.Serialize(agvList.ToDictionary(agv => agv.ID, agv => agv.agvcStates)));
                    await wsClient.SendAsync(data, System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);
                }

            }
        }

        [HttpGet("PositionSim")]
        public async Task PositionSim(string agv_id, double x, double y)
        {
            IAGVC? agv = AGVCManager.AGVCList.FirstOrDefault(agv => agv.ID == agv_id);
            if (agv == null)
                return;
            agv.agvcStates.MapStates.globalCoordinate.x = x;
            agv.agvcStates.MapStates.globalCoordinate.y = y;
        }


        [HttpGet("PositionInitializeSim")]
        public async Task PositionInitializeSim(string agv_id)
        {
            IAGVC? agv = AGVCManager.AGVCList.FirstOrDefault(agv => agv.ID == agv_id);
            if (agv == null)
                return;
            agv.agvcStates.MapStates.globalCoordinate.x = 0;
            agv.agvcStates.MapStates.globalCoordinate.y = 0;
            agv.agvcStates.MapStates.globalCoordinate.r = 0;
        }


        [HttpGet("RotationSim")]
        public async Task PositionInitializeSim(string agv_id = "0001", double r = 90)
        {
            IAGVC? agv = AGVCManager.AGVCList.FirstOrDefault(agv => agv.ID == agv_id);
            if (agv == null)
                return;
            bool isTurnRight = agv.agvcStates.MapStates.globalCoordinate.r < r;
            Task.Run(() =>
            {

                while (isTurnRight ? agv.agvcStates.MapStates.globalCoordinate.r <= r : agv.agvcStates.MapStates.globalCoordinate.r >= r)
                {
                    agv.agvcStates.MapStates.globalCoordinate.r += isTurnRight ? 0.1 : -0.1;
                    Thread.Sleep(TimeSpan.FromMilliseconds(0.8));
                }
            });
        }
    }
}
