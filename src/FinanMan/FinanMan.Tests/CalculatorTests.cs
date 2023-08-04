using FinanMan.BlazorUi.Components.CalculatorComponents;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinanMan.Tests
{
    public class CalculatorTests : TestContext
    {

        [Fact]
        public void StartsAtZero()
        {
            TestTheCalculator(
                inputs: "",
                numOutput: "0"
                );
        }

        [Fact]
        public void PositiveNumberEntryWorks()
        {
            TestTheCalculator(
                inputs: "12",
                numOutput: "12"
                );
        }

        [Fact]
        public void NegationWorks()
        {
            TestTheCalculator(
                inputs: "1 nn nn nn nn nn nn n",
                numOutput: "-1"
                );
        }

        [Fact]
        public void BasicAdditionWorks()
        {
            TestTheCalculator(
                inputs: "1 + 20 =",
                currentValue: "21",
                numOutput: "20",
                formulaOutput: "1 + 20 = ");
        }

        private void TestTheCalculator(string inputs, string? currentValue = null, string? numOutput = null, string? formulaOutput = null)
        {
            // Arrange
            var renderedComponent = RenderComponent<Calculator>(parameters => parameters.Add(p => p.Show, true));
            var calculator = renderedComponent.Instance;

            // Act
            renderedComponent.Enter(inputs);

            // Assert
            var display = renderedComponent.GetDisplay();

            if (currentValue == null && numOutput == null && formulaOutput == null) throw new Exception("you must make an assertion");
            if (currentValue != null) Assert.Equal(currentValue, display.CurrentValue);
            if (numOutput != null) Assert.Equal(numOutput, display.NumOutput); // for helpful debugging?
            if (formulaOutput != null) Assert.Equal(formulaOutput, display.FormulaOutput);
        }
    }

    public static class CalculatorTestExtensions
    {
        private static readonly Dictionary<char, string> _operatorMap = new()
        {
            { '+', "plus" },
            { '-', "minus" },
            { '*', "multipy" },
            { '/', "divide" },
            { '=', "submit" },
            { '.', "decimal" },
        };

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
            public string? NumOutput { get; set; }
            public string? CurrentValue { get; set; }
            public string? FormulaOutput { get; set; }
        }

        public static void Enter(this IRenderedComponent<Calculator> component, string entries)
        {
            var buttonClicks = entries.ToCharArray()
                .Where(x => x != ' ')
                // ignore ' ' for easier reading
                .Select(x => _operatorMap.TryGetValue(x, out var op) ? op : x.ToString());

            foreach (var click in buttonClicks)
            {
                var button = component.Find($"button#btn-{click}"); // do not try to move this inefficient line of code. We must re-find all buttons after each render (i.e. after each button click)
                button.Click();
            }
        }

    }
}