using Imperial.SalesOrder.DataModel;
using Imperial.SalesOrder.ServiceLibrary;
using Imperial.SalesOrder.Web.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Imperial.SalesOrder.Web.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            // Get a list of orders using the OrderProcessor
            List<Order>orders = OrderProcessor.GetOrderDetails();
            return View(orders);
        }
        public ActionResult AddOrder()
        {
            Order model = new Order();
            model.OrderLines = new List<OrderLine>();
            // Load lookup values for dropdown lists
            LoadLookups(model);
            return View(model);
        }
        [HttpPost]
        public ActionResult AddOrder(Order order, List<OrderLine> orderLines)
        {
            ModelState.Clear();
            // Validate the order and order lines
            List<string> validationErrors = ValidateOrder(order, orderLines);

            if (validationErrors.Count > 0)
            {
                // Store validation errors in session
                Session["ValidationErrors"] = validationErrors;

                LoadLookups(order);
                return View(order);
            }

            // Retrieve and set order type and order status values
            var orderType = DataProcessor.GetLookUpValue(order.Id_OrderType);
            var orderStatus = DataProcessor.GetLookUpValue(order.Id_OrderStatus);

            if (orderType != null)
                order.OrderType = orderType.Value;
            if (orderStatus != null)
                order.OrderStatus = orderStatus.Value;

            OrderProcessor.SaveNewOrder(order, orderLines);

            TempData["SuccessMessage"] = "Order has been saved successfully!!";
            return RedirectToAction("AddOrder");
        }


        public ActionResult GetOrderByOrderId(int orderId)
        {
            List<OrderLine> orderLines =OrderProcessor.GetOrderLinesByOrderId(orderId);
            return PartialView("_OrderLines", orderLines);
        }
        public ActionResult DeleteOrderLine(int orderLineId)
        {
            try
            {
                OrderProcessor.DeleteOrderLine(orderLineId);
                TempData["SuccessMessage"] = "Order line deleted successfully";
                return Json(new { success = true, message = "Order line deleted successfully" });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to delete order line: " + ex.Message;
                return Json(new { success = false, message = "Error deleting order line: " + ex.Message });
            }

        }
        public ActionResult DeleteOrder(int orderId)
        {
            try
            {
                OrderProcessor.DeleteOrder(orderId);
                TempData["SuccessMessage"] = "Order deleted successfully";
                return Json(new { success = true, message = "Order deleted successfully" });
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Failed to delete order: " + ex.Message;
                return Json(new { success = false, message = "Error deleting order: " + ex.Message });
            }

        }
        public ActionResult EditOrderAndLines(int orderId)
        {            
            var result =OrderProcessor.GetOrderByOrderLineId(orderId);

            Order model = new Order();
            model.OrderLines = new List<OrderLine>();

            foreach (var item in result)
            {
                if (!string.IsNullOrEmpty(item.OrderLinesJson))
                {
                    // Deserialize the JSON data into the list of order lines
                    model.OrderLines = JsonConvert.DeserializeObject<List<OrderLine>>(item.OrderLinesJson);
                }
                model.OrderNumber = item.OrderNumber;
                model.CustomerName = item.CustomerName;
                model.OrderType = item.OrderType;
                model.OrderStatus = item.OrderStatus;
                model.CreateDate = item.CreateDate;
            }
           
            return View(model);
        }
        [HttpPost]
        public ActionResult EditOrderAndLines(Order order, List<OrderLine> orderLines)
        {
            ModelState.Clear();
            List<string> validationErrors = ValidateOrder(order, orderLines);

            if (validationErrors.Count > 0)
            {
                // Store validation errors in session
                Session["ValidationErrors"] = validationErrors;

                LoadLookups(order);
                return View(order);
            }
            OrderProcessor.UpdateOrderAndOrderLines(order.OrderId, order.OrderNumber, order.OrderType, order.OrderStatus, order.CustomerName, order.CreateDate, orderLines);
            TempData["SuccessMessage"] = "Order has been updated successfully!!";
            return RedirectToAction("EditOrderAndLines", new { orderId = order.OrderId });
        }
        private void LoadLookups(Order model)
        {
            ViewBag.OrderType = Helpers.SelectLists.GetOrderType(model.Id_OrderType);
            ViewBag.OrderStatus = Helpers.SelectLists.GetOrderStatus(model.Id_OrderStatus);
            ViewBag.ProductType = Helpers.SelectLists.GetProductType(model.Id_ProductCode);
        }
        private List<string> ValidateOrder(Order order, List<OrderLine> orderLines)
        {
            List<string> validationErrors = new List<string>();

            if (string.IsNullOrEmpty(order.OrderNumber))
            {
                validationErrors.Add("Order Number is required.");
            }
            if (string.IsNullOrEmpty(order.OrderType) && order.Id_OrderType == 0)
            {
                validationErrors.Add("Order Number is required.");
            }
            if (string.IsNullOrEmpty(order.OrderStatus) && order.Id_OrderStatus == 0)
            {
                validationErrors.Add("Order Status is required.");
            }
            if (string.IsNullOrEmpty(order.CustomerName))
            {
                validationErrors.Add("Customer Name is required.");
            }
            if (orderLines == null || !orderLines.Any())
            {
                validationErrors.Add("An order cannot be saved without at least one order line. Please add an order line.");
            }

            if (orderLines != null)
            {
                for (int i = 0; i < orderLines.Count; i++)
                {
                    var orderLine = orderLines[i];

                    if (string.IsNullOrEmpty(orderLine.ProductCode))
                    {
                        validationErrors.Add($"Product Code is required for Order Line {i + 1}.");
                    }

                    if (string.IsNullOrEmpty(orderLine.ProductType))
                    {
                        validationErrors.Add($"Product Type is required for Order Line {i + 1}.");
                    }

                    if (orderLine.ProductCostPrice <= 0)
                    {
                        validationErrors.Add($"Cost Price must be greater than zero for Order Line {i + 1}.");
                    }

                    if (orderLine.ProductSalesPrice <= 0)
                    {
                        validationErrors.Add($"Sales Price must be greater than zero for Order Line {i + 1}.");
                    }

                    if (orderLine.Quantity <= 0)
                    {
                        validationErrors.Add($"Quantity must be greater than zero and must be a number for Order Line {i + 1}.");
                    }
                }
            }

            return validationErrors;
        }

    }
}