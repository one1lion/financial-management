using FinanMan.BlazorUi.Components.CalculatorComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FinanMan.Tests
{
    public class CalculatorTests
    {
        [Fact]
        public void Test()
        {
            // Arrange + Act
            var calculator = new Calculator();

            // Assert
            Assert.Equal(0m, calculator.PublicNumOutput()); // Calculator should display '0' when first loaded
        }
    }

    // --------------------------------------------
    // DIRTY STUFF BELOW - AVERT YOUR EYES

    public static class CalculatorTestExtensions
    {
        public static decimal PublicNumOutput(this Calculator calculator)
        {
            var prop = typeof(Calculator).GetProperty("NumOutput", BindingFlags.NonPublic | BindingFlags.Instance);
            var getter = prop.GetGetMethod(nonPublic: true);
            return (decimal)getter.Invoke(calculator, null);
        }
    }
}

