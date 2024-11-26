using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WebGoatCore.Data;
using WebGoatCore.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace WebGoatCore.Tests
{
    [TestClass]
    public class OrderRepositoryTests
    {
        private DbContextOptions<NorthwindContext> GetDbContextOptions()
        {
            return new DbContextOptionsBuilder<NorthwindContext>()
                .UseSqlite("DataSource=:memory:") // Use in-memory SQLite for testing
                .Options;
        }

        [TestMethod]
        public void CreateOrder_ShouldPreventSqlInjection()
        {
            // Arrange
            var options = GetDbContextOptions();
            using var context = new NorthwindContext(options);
            context.Database.OpenConnection();
            context.Database.EnsureCreated();

            var repository = new OrderRepository(context);
            var order = new Order
            {
                CustomerId = "ALFKI",
                EmployeeId = 1,
                OrderDate = DateTime.Now,
                RequiredDate = DateTime.Now.AddDays(7),
                ShipVia = 1,
                Freight = 10,
                ShipName = "SafeShip",
                ShipAddress = "SafeAddress",
                ShipCity = "SafeCity",
                ShipRegion = "SafeRegion",
                ShipPostalCode = "12345",
                ShipCountry = "SafeCountry"
            };

            var sqlInjectionAttempts = new[]
            {
                "'; DROP TABLE Orders; --",
                "'); UPDATE Products SET UnitPrice = 0; --"
            };

            foreach (var injection in sqlInjectionAttempts)
            {
                order.ShipCity = injection;

                // Act and Assert
                try
                {
                    repository.CreateOrder(order);
                    Assert.Fail("Expected exception was not thrown due to SQL injection attempt.");
                }
                catch (ApplicationException ex)
                {
                    StringAssert.Contains(ex.Message, "An error occurred while creating the order");
                }
            }
        }
    }
}
