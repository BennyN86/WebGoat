using System;

//To fix error run these 2
//dotnet add package Microsoft.EntityFrameworkCore 
//dotnet add package Microsoft.EntityFrameworkCore.Sqlite
using Microsoft.EntityFrameworkCore; 



using System.Data.Common; // For mocking database connections manually
using Microsoft.Data.Sqlite; // Use an in-memory SQLite database for testing
using WebGoatCore.Data;
using WebGoatCore.Models;

public class OrderRepositoryManualTest
{
    public void Test_CreateOrder_PreventsSQLInjection()
    {
        // Arrange
        var connection = new SqliteConnection("DataSource=:memory:");
        connection.Open();

        var options = new DbContextOptionsBuilder<NorthwindContext>()
            .UseSqlite(connection) // Use SQLite in-memory for testing
            .Options;

        using (var context = new NorthwindContext(options))
        {
            context.Database.EnsureCreated();

            var customerRepo = new CustomerRepository(context);
            var repository = new OrderRepository(context, customerRepo);

            var maliciousOrder = new Order
            {
                CustomerId = "123",
                EmployeeId = 1,
                OrderDate = DateTime.Now,
                RequiredDate = DateTime.Now.AddDays(7),
                ShipVia = 1,
                Freight = 10.0m,
                ShipName = "', 'Street', 'City', 'Region', '12345', 'Country'); UPDATE Products SET UnitPrice = 0; --",
                ShipAddress = "Address",
                ShipCity = "City",
                ShipRegion = "Region",
                ShipPostalCode = "12345",
                ShipCountry = "Country"
            };

            try
            {
                // Act
                repository.CreateOrder(maliciousOrder);

                // If no exception is thrown, fail the test
                throw new Exception("SQL Injection vulnerability detected.");
            }
            catch (Exception ex)
            {
                // Assert
                if (!ex.Message.Contains("malicious"))
                {
                    throw new Exception("Test failed: The input validation did not correctly prevent SQL injection.");
                }
            }
        }
    }
}
