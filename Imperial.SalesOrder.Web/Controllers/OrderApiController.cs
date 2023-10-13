using Imperial.SalesOrder.ServiceLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace Imperial.SalesOrder.Web.Controllers
{
    public class OrderApiController : ApiController
    {
        [HttpGet]
        public IHttpActionResult GetOrderDetails(string orderNumber)
        {
            var orderDetails = OrderProcessor.SearchOrderByOrderNumber(orderNumber);

            if (orderDetails == null)
            {
                return NotFound();
            }

            return Ok(orderDetails);
        }
    }
}
