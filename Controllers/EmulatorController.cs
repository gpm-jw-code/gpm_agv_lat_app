using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GPM_AGV_LAT_CORE.Emulators;
namespace GPM_AGV_LAT_APP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmulatorController : ControllerBase
    {

        [HttpGet("TaskDownload")]
        public async Task<IActionResult> TaskDownloadRequest(string SID = "001:001:001", string EQName = "AGV_001")
        {
            EmulatorsManager.kingGallentAgvc.TaskDownload(SID, EQName);
            return Ok();
        }
    }
}
