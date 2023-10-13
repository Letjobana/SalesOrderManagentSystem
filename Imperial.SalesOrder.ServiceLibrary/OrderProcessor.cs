using Imperial.SalesOrder.Data;
using Imperial.SalesOrder.DataModel;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imperial.SalesOrder.ServiceLibrary
{
    public class OrderProcessor
    {
        public static void SaveNewOrder(Order order, List<OrderLine> orderLines)
        {
            string orderLinesJson = JsonConvert.SerializeObject(orderLines);
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("OrderNumber", order.OrderNumber),
                new SqlParameter("OrderType", order.OrderType),
                new SqlParameter("OrderStatus", order.OrderStatus),
                new SqlParameter("CustomerName", order.CustomerName),
                new SqlParameter("CreateDate", order.CreateDate),
                new SqlParameter("OrderLinesJson", orderLinesJson)
            };

            using (Repository repo = new Repository())
            {
                repo.ExecuteNonQuery("usp_InsertOrderWithOrderLines", parameters);
            }
        }
        public static List<Order> SearchOrderByOrderNumber(string item)
        {
            List<SqlParameter> lsParms = new List<SqlParameter>()
            {
                new SqlParameter("OrderNumber", item)
            };

            using (Repository repo = new Repository())
            {
                return repo.GetRecords<Order>("usp_SearchOrderByOrderNumber", lsParms);
            }
        }
        public static List<Order> GetOrderDetails()
        {
            using (Repository repo = new Repository())
            {
                return repo.GetRecords<Order>("usp_GetOrderDetails");
            }
        }
        public static List<OrderLine> GetOrderLinesByOrderId(int orderId)
        {
            List<SqlParameter> lsParms = new List<SqlParameter>()
            {
                new SqlParameter("OrderId", orderId)
            };
            using (Repository repo = new Repository())
            {
                return repo.GetRecords<OrderLine>("usp_GetOrderLinesByOrderId",lsParms);
            }
        }
        public static void DeleteOrderLine(int id_OrderLine)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("Id", id_OrderLine)
            };

            using (Repository repo = new Repository())
            {
                repo.ExecuteNonQuery("usp_DeleteOrderLine", sqlParameters);
            }
        }
        public static void DeleteOrder(int id_OrderId)
        {
            List<SqlParameter> sqlParameters = new List<SqlParameter>
            {
                new SqlParameter("OrderId", id_OrderId)
            };

            using (Repository repo = new Repository())
            {
                repo.ExecuteNonQuery("usp_DeleteOrder", sqlParameters);
            }
        }
        public static List<Order> GetOrderByOrderLineId(int item)
        {
            List<SqlParameter> lsParms = new List<SqlParameter>()
            {
                new SqlParameter("OrderLineId",item )
            };

            using (Repository repo = new Repository())
            {
                return repo.GetRecords<Order>("usp_GetOrderByOrderLineId", lsParms);
            }
        }
        public static void UpdateOrderAndOrderLines(int orderId,string orderNumber,string orderType,string orderStatus,string custName,DateTime createDate, List<OrderLine> orderLines)
        {
            string orderLinesJson = JsonConvert.SerializeObject(orderLines);
            List<SqlParameter> parameters = new List<SqlParameter>
            {
                new SqlParameter("OrderId", orderId),
                new SqlParameter("OrderNumber", orderNumber),
                new SqlParameter("OrderType", orderType),
                new SqlParameter("OrderStatus", orderStatus),
                new SqlParameter("CustomerName", custName),
                new SqlParameter("OrderDateTime", createDate),
                new SqlParameter("OrderLinesJson", orderLinesJson)
            };
            using (Repository repo = new Repository())
            {
                repo.ExecuteNonQuery("usp_UpdateOrderAndOrderLines", parameters);
            }
        }
    }
}
