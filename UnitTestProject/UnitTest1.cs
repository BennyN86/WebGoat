//using System;
using WebGoatCore.Models;
using Xunit;


namespace UnitTestProject
{
    public class OrderDetailTests
    {
        [Fact]
        public void Quantity_SetToPositiveValue_Succeeds()
        {
            // Arrange
            var orderDetail = new OrderDetail();

            // Act
            orderDetail.Quantity = 5;

            // Assert
            Assert.Equal(5, orderDetail.Quantity);
        }

        [Fact]
        public void Quantity_SetToNegativeValue_ThrowsArgumentOutOfRangeException()
        {
            // Arrange
            var orderDetail = new OrderDetail();

            // Act & Assert
            Assert.Throws<ArgumentOutOfRangeException>(() => orderDetail.Quantity = -1);
        }
    }
}
