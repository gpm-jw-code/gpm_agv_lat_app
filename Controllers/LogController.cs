using GPM_AGV_LAT_APP.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;

namespace GPM_AGV_LAT_APP.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogController : ControllerBase
    {
        [HttpGet("/log")]
        public async Task Get()
        {
            await WebsocketClientManager.CreatLogConnection(HttpContext);
        }

        [HttpGet("/messageHsLog")]
        public async Task MessageHandShakeLog()
        {
            await WebsocketClientManager.CreateMessageHSLogConnection(HttpContext);

        }

    }
}
