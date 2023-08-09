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
                expectedInputNumDisplay: "0"
                );
        }

        [Fact]
        public void PositiveNumberEntryWorks()
        {
            TestTheCalculator(
                inputs: "12",
                expectedInputNumDisplay: "12"
                );
        }

        [Fact]
        public void NegationWorks()
        {
            TestTheCalculator(
                inputs: "1 nn nn nn nn nn nn n",
                expectedInputNumDisplay: "-1"
                );
        }

        [Fact]
        public void BasicAdditionWorks()
        {
            TestTheCalculator(
                inputs: "1 + 20 =",
                expectedCalculatedValue: "21",
                expectedInputNumDisplay: "20",
                expectedFormulaOutput: "1 + 20 = ");
        }

        [Fact]
        public void BasicSubtractWorks()
        {
            TestTheCalculator(
                inputs: "2 - 20 =",
                expectedCalculatedValue: "-18",
                expectedInputNumDisplay: "20",
                expectedFormulaOutput: "2 - 20 = ");
        }

        [Fact]
        public void BasicMultiplicationWorks()
        {
            TestTheCalculator(
                inputs: "2 * 20 =",
                expectedCalculatedValue: "40",
                expectedInputNumDisplay: "20",
                expectedFormulaOutput: "2 * 20 = ");
        }

        [Fact]
        public void BasicDivisionWorks()
        {
            TestTheCalculator(
                inputs: "20 / 2 =",
                expectedCalculatedValue: "10",
                expectedInputNumDisplay: "2",
                expectedFormulaOutput: "20 / 2 = ");
        }

        [Fact]
        public void DivideByZeroOutputWorks()
        {
            TestTheCalculator(
                inputs: "20 / 0 =",
                expectedCalculatedValue: "0",
                expectedInputNumDisplay: "0",
                expectedFormulaOutput: "20 / 0 = #DIV/0!");
        }

        [Theory]
        [InlineData("1 + 22 + 4 =", "27", "4", "23 + 4 = ")]
        [InlineData("1 + 22 + 4 + 21 =", "48", "21", "27 + 21 = ")]
        [InlineData("1 + 22 * 4 =", "92", "4", "23 * 4 = ")]
        public void ConsecutiveOperationsTotalCorrectly(string inputs, string? expectedCalculatedValue = null, string? expectedInputNumDisplay = null, string? expectedFormulaOutput = null)
        {
            TestTheCalculator(
                inputs: inputs,
                expectedCalculatedValue: expectedCalculatedValue,
                expectedInputNumDisplay: expectedInputNumDisplay,
                expectedFormulaOutput: expectedFormulaOutput);
        }

        // Test for pushing operator buttons after equals button followed by another number
        [Theory]
        [InlineData("1 + 22 + 14 = + 5 =", "42", "5", "37 + 5 = ")]
        [InlineData("1 + 22 - 14 = * 5 =", "45", "5", "9 * 5 = ")]
        public void PressingOperationButtonAfterEqualsWork(string inputs, string? expectedCalculatedValue = null, string? expectedInputNumDisplay = null, string? expectedFormulaOutput = null)
        {
            TestTheCalculator(
                inputs: inputs,
                expectedCalculatedValue: expectedCalculatedValue,
                expectedInputNumDisplay: expectedInputNumDisplay,
                expectedFormulaOutput: expectedFormulaOutput);
        }

        // TODO: Test for Clear button only clearing current input and not total

        // TODO: Test for pushing Clear all button

        // TODO: Test for pushing operator button multiple times in a row

        // TODO: Test for pushing different operator buttons consecutively uses the most recently pressed operator

        // DukaDo: Test for everything I haven't thought of yet

        /// <summary>
        /// Presses the buttons in the calculator component that correspond to the input 
        /// string and asserts that the expected values are displayed.
        /// </summary>
        /// <param name="inputs">
        /// A string of characters that will be pressed in the calculator component.
        /// </param>
        /// <param name="expectedCalculatedValue">
        /// The expected value of the current value display.
        /// </param>
        /// <param name="expectedInputNumDisplay">
        /// The expected value of the number output display.
        /// </param>
        /// <param name="expectedFormulaOutput">
        /// The expected value of the formula output display.
        /// </param>
        /// <exception cref="Exception"></exception>
        private void TestTheCalculator(string inputs, string? expectedCalculatedValue = null, string? expectedInputNumDisplay = null, string? expectedFormulaOutput = null)
        {
            // Arrange
            var renderedComponent = RenderComponent<Calculator>(parameters => parameters.Add(p => p.Show, true));
            var calculator = renderedComponent.Instance;

            // Act
            renderedComponent.Enter(inputs);

            // Assert
            var display = renderedComponent.GetDisplay();

            if (expectedCalculatedValue == null && expectedInputNumDisplay == null && expectedFormulaOutput == null) throw new Exception("you must make an assertion");
            if (expectedCalculatedValue != null) Assert.Equal(expectedCalculatedValue, display.CurrentValue);
            if (expectedInputNumDisplay != null) Assert.Equal(expectedInputNumDisplay, display.NumOutput); // for helpful debugging?
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