using Imperial.SalesOrder.DataModel;
using Imperial.SalesOrder.ServiceLibrary;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using System.Web;

namespace Imperial.SalesOrder.Web.Helpers
{
    public class SelectLists
    {
        public static SelectList GetOrderType(int selected)
        {
            List<GenericLookup> ls = new List<GenericLookup>
            {
                new GenericLookup{ Id = 0, Value = "- Select Order Type -" }
            };
            ls.AddRange(DataProcessor.GetLookupsByType("OType"));

            return new SelectList(ls, "Id", "Value", selected);
        }
        public static SelectList GetOrderStatus(int selected)
        {
            List<GenericLookup> ls = new List<GenericLookup>
            {
                new GenericLookup{ Id = 0, Value = "- Select Order Status -" }
            };
            ls.AddRange(DataProcessor.GetLookupsByType("Ostatus"));

            return new SelectList(ls, "Id", "Value", selected);
        }
        public static SelectList GetProductType(int selected)
        {
            List<GenericLookup> ls = new List<GenericLookup>
            {
                new GenericLookup{ Id = 0, Value = "- Select Product Type -" }
            };
            ls.AddRange(DataProcessor.GetLookupsByType("PType"));

            return new SelectList(ls, "Id", "Value", selected);
        }
    }
}