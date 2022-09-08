using GangHaoAGV.Models.Order;
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
            var order = GetOrderByOrderID(orderID);
            if (order != null)
                order.state = State.ToUpper();
            return Ok(order);
        }

        [HttpGet("OrderRunningEmulate")]
        public async Task<IActionResult> OrderRunningEmulate(string orderID)
        {
            var order = GetOrderByOrderID(orderID);
            if (order != null)
                order.state = "RUNNING";
            return Ok(order);
        }

        [HttpGet("OrderFinishedEmulate")]
        public async Task<IActionResult> OrderFinishedEmulate(string orderID)
        {
            var order = GetOrderByOrderID(orderID);
            if (order != null)
                order.state = "FINISHED";
            return Ok(order);
        }


        private OrderDetails GetOrderByOrderID(string orderID)
        {
            var index = ServerController.orderList.FindIndex(order => order.id == orderID);
            if (index != -1)
                return ServerController.orderList[index];
            else
                return null;
        }
    }
}
