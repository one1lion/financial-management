using System.ComponentModel.DataAnnotations;

namespace FinanMan.BlazorUi.Services;
public class CalculatorService
{
    public enum Key
    {
        Zero,
        One,
        Two,
        Three,
        Four,
        Five,
        Six,
        Seven,
        Eight,
        Nine,
        Decimal,
        Negate,
        Add,
        Subtract,
        Multiply,
        Divide,
        Equals,
        Remove,
        Clear,
        ClearAll
    }

    public enum Operator
    {
        [Display(Name = "+")]
        Add,
        [Display(Name = "-")]
        Subtract,
        [Display(Name = "*")]
        Multiply,
        [Display(Name = "/")]
        Divide
    }

    public decimal? ProcessKeyPress()
    {
        throw new NotImplementedException();
    }

    public decimal? Calculate(decimal? currentValue, decimal numOutput, Operator? prevOp, Operator? activeOp)
    {
        if (currentValue is null || activeOp is null) { return numOutput; }

        return activeOp switch
        {
            Operator.Add => currentValue + numOutput,
            Operator.Subtract => currentValue - numOutput,
            Operator.Multiply => currentValue * numOutput,
            Operator.Divide => currentValue / numOutput,
            _ => throw new NotImplementedException()
        };
    }
}
