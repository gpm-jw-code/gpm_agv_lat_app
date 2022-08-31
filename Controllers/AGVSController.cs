using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GPM_AGV_LAT_CORE.LATSystem;
using GPM_AGV_LAT_CORE.AGVS;

namespace GPM_AGV_LAT_APP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AGVSController : ControllerBase
    {
        [HttpGet("Settings")]
        public async Task<IActionResult> Settings()
        {
            return Ok(AGVSManager.CurrentAGVS);
        }

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(AGVSManager.CurrentAGVS);
        }
    }
}
