using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GangHaoAGV.Models.Order;
using Newtonsoft.Json;

namespace GPM_AGV_LAT_APP.Controllers.GangHaoBotServerController
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServerController : ControllerBase
    {

        public static List<OrderDetails> orderList = new List<OrderDetails>();

        [HttpPost("setOrder")]
        public async Task<IActionResult> SetOrder(SetOrder setOrder)
        {

            OrderDetails orderDetails = JsonConvert.DeserializeObject<OrderDetails>(JsonConvert.SerializeObject(setOrder));
            DateTime start = new DateTime(1970, 1, 1, 8, 0, 0);
            orderDetails.createTime = Convert.ToInt32((DateTime.Now - start).TotalSeconds);
            orderList.Add(orderDetails);

            return Ok(new ResponseBase()
            {
                code = 0,
                msg = "ok",
                create_on = orderDetails.createTime,
            });
        }

        /// <summary>
        /// 查詢訂單狀態
        /// </summary>
        /// <param name="id">指定訂單 id</param>
        /// <returns></returns>
        [HttpGet("orderDetails/{id}")]
        public async Task<IActionResult> OrderDetails(string id)
        {
            return Ok(orderList.FirstOrDefault(order => order.id == id));
        }


        [HttpPost("addBlocks")]
        public async Task<IActionResult> AddBlocks()
        {
            return Ok();
        }

        /// <summary>
        /// 通知任務封口
        /// </summary>
        /// <returns></returns>
        [HttpPost("markComplete")]
        public async Task<IActionResult> MarkComplete()
        {
            return Ok();
        }
        /// <summary>
        /// 終止任務
        /// </summary>
        /// <returns></returns>
        [HttpPost("terminate")]
        public async Task<IActionResult> Terminate(object terminateObj)
        {
            var name = terminateObj.GetType().Name;
            return Ok(new ResponseBaseWithStringCreateOn()
            {
                code = 0,
                msg = "ok",
                create_on = DateTime.Now.ToString(),//2022-02-10T18:21:25.073Z
            });
        }


        /// <summary>
        /// 分頁查詢訂單
        /// </summary>
        /// <param name="page">頁碼數，如果不傳默認為 1</param>
        /// <param name="size">每頁訂單數量，如果不傳默認為 20</param>
        /// <param name="orderBy">排序的字段，可以為 createTime，terminalTime 和priority，默認為 createTim</param>
        /// <param name="orderMethod">排序方法，可以為 descending 和 ascending，默認為descending 降序</param>
        /// <param name="where">過濾條件，為 JSON 字符串，不傳時不做過濾，詳情見過濾條件</param>
        /// <returns></returns>

        [HttpGet("orders")]
        public async Task<IActionResult> OrderDetails(int? page, int? size, string? orderBy, string? orderMethod, string? where)
        {
            return Ok();
        }


    }
}
