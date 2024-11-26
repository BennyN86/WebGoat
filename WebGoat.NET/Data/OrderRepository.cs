using WebGoatCore.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Globalization;
using System.Threading;

namespace WebGoatCore.Data
{
    public class OrderRepository
    {
        private readonly NorthwindContext _context;
        private readonly CustomerRepository _customerRepository;

        public OrderRepository(NorthwindContext context, CustomerRepository customerRepository)
        {
            _context = context;
            _customerRepository = customerRepository;
        }

        public Order GetOrderById(int orderId)
        {
            return _context.Orders.Single(o => o.OrderId == orderId);
        }

        public int CreateOrder(Order order)
        {
            Thread.CurrentThread.CurrentCulture = new CultureInfo("en-US");

            // Prepare the shippedDate value
            string shippedDate = order.ShippedDate.HasValue ? order.ShippedDate.Value.ToString("yyyy-MM-dd") : null;

            // Use parameterized query for the Orders insertion
            var sql = "INSERT INTO Orders (" +
                "CustomerId, EmployeeId, OrderDate, RequiredDate, ShippedDate, ShipVia, Freight, ShipName, ShipAddress, " +
                "ShipCity, ShipRegion, ShipPostalCode, ShipCountry" +
                ") VALUES (" +
                "@CustomerId, @EmployeeId, @OrderDate, @RequiredDate, @ShippedDate, @ShipVia, @Freight, @ShipName, @ShipAddress, " +
                "@ShipCity, @ShipRegion, @ShipPostalCode, @ShipCountry);" +
                "SELECT OrderID FROM Orders ORDER BY OrderID DESC LIMIT 1;";

            using (var command = _context.Database.GetDbConnection().CreateCommand())
            {
                command.CommandText = sql;

                // Add parameters
                command.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter("@CustomerId", order.CustomerId));
                command.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter("@EmployeeId", order.EmployeeId));
                command.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter("@OrderDate", order.OrderDate.ToString("yyyy-MM-dd")));
                command.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter("@RequiredDate", order.RequiredDate.ToString("yyyy-MM-dd")));
                command.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter("@ShippedDate", (object)shippedDate ?? DBNull.Value));
                command.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter("@ShipVia", order.ShipVia));
                command.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter("@Freight", order.Freight));
                command.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter("@ShipName", order.ShipName));
                command.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter("@ShipAddress", order.ShipAddress));
                command.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter("@ShipCity", order.ShipCity));
                command.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter("@ShipRegion", order.ShipRegion));
                command.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter("@ShipPostalCode", order.ShipPostalCode));
                command.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter("@ShipCountry", order.ShipCountry));

                _context.Database.OpenConnection();

                using var dataReader = command.ExecuteReader();
                dataReader.Read();
                order.OrderId = Convert.ToInt32(dataReader[0]);
            }

            // Use parameterized query for OrderDetails
            sql = "INSERT INTO OrderDetails (" +
                "OrderId, ProductId, UnitPrice, Quantity, Discount" +
                ") VALUES (@OrderId, @ProductId, @UnitPrice, @Quantity, @Discount)";

            foreach (var orderDetails in order.OrderDetails)
            {
                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sql;

                    // Add parameters
                    command.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter("@OrderId", order.OrderId));
                    command.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter("@ProductId", orderDetails.ProductId));
                    command.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter("@UnitPrice", orderDetails.UnitPrice));
                    command.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter("@Quantity", orderDetails.Quantity));
                    command.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter("@Discount", orderDetails.Discount));

                    _context.Database.OpenConnection();
                    command.ExecuteNonQuery();
                }
            }

            if (order.Shipment != null)
            {
                var shipment = order.Shipment;
                shipment.OrderId = order.OrderId;

                sql = "INSERT INTO Shipments (" +
                    "OrderId, ShipperId, ShipmentDate, TrackingNumber" +
                    ") VALUES (@OrderId, @ShipperId, @ShipmentDate, @TrackingNumber)";

                using (var command = _context.Database.GetDbConnection().CreateCommand())
                {
                    command.CommandText = sql;

                    // Add parameters
                    command.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter("@OrderId", shipment.OrderId));
                    command.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter("@ShipperId", shipment.ShipperId));
                    command.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter("@ShipmentDate", shipment.ShipmentDate.ToString("yyyy-MM-dd")));
                    command.Parameters.Add(new Microsoft.Data.Sqlite.SqliteParameter("@TrackingNumber", shipment.TrackingNumber));

                    _context.Database.OpenConnection();
                    command.ExecuteNonQuery();
                }
            }

            return order.OrderId;
        }

        public void CreateOrderPayment(int orderId, decimal amountPaid, string creditCardNumber, DateTime expirationDate, string approvalCode)
        {
            var orderPayment = new OrderPayment()
            {
                AmountPaid = Convert.ToDouble(amountPaid),
                CreditCardNumber = creditCardNumber,
                ApprovalCode = approvalCode,
                ExpirationDate = expirationDate,
                OrderId = orderId,
                PaymentDate = DateTime.Now
            };
            _context.OrderPayments.Add(orderPayment);
            _context.SaveChanges();
        }

        public ICollection<Order> GetAllOrdersByCustomerId(string customerId)
        {
            return _context.Orders
                .Where(o => o.CustomerId == customerId)
                .OrderByDescending(o => o.OrderDate)
                .ThenByDescending(o => o.OrderId)
                .ToList();
        }
    }
}
