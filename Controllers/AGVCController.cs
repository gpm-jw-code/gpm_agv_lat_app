using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GPM_AGV_LAT_CORE.AGVC;

namespace GPM_AGV_LAT_APP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AGVCController : ControllerBase
    {

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
    }
}
