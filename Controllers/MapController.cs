using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GPM_AGV_LAT_CORE.AGVC;
using System.Text;
using static GPM_AGV_LAT_CORE.AGVC.AGVCStates.MapState;

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
                    byte[] data = Encoding.ASCII.GetBytes(System.Text.Json.JsonSerializer.Serialize(agvList.ToDictionary(agv => agv.EQName, agv => agv.agvcStates)));
                    await wsClient.SendAsync(data, System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);
                }

            }
        }

        [HttpGet("/map_agvc")]
        public async Task GetAGVCByMapName(string mapName)
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                List<IAGVC> agvList = AGVCManager.GetAGVCByMapName(mapName);
                var wsClient = await HttpContext.WebSockets.AcceptWebSocketAsync();
                _ = Task.Factory.StartNew(() => wsClient.ReceiveAsync(new ArraySegment<byte>(new byte[128]),
                    CancellationToken.None));

                while (wsClient.State == System.Net.WebSockets.WebSocketState.Open)
                {
                    await Task.Delay(TimeSpan.FromMilliseconds(400));
                    byte[] data = Encoding.ASCII.GetBytes(System.Text.Json.JsonSerializer.Serialize(agvList.ToDictionary(agv => agv.EQName, agv => agv.agvcStates)));
                    await wsClient.SendAsync(data, System.Net.WebSockets.WebSocketMessageType.Text, true, CancellationToken.None);
                }

            }
        }

        /// <summary>
        /// 取得所有地圖名稱
        /// </summary>
        /// <returns></returns>
        [HttpGet("GetMapNames")]
        public async Task<List<string>> GetMapNames()
        {
            return AGVCManager.GetMapNames();
        }

        [HttpGet("GetMapInfo")]
        public async Task<MapInfo> GetMapInfo(string mapName)
        {
            List<IAGVC>? agvcList = AGVCManager.GetAGVCByMapName(mapName);
            List<StationInfo> stations = new List<StationInfo>();
            foreach (var agvc in agvcList)
            {
                var _stations = agvc.agvcStates.MapStates.currentMapInfo.stations;
                foreach (var _station in _stations)
                {
                    if (stations.FirstOrDefault(s => s.id == _station.id) == null)
                        stations.Add(_station);
                }
            }
            return new MapInfo() { stations = stations.Distinct().ToList(), name = mapName, mapFileUrl = "http://192.168.0.104:7122/map/map1.png" };
        }

        [HttpGet("GetMapInfos")]
        public async Task<List<MapInfo>> GetMapInfos()
        {
            List<string>? mapNames = await GetMapNames();
            return mapNames.Select(name => GetMapInfo(name).Result).ToList();
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
