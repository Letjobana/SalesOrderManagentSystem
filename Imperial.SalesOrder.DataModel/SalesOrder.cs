using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Imperial.SalesOrder.DataModel
{
    public class Order
    {
        public int OrderId { get; set; }
        [Display(Name = "Select Type")]
        public int Id_OrderType { get; set; }
        [Display(Name = "Select Status")]
        public int Id_OrderStatus { get; set; }
        public int Id_ProductCode { get; set; }
        [Display(Name = "Order Number")]
        public string OrderNumber { get; set; }
        [Display(Name = "Order Type")]
        public string OrderType { get; set; }
        [Display(Name = "Order Status")]
        public string OrderStatus { get; set; }
        [Display(Name = "Customer Name")]
        public string CustomerName { get; set; }
        [Display(Name = "Date Created")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy HH:mm:ss tt}")]
        public DateTime CreateDate { get; set; }
        public string OrderLinesJson { get; set; }
        public List<OrderLine> OrderLines { get; set; }
        public string SearchItem { get; set; }
        public Decimal Test { get; set; }
    }

    public class OrderLine
    {
        public int Id { get; set; }
        [Display(Name = "Product Code")]
        public string ProductCode { get; set; }
        [Display(Name = "Product Type")]
        public string ProductType { get; set; }
        [Display(Name = "Product Cost Price")]
        public decimal ProductCostPrice { get; set; }
        [Display(Name = "Product Sales Price")]
        public decimal ProductSalesPrice { get; set; }
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
    }


}
