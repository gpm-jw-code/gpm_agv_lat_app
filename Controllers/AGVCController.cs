using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GPM_AGV_LAT_CORE.AGVC;
using static GangHaoAGV.Models.StateModels.Responses.robotStatusRelocRes_11021;

namespace GPM_AGV_LAT_APP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AGVCController : ControllerBase
    {

        [HttpGet("AGVCParams")]
        public async Task<IActionResult> GetAGVCParams()
        {
            return Ok(AGVCManager.AGVCList.Select(agv => agv.agvcParameters));
        }

        [HttpGet("States")]
        public async Task<IActionResult> States()
        {
            return Ok(AGVCManager.AGVCList);
        }

        [HttpGet("State")]
        public async Task<IActionResult> StatesByID(string agvc_id)
        {
            return Ok(AGVCManager.AGVCList.FirstOrDefault(agv => agv.ID == agvc_id));
        }

        [HttpGet("AgvTypes")]
        public async Task<IActionResult> TypeQuery()
        {
            return Ok(AGVCManager.AGVCList.ToDictionary(agv => agv.ID, agv => agv.agvcType.ToString()));
        }


        [HttpGet("list")]
        public async Task<IActionResult> GetList()
        {
            return Ok(AGVCManager.AGVCList.ToDictionary(agv => agv.ID, agv => new Dictionary<string, object>()
            {
                { "EQName", agv.EQName },
                { "Type", agv.agvcType.ToString() },
                { "Connected", agv.agvcStates.States.EConnectionState== GPM_AGV_LAT_CORE.AGVC.AGVCStates.CONNECTION_STATE.CONNECTED}
            }).ToArray());
        }


        /// <summary>
        /// 取得AGVC IP連線列表
        /// </summary>
        /// <returns></returns>
        [HttpGet("IPList")]
        public async Task<Dictionary<string, string>> GetAgvcIPList()
        {
            return AGVCManager.AGVCList.ToDictionary(agv => agv.EQName, agv => agv.agvcParameters.tcpParams.HostIP);
        }

        [HttpGet("GetNativeData")]
        public async Task<dynamic> GetAgvcNativeData(string eqName)
        {
            return await AGVCManager.GetAgvcNativeDataByEqName(eqName);
        }


        [HttpGet("GetAlarmState")]
        public async Task<IActionResult> GetAlarmState(string eqName)
        {
            return Ok(await AGVCManager.GetAlarmStateByEqName(eqName));
        }

        [HttpPost("Reloc")]
        public async Task Reloc(string eqName)
        {
            var agv = AGVCManager.AGVCList.FirstOrDefault(agv => agv.EQName == eqName);
            if (agv != null)
            {
                if (agv.agvcType == GPM_AGV_LAT_CORE.LATSystem.AGVC_TYPES.GangHau)
                {

                    GangHaoAGVC gagv = agv as GangHaoAGVC;
                    await gagv.AGVInterface.CONTROL.Reloc();
                }
            }
        }


        [HttpPost("OnlineStateSwitch")]
        public async Task<IActionResult> ONlineStateSwitch(string eqName, int onlineMode, int currentStation)
        {
            Console.WriteLine("使用者要求 AGV-{0}在站點{1} 進行 {2}", eqName, currentStation, onlineMode == 0 ? "下線" : "上線");
            var agv = AGVCManager.AGVCList.FirstOrDefault(agv => agv.EQName == eqName);
            var currentOnlineMode = -1;
            if (agv != null)
            {
                currentOnlineMode = await agv.OnlineStateSwitchInvoke(onlineMode, currentStation);
            }

            return Ok(currentOnlineMode);
        }

    }
}
