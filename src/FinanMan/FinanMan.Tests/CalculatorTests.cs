using FinanMan.BlazorUi.Components.CalculatorComponents;
using FinanMan.BlazorUi.SharedComponents.JsInterop;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FinanMan.Tests;

public class CalculatorTests : TestContext
{
    public CalculatorTests()
    {
        JSInterop.Mode = JSRuntimeMode.Loose;
        Services.AddSingleton<IMyIsolatedModule, MockIsolatedModule>();
    }

    [Fact]
    public void StartsAtZero()
    {
        TestTheCalculator(
            inputs: "",
            expectedInputNumDisplay: "0"
            );
    }

    [Fact]
    public void PositiveWholeNumberEntryWorks()
    {
        TestTheCalculator(
            inputs: "12",
            expectedInputNumDisplay: "12");
    }

    [Fact]
    public void DecimalNumberEntryWorks()
    {
        TestTheCalculator(
            inputs: "1 . 0 5",
            expectedInputNumDisplay: "1.05");
    }

    [Fact]
    public void NegationWorks()
    {
        TestTheCalculator(
            inputs: "1 nn nn nn nn nn nn n",
            expectedInputNumDisplay: "-1"
            );
    }

    [Theory]
    [InlineData("1 + 20 = ", "21", "1 + 20 = ")]
    [InlineData(". 1236 + 9 = ", "9.1236", "0.1236 + 9 = ")]
    public void BasicAdditionWorks(string inputs, string? expectedInputNumDisplay = null, string? expectedFormulaOutput = null)
    {
        TestTheCalculator(
            inputs: inputs,
            expectedInputNumDisplay: expectedInputNumDisplay,
            expectedFormulaOutput: expectedFormulaOutput);
    }

    [Theory]
    [InlineData("2 - 20 =", "-18", "2 - 20 = ")]
    [InlineData(". 1236 - 9 = ", "-8.8764", "0.1236 - 9 = ")]
    public void BasicSubtractWorks(string inputs, string? expectedInputNumDisplay = null, string? expectedFormulaOutput = null)
    {
        TestTheCalculator(
            inputs: inputs,
            expectedInputNumDisplay: expectedInputNumDisplay,
            expectedFormulaOutput: expectedFormulaOutput);
    }

    [Fact]
    public void BasicMultiplicationWorks()
    {
        TestTheCalculator(
            inputs: "2 * 20 =",
            expectedInputNumDisplay: "40",
            expectedFormulaOutput: "2 * 20 = ");
    }

    [Fact]
    public void BasicDivisionWorks()
    {
        TestTheCalculator(
            inputs: "20 / 2 =",
            expectedInputNumDisplay: "10",
            expectedFormulaOutput: "20 ÷ 2 = ");
    }

    [Fact]
    public void DivideByZeroOutputWorks()
    {
        TestTheCalculator(
            inputs: "20 / 0 =",
            expectedInputNumDisplay: "0",
            expectedFormulaOutput: "20 ÷ 0 = #DIV/0!");
    }

    [Theory]
    [InlineData("1 + 22 + ", "0", "23 +")]
    [InlineData("1 + 22 + 4 =", "27", "23 + 4 = ")]
    [InlineData("1 + 22 + 4 + 21 =", "48", "27 + 21 = ")]
    [InlineData("1 + 22 * 4 =", "92", "23 * 4 = ")]
    public void ConsecutiveOperationsTotalCorrectly(string inputs, string? expectedInputNumDisplay = null, string? expectedFormulaOutput = null)
    {
        TestTheCalculator(
            inputs: inputs,
            expectedInputNumDisplay: expectedInputNumDisplay,
            expectedFormulaOutput: expectedFormulaOutput);
    }

    // Test for pushing operator buttons after equals button followed by another number
    [Theory]
    [InlineData("1 + 22 + 14 = + 5 =", "42", "37 + 5 = ")]
    [InlineData("1 + 22 - 14 = * 5 =", "45", "9 * 5 = ")]
    public void PressingOperationButtonAfterEqualsWork(string inputs, string? expectedInputNumDisplay = null, string? expectedFormulaOutput = null)
    {
        TestTheCalculator(
            inputs: inputs,
            expectedInputNumDisplay: expectedInputNumDisplay,
            expectedFormulaOutput: expectedFormulaOutput);
    }

    // Test for pushing submit button multiple times in a row after performing a calculation repeats the last operation using the new calculated value and previous input value
    [Theory]
    [InlineData("5 =", "5", "")]
    [InlineData("5 = = = = = =", "5", "")]
    [InlineData("5 + 2 = = = =", "13", "11 + 2 = ")]
    public void PressingSubmitButtonMultipleTimesWork(string inputs, string? expectedInputNumDisplay = null, string? expectedFormulaOutput = null)
    {
        TestTheCalculator(
            inputs: inputs,
            expectedInputNumDisplay: expectedInputNumDisplay,
            expectedFormulaOutput: expectedFormulaOutput);
    }

    // Test for pushing different operator buttons consecutively uses the most recently pressed operator
    [Theory]
    [InlineData("1 * + 3 =", "4", "1 + 3 = ")]
    [InlineData("1 + 2 = * + 4 =", "7", "3 + 4 = ")]
    [InlineData("1 + 2 * + 4 =", "7", "3 + 4 = ")]
    [InlineData("1 + 2 * + 4 = + - ", "0", "7 -")]
    [InlineData("1 + 2 * + 4 = + - =", "0", "7 -")] // This ends up performing the operation 7 - 7 =
    public void PressingConsecutiveOperatorsWork(string inputs, string? expectedInputNumDisplay = null, string? expectedFormulaOutput = null)
    {
        TestTheCalculator(
            inputs: inputs,
            expectedInputNumDisplay: expectedInputNumDisplay,
            expectedFormulaOutput: expectedFormulaOutput);
    }

    // Test for what should happen when Submit is the first button pressed after loading
    [Fact]
    public void SubmitFirstButtonPressedAfterLoadingTest()
    {
        TestTheCalculator(
            inputs: "= ",
            expectedInputNumDisplay: "0",
            expectedFormulaOutput: "");
    }

    [Theory]
    [InlineData("1 + ", "0", "1 +")]
    [InlineData("1 - ", "0", "1 -")]
    [InlineData("1 * ", "0", "1 *")]
    [InlineData("1 / ", "0", "1 ÷")]
    public void DisplayedInputZeroesOutOnNonSubmitOperatorPressed(string inputs, string? expectedInputNumDisplay = null, string? expectedFormulaOutput = null)
    {
        TestTheCalculator(
            inputs: inputs,
            expectedInputNumDisplay: expectedInputNumDisplay,
            expectedFormulaOutput: expectedFormulaOutput);
    }

    // TODO: Test for Clear button only clearing current input and not total

    // TODO: Test for pushing Clear all button

    // DukaDo: Test for everything I haven't thought of yet

    /// <summary>
    /// Presses the buttons in the calculator component that correspond to the input 
    /// string and asserts that the expected values are displayed.
    /// </summary>
    /// <param name="inputs">
    /// A string of characters that will be pressed in the calculator component.
    /// </param>
    /// <param name="expectedInputNumDisplay">
    /// The expected value of the number output display.
    /// </param>
    /// <param name="expectedFormulaOutput">
    /// The expected value of the formula output display.
    /// </param>
    /// <exception cref="Exception"></exception>
    private void TestTheCalculator(string inputs, string? expectedInputNumDisplay = null, string? expectedFormulaOutput = null)
    {
        // Arrange
        var renderedComponent = RenderComponent<Calculator>(parameters => parameters.Add(p => p.Show, true));
        var calculator = renderedComponent.Instance;

        // Act
        renderedComponent.Enter(inputs);

        // Assert
        var display = renderedComponent.GetDisplay();

        if (expectedInputNumDisplay == null && expectedFormulaOutput == null) throw new Exception("you must make an assertion");
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
        var numOutput = component.Find("#numberOutput").InnerHtml;
        return new Result
        {
            FormulaOutput = formulaOutput,
            NumOutput = numOutput
        };
    }

    public class Result
    {
        public string? NumOutput { get; set; }
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

public class MockIsolatedModule : IMyIsolatedModule
{
    public ValueTask CapturePointerEvents(ElementReference element)
    {
        return ValueTask.CompletedTask;
    }

    public ValueTask ReleasePointerEvents(ElementReference element)
    {
        return ValueTask.CompletedTask;
    }
}