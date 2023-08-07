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
                expectedNumOutput: "0"
                );
        }

        [Fact]
        public void PositiveNumberEntryWorks()
        {
            TestTheCalculator(
                inputs: "12",
                expectedNumOutput: "12"
                );
        }

        [Fact]
        public void NegationWorks()
        {
            TestTheCalculator(
                inputs: "1 nn nn nn nn nn nn n",
                expectedNumOutput: "-1"
                );
        }

        [Fact]
        public void BasicAdditionWorks()
        {
            TestTheCalculator(
                inputs: "1 + 20 =",
                expectedCurrentValue: "21",
                expectedNumOutput: "20",
                expectedFormulaOutput: "1 + 20 = ");
        }

        [Fact]
        public void BasicSubtractWorks()
        {
            TestTheCalculator(
                inputs: "2 - 20 =",
                expectedCurrentValue: "-18",
                expectedNumOutput: "20",
                expectedFormulaOutput: "2 - 20 = ");
        }
        
        [Fact]
        public void BasicMultiplicationWorks()
        {
            TestTheCalculator(
                inputs: "2 * 20 =",
                expectedCurrentValue: "40",
                expectedNumOutput: "20",
                expectedFormulaOutput: "2 * 20 = ");
        }
        
        [Fact]
        public void BasicDivisionWorks()
        {
            TestTheCalculator(
                inputs: "20 / 2 =",
                expectedCurrentValue: "10",
                expectedNumOutput: "2",
                expectedFormulaOutput: "20 / 2 = ");
        }
        
        [Fact]
        public void DivideByZeroOutputWorks()
        {
            TestTheCalculator(
                inputs: "20 / 0 =",
                expectedCurrentValue: "0",
                expectedNumOutput: "0",
                expectedFormulaOutput: "20 / 0 = #DIV/0!");
        }

        /// <summary>
        /// Presses the buttons in the calculator component that correspond to the input 
        /// string and asserts that the expected values are displayed.
        /// </summary>
        /// <param name="inputs">
        /// A string of characters that will be pressed in the calculator component.
        /// </param>
        /// <param name="expectedCurrentValue">
        /// The expected value of the current value display.
        /// </param>
        /// <param name="expectedNumOutput">
        /// The expected value of the number output display.
        /// </param>
        /// <param name="expectedFormulaOutput">
        /// The expected value of the formula output display.
        /// </param>
        /// <exception cref="Exception"></exception>
        private void TestTheCalculator(string inputs, string? expectedCurrentValue = null, string? expectedNumOutput = null, string? expectedFormulaOutput = null)
        {
            // Arrange
            var renderedComponent = RenderComponent<Calculator>(parameters => parameters.Add(p => p.Show, true));
            var calculator = renderedComponent.Instance;

            // Act
            renderedComponent.Enter(inputs);

            // Assert
            var display = renderedComponent.GetDisplay();

            if (expectedCurrentValue == null && expectedNumOutput == null && expectedFormulaOutput == null) throw new Exception("you must make an assertion");
            if (expectedCurrentValue != null) Assert.Equal(expectedCurrentValue, display.CurrentValue);
            if (expectedNumOutput != null) Assert.Equal(expectedNumOutput, display.NumOutput); // for helpful debugging?
            if (expectedFormulaOutput != null) Assert.Equal(expectedFormulaOutput, display.FormulaOutput);
        }
    }

    public static class CalculatorTestExtensions
    {
        private static readonly Dictionary<char, string> _operatorMap = new()
        {
            { '+', "plus" },
            { '-', "minus" },
            { '*', "multiply" },
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