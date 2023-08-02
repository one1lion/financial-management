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
            var display = renderedComponent.GetDisplay();
            Assert.Equal("0", display.CurrentValue); // Calculator should display '0' when first loaded
        }

        [Fact]
        public void PositiveNumberEntryWorks()
        {
            // Arrange
            var renderedComponent = RenderComponent<Calculator>(parameters => parameters.Add(p => p.Show, true));

            // Act
            renderedComponent.Enter("1");

            // Assert
            var display = renderedComponent.GetDisplay();
            Assert.Equal("1", display.NumOutput);
        }

        [Fact]
        public void NegationWorks()
        {
            // Arrange
            var renderedComponent = RenderComponent<Calculator>(parameters => parameters.Add(p => p.Show, true));

            // Act
            // apology: 'n' for negation is awkward, but we're already using '-' for subtract
            renderedComponent.Enter("1 nn nn nn nn nn nn n"); // negated 13 times

            // Assert
            var display = renderedComponent.GetDisplay();
            Assert.Equal("-1", display.NumOutput);
        }

        [Fact]
        public void BasicAdditionWorks()
        {
            // Arrange
            var renderedComponent = RenderComponent<Calculator>(parameters => parameters.Add(p => p.Show, true));
            var calculator = renderedComponent.Instance;

            // Act
            renderedComponent.Enter("1 + 20 =");

            // Assert
            var display = renderedComponent.GetDisplay();
            Assert.Equal("21", display.CurrentValue);
            Assert.Equal("20", display.NumOutput); // for helpful debugging?
            Assert.Equal("1 + 20 = ", display.FormulaOutput);
        }
    }

    public static class CalculatorTestExtensions
    {
        public static Result GetDisplay(this IRenderedComponent<Calculator> component)
        {
            var formulaOutput = component.Find("#formulaOutput").InnerHtml;
            var currentValue = component.Find("#currentValue").InnerHtml;
            var numOutput = component.Find("#numberOutput").InnerHtml;
            return new Result
            {
                FormulaOutput = formulaOutput,
                CurrentValue = currentValue,
                NumOutput = numOutput
            };
        }

        public class Result
        {
            public string NumOutput { get; set; }
            public string CurrentValue { get; set; }
            public string FormulaOutput { get; set; }
        }

        public static void Enter(this IRenderedComponent<Calculator> component, string entries)
        {
            var buttonClicks = entries.ToCharArray()
                .Where(x => x != ' '); // ignore ' ' for easier reading

            foreach (var click in buttonClicks)
            {
                var allButtons = component.FindAll("button"); // do not try to move this inefficient line of code. We must re-find all buttons after each render (i.e. after each button click)
                var button = allButtons.Single(x => x.Id == $"btn-{click}");
                button.Click();
            }
        }

    }
}

