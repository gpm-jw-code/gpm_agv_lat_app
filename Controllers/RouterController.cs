using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GPM_AGV_LAT_APP.Controllers
{
    [Route("/")]
    [ApiController]
    public class RouterController : ControllerBase
    {

        [HttpGet("Orders")]
        public async Task<ContentResult> Orders()
        {
            return StaticHtmlContent();
        }

        [HttpGet("agvc")]
        public async Task<ContentResult> agvc()
        {
            return StaticHtmlContent();
        }

        [HttpGet("logview")]
        public async Task<ContentResult> log()
        {
            return StaticHtmlContent();
        }

        [HttpGet("map")]
        public async Task<ContentResult> map(string agv_id)
        {
            return StaticHtmlContent();
        }


        [HttpGet("map_view")]
        public async Task<ContentResult> map_view()
        {
            return StaticHtmlContent();
        }

        [HttpGet("agvs")]
        public async Task<ContentResult> agvs()
        {
            return StaticHtmlContent();
        }

        private ContentResult StaticHtmlContent()
        {
            var html = System.IO.File.ReadAllText("wwwroot/index.html");
            //var html = System.IO.File.ReadAllText("htmlpage.html");
            return base.Content(html, "text/html");
        }

    }
}
