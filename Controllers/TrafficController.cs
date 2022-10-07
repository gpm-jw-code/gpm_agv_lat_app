using GPM_AGV_LAT_CORE.GPMMiddleware.TrafficControl;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GPM_AGV_LAT_APP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrafficController : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            return Ok(TrafficControlCenter.ControlingStation.ToList().FindAll(cs => cs.Value.inControlingAgvcStateList.Count >= 2));
        }

    }
}
