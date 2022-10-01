using GPM_AGV_LAT_CORE.GPMMiddleware.Manergers.Order;
using GPM_AGV_LAT_CORE.LATSystem.Dispatch;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GPM_AGV_LAT_APP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LATDispatchController : ControllerBase
    {

        [HttpPost("TaskDispatch")]
        public async Task<IActionResult> TaskDispatch(clsLATTaskOrder latOrder)
        {
            AgvcDisPatcher.TaskDispatch(latOrder);
            return Ok();
        }

    }
}
