using GPM_AGV_LAT_CORE.Emulators;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static GangHaoAGV.Models.StateModels.Responses.robotStatusRelocRes_11021;

namespace GPM_AGV_LAT_APP.Controllers.EmulatorControllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GangHaoAGVCEmulatorController : ControllerBase
    {

        [HttpPost("ReLocStateEmulate")]
        public async Task ReLocState(string IP, RELOC_STATE state)
        {
            var agvemu = EmulatorsManager.gangHaoAgvcList.FirstOrDefault(gagv => gagv.IP == IP);
            if (agvemu != null)
            {
                agvemu.stateEmulator.relocState = state;
            }
        }
    }
}
