using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GangHaoAGV.AGV;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using GangHaoAGV.Models;
using System.Linq.Expressions;
using GangHaoAGV.Models.MapModels.Requests;

namespace GPM_AGV_LAT_APP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GangHaoAGVCController : ControllerBase
    {

        public GangHaoAGVCController()
        {

        }
        #region STATE 狀態

        /// <summary>
        /// 查詢機器人信息
        /// </summary>
        /// <param name="ip"></param>
        /// <param name="cmdNo"></param>
        /// <returns></returns>
        [HttpGet("robot_status_info_req")]
        public async Task<IActionResult> robot_status_info_req(string ip = "192.168.1.227", ushort cmdNo = 1000)
        {
            return Ok(await GetState(ip, cmdNo));
        }
        /// <summary>
        /// 查詢機器人的運行狀態信息(如運行時間、里程等)
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [HttpGet("robot_status_run_req")]
        public async Task<IActionResult> robot_status_run_req(string ip = "192.168.1.227")
        {
            return Ok(await GetState(ip, 1002));
        }

        /// <summary>
        /// 查詢機器人位置
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [HttpGet("robot_status_loc_req")]
        public async Task<IActionResult> robot_status_loc_req(string ip = "192.168.1.227")
        {
            return Ok(await GetState(ip, 1004));
        }

        /// <summary>
        /// 查詢機器人速度
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [HttpGet("robot_status_speed_req")]
        public async Task<IActionResult> robot_status_speed_req(string ip = "192.168.1.227")
        {
            return Ok(await GetState(ip, 1005));
        }

        /// <summary>
        /// 查詢機器人被阻擋狀態
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [HttpGet("robot_status_block_req")]
        public async Task<IActionResult> robot_status_block_req(string ip = "192.168.1.227")
        {
            return Ok(await GetState(ip, 1006));
        }
        /// <summary>
        /// 查詢機器人電池狀態
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [HttpGet("robot_status_battery_req")]
        public async Task<IActionResult> robot_status_battery_req(string ip = "192.168.1.227")
        {
            return Ok(await GetState(ip, 1007));
        }
        /// <summary>
        /// 查詢機器人激光點雲數據
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [HttpGet("robot_status_laser_req")]
        public async Task<IActionResult> robot_status_laser_req(string ip = "192.168.1.227")
        {
            return Ok(await GetState(ip, 1009));
        }
        /// <summary>
        /// 查詢機器人路徑數據
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [HttpGet("robot_status_path_req")]
        public async Task<IActionResult> robot_status_path_req(string ip = "192.168.1.227")
        {
            return Ok(await GetState(ip, 1010));
        }

        /// <summary>
        /// 取得狀態數據JSON字串
        /// </summary>
        /// <param name="ip">AGVC IP</param>
        /// <param name="cmdNo">Command ID</param>
        /// <returns>JSON字串</returns>
        [HttpGet("StateQuery")]
        public async Task<string> GetState(string ip = "192.168.1.227", ushort cmdNo = 1000)
        {
            AgvcIni(ip, out cAGV agv);
            if (!agv.StatesPortConnected)
                return "Disconnected";
            return await agv.STATES.API.GetStateJsonResponse(cmdNo, saveAsJsonFile: true);
        }

        #endregion

        #region Control

        /// <summary>
        /// 重定位 
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [HttpPost("robot_control_reloc_req")]
        public async Task<object> robot_control_reloc_req(string ip = "192.168.1.227")
        {
            AgvcIni(ip, out cAGV agv);
            if (!agv.StatesPortConnected)
                return "Disconnected";
            return await agv.CONTROL.ControlAPI.ReLoc();
        }
        /// <summary>
        /// 確認定位正確 
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        [HttpPost("robot_control_comfirmloc_req")]
        public async Task<object> robot_control_comfirmloc_req(string ip = "192.168.1.227")
        {
            AgvcIni(ip, out cAGV agv);
            if (!agv.StatesPortConnected)
                return "Disconnected";
            return await agv.CONTROL.ControlAPI.ConfirmLoc();
        }

        #endregion

        #region 導航 Navigation

        /// <summary>
        /// 取消當前導航
        /// </summary>
        /// <returns></returns>
        [HttpPost("robot_task_cancel_req")]
        public async Task<object> robot_task_cancel_req(string ip = "192.168.1.227")
        {
            AgvcIni(ip, out cAGV agv);
            if (!agv.MapPortConnected)
                return "Disconnected";
            return await agv.NAVIGATIOR.mapAPI.TaskCancel();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="target"></param>
        /// <param name="ip"></param>
        /// <returns></returns>
        [HttpPost("robot_task_gotarget_req")]
        public async Task<object> robot_task_gotarget_req([FromBody] robotMapTaskGoTargetReq_3051 target, string ip = "192.168.1.227")
        {
            AgvcIni(ip, out cAGV agv);
            if (!agv.MapPortConnected)
                return "Disconnected";
            return await agv.NAVIGATIOR.mapAPI.GoTarget(target);
        }
        #endregion

        [HttpPost("PauseNavigate")]
        public async Task PauseNavigate(string ip)
        {
            if (AgvcIni(ip, out cAGV agv))
            {
                await agv.NAVIGATIOR.PauseNavigate();
            }
        }

        private bool AgvcIni(string ip, out cAGV agv)
        {
            agv = new cAGV(ip, autoFetchStateData: false);
            return true;
        }



    }
}
