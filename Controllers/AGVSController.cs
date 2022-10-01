using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GPM_AGV_LAT_CORE.LATSystem;
using GPM_AGV_LAT_CORE.AGVS;
using GPM_AGV_LAT_APP.ViewModels;
using GPM_AGV_LAT_CORE.GPMMiddleware;

namespace GPM_AGV_LAT_APP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AGVSController : ControllerBase
    {
        [HttpGet("Settings")]
        public async Task<IActionResult> Settings()
        {
            AgvsInfo agvsInfo = new AgvsInfo
            {
                VenderName = AGVSManager.CurrentAGVS.VenderName,
                ConnectionString = AGVSManager.CurrentAGVS.agvsParameters.tcpParams.Host
            };
            return Ok(agvsInfo);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(AGVSManager.CurrentAGVS);
        }

        [HttpGet("ExeTaskList")]
        public async Task<List<clsHostExecuting>> ExeTaskList()
        {
            return AGVSManager.CurrentAGVS.ExecuteTaskList;
        }
    }
}
