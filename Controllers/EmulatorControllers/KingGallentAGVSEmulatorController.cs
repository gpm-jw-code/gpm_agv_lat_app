using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GPM_AGV_LAT_CORE.Emulators;
using GPM_AGV_LAT_APP.Controllers.GangHaoBotServerController;

namespace GPM_AGV_LAT_APP.Controllers.EmulatorControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class KingGallentAGVSEmulatorController : ControllerBase
    {

        [HttpGet("TaskDownload")]
        public async Task<IActionResult> TaskDownloadRequest(string taskName, string SID = "001:001:001", string EQName = "AGV_001")
        {
            EmulatorsManager.kingGallentAgvc.TaskDownload(SID, EQName, taskName);
            return Ok();
        }

        [HttpGet("AGVSReset")]
        public async Task<IActionResult> AGVSReset(int resetMode = 0, string SID = "001:001:001", string EQName = "AGV_001")
        {
            EmulatorsManager.kingGallentAgvc.AGVSReset(SID, EQName, resetMode);
            return Ok();
        }

    }
}
