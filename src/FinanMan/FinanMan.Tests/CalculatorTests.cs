using FinanMan.BlazorUi.Components.CalculatorComponents;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace FinanMan.Tests
{
    public class CalculatorTests : TestContext
    {
        [Fact]
        public void StartsAtZero()
        {
            // Arrange + Act
            var renderedComponent = RenderComponent<Calculator>(parameters => parameters.Add(p => p.Show, true));


            // Assert
            Assert.Equal("0", renderedComponent.Find("#numberOutput").InnerHtml); // Calculator should display '0' when first loaded
        }

        [Fact]
        public void PositiveNumberEntryWorks()
        {
            // Arrange
            var renderedComponent = RenderComponent<Calculator>(parameters => parameters.Add(p => p.Show, true));
            var calculator = renderedComponent.Instance;

            // Act
            renderedComponent.Find("#btn-1").Click();

            // Assert
            Assert.Equal("1", renderedComponent.Find("#numberOutput").InnerHtml);
        }
    }
}

