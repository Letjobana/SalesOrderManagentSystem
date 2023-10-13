using Imperial.SalesOrder.DataModel;
using Imperial.SalesOrder.ServiceLibrary;
using System;
using System.Collections.Generic;
using Xunit;

namespace Imperial.SalesOrder.UnitTest
{
    public class HomeControllerTests
    {
        [Fact]
        public void SaveNewOrder_Should_Save_Order()
        {
            // Arrange
            Order order = new Order
            {
                OrderNumber = "SO625145",
                OrderType = "Normal",
                OrderStatus = "New",
                CustomerName = "Test",
                CreateDate = DateTime.Now
            };

            List<OrderLine> orderLines = new List<OrderLine>
            {
                new OrderLine { ProductCode = ">GSX837422",ProductType = "Parts", ProductCostPrice = 10.00m, Quantity = 2},
                new OrderLine { ProductCode = ">GSX837421",ProductType = "Parts",ProductSalesPrice = 15.99m, Quantity = 3}
            };

            // Act
            OrderProcessor.SaveNewOrder(order, orderLines);

            // Assert
            List<Order> savedOrders = OrderProcessor.GetOrderDetails();
            Assert.NotEmpty(savedOrders);
        }
        [Fact]
        public void SearchOrderByOrderNumber_Should_Return_Correct_Order()
        {
            // Arrange
            string orderNumberToSearch = "SO625145";

            // Act
            List<Order> foundOrders = OrderProcessor.SearchOrderByOrderNumber(orderNumberToSearch);

            // Assert
            Assert.NotNull(foundOrders);
            Assert.NotEmpty(foundOrders);
            Assert.Single(foundOrders); 
            Assert.Equal(orderNumberToSearch, foundOrders[0].OrderNumber);
        }
    }
}
