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
            Assert.Equal("0", renderedComponent.GetDisplay()); // Calculator should display '0' when first loaded
        }

        [Fact]
        public void PositiveNumberEntryWorks()
        {
            // Arrange
            var renderedComponent = RenderComponent<Calculator>(parameters => parameters.Add(p => p.Show, true));
            var calculator = renderedComponent.Instance;

            // Act
            renderedComponent.Enter("1");

            // Assert
            Assert.Equal("1", renderedComponent.GetDisplay());
        }
    }

    public static class CalculatorTestExtensions
    {
        public static string GetDisplay(this IRenderedComponent<Calculator> component)
        {
            return component.Find("#numberOutput").InnerHtml;
        }

        public static void Enter(this IRenderedComponent<Calculator> component, string entries)
        {
            var buttonClicks = entries.ToCharArray();
            foreach (var click in buttonClicks)
            {
                component.Find($"#btn-{click}").Click();
            }
        }
    }
}

