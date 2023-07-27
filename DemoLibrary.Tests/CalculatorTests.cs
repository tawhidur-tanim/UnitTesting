using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace DemoLibrary.Tests
{
    public class CalculatorTests
    {

        [Theory]
        [InlineData(3,5,8)]
        [InlineData(3,5.5,8.5)]
        [InlineData(Double.MaxValue,5,Double.MaxValue)]
        public void Add_SimpleValuesShouldCalculate(double x, double y, double expected)
        {
            // Arrange

            // double expected = 5;

            // Act

            double actual = Calculator.Add(x, y);

            // Assert

            Assert.Equal(expected, actual);


        }



        [Theory]
        [InlineData(4, 2, 2)]
        [InlineData(32, 2, 16)]
        public void Divide_SimpleValuesShouldCalculate(double x, double y, double expected)
        {
            // Arrange

            // double expected = 5;

            // Act

            double actual = Calculator.Divide(x, y);

            // Assert

            Assert.Equal(expected, actual);
        }


        [Fact]
        public void Divide_DivideByZero()
        {
            // Arrange

            double expected = 0;

            // Act

            double actual = Calculator.Divide(1, 0);

            // Assert

            Assert.Equal(expected, actual);
        }

    }
}
