using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GPM_AGV_LAT_CORE.Emulators;
using GPM_AGV_LAT_APP.Controllers.GangHaoBotServerController;

namespace GPM_AGV_LAT_APP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmulatorController : ControllerBase
    {

        [HttpGet("TaskDownload")]
        public async Task<IActionResult> TaskDownloadRequest(string taskName, string SID = "001:001:001", string EQName = "AGV_001")
        {
            EmulatorsManager.kingGallentAgvc.TaskDownload(SID, EQName, taskName);
            return Ok();
        }


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
