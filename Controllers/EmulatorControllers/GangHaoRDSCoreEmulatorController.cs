using GPM_AGV_LAT_APP.Controllers.GangHaoBotServerController;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GPM_AGV_LAT_APP.Controllers.EmulatorControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GangHaoRDSCoreEmulatorController : ControllerBase
    {
        [HttpGet("GangHaoOrderStateChange")]
        public async Task<IActionResult> OrderStateChange(string orderID, string State)
        {
            var index = ServerController.orderList.FindIndex(order => order.id == orderID);
            if (index != -1)
                ServerController.orderList[index].state = State;
            return Ok();
        }
    }
}
