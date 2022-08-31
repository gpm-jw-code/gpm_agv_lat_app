using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GPM_AGV_LAT_CORE.AGVC;

namespace GPM_AGV_LAT_APP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AGVCController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(GPM_AGV_LAT_CORE.Startup.agv1.STATES._conn.connected);
        }

        [HttpGet("States")]
        public async Task<IActionResult> States()
        {
            return Ok(AGVCManager.AGVCList);
        }

        [HttpGet("AgvTypes")]
        public async Task<IActionResult> TypeQuery()
        {
            return Ok(AGVCManager.AGVCList.ToDictionary(agv => agv.ID, agv => agv.agvcType.ToString()));
        }
    }
}
