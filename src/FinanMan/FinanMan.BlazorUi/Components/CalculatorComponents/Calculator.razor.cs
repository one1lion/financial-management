using System.ComponentModel.DataAnnotations;

namespace FinanMan.BlazorUi.Components.CalculatorComponents;
public partial class Calculator
{
    private readonly static int[] _numPadItems = new[]
    {
        7, 8, 9, 4, 5, 6, 1, 2, 3
    };

    private decimal DisplayedInputNum
    {
        get => decimal.Parse($"{_wholeNumberPart}{(!string.IsNullOrWhiteSpace(_decimalPart) ? $".{_decimalPart}" : string.Empty)}");
        set
        {
            _wholeNumberPart = (long)Math.Floor(value);

            var numParts = value.ToString().Split('.');
            _decimalPart = numParts.Length > 1 ? numParts.Last().TrimEnd('0') : string.Empty;
        }
    }

    private long _wholeNumberPart;
    private string _decimalPart = string.Empty;

    /// <summary>
    /// Used to identify whether the input has been changed since the last calculation.
    /// </summary>
    private bool _inputDirty = true;

    private decimal? _currentCalculatedValue;
    private Operator? _prevOp;
    private Operator? _activeOp;

    private string _formulaOutput = string.Empty;

    private bool _decimalActive = false;

    [Parameter] public bool Show { get; set; }
    [Parameter] public EventCallback<bool> ShowChanged { get; set; }

    protected override void OnInitialized()
    {
        HandleClearAllClicked();
    }

    private async Task HandleShowChanged(bool newShow)
    {
        if (Show == newShow) { return; }

        Show = newShow;
        if (ShowChanged.HasDelegate)
        {
            await ShowChanged.InvokeAsync(Show);
        }

        await InvokeAsync(StateHasChanged);
    }

    private void HandleRemove()
    {

        StateHasChanged();
    }

    private void HandleNumberClicked(int num)
    {
        if (!_inputDirty)
        {
            HandleClearClicked();
        }

        _inputDirty = true;

        if (_decimalActive)
        {
            _decimalPart += num;
        }
        else
        {
            _wholeNumberPart = _wholeNumberPart * 10 + num;
        }
        StateHasChanged();
    }

    private void HandleNegateClicked()
    {
        _wholeNumberPart *= -1;
    }

    private void HandlePeriodClicked()
    {
        _decimalActive = !_decimalActive || !string.IsNullOrWhiteSpace(_decimalPart);
    }

    private void HandleClearClicked()
    {
        _wholeNumberPart = 0;
        _decimalPart = string.Empty;
        _decimalActive = false;
    }

    private void HandleClearAllClicked()
    {
        HandleClearClicked();
        _inputDirty = true;
        _activeOp = null;
        _formulaOutput = string.Empty;
        _currentCalculatedValue = null;
        _prevOp = null;
    }

    private void HandleOperatorClicked(Operator op)
    {
        switch (op)
        {
            case Operator.Submit:
                var calcError = false;
                // Perform the operation of the stored value with the previous operator and the current input value
                switch (_prevOp)
                {
                    case Operator.Add:
                        _currentCalculatedValue += DisplayedInputNum;
                        break;
                    case Operator.Subtract:
                        _currentCalculatedValue -= DisplayedInputNum;
                        break;
                    case Operator.Multiply:
                        _currentCalculatedValue *= DisplayedInputNum;
                        break;
                    case Operator.Divide:
                        if (DisplayedInputNum == 0)
                        {
                            calcError = true;
                            _formulaOutput = $"{_formulaOutput} {DisplayedInputNum} = #DIV/0!";
                            _currentCalculatedValue = 0;
                            HandleClearClicked();
                        }
                        else
                        {
                            _currentCalculatedValue /= DisplayedInputNum;
                        }
                        break;
                }

                if (!calcError)
                {
                    _formulaOutput = $"{_formulaOutput} {DisplayedInputNum} = ";
                    DisplayedInputNum = _currentCalculatedValue ?? 0m;
                }
                break;
        }

        if (op != Operator.Submit)
        {
            _activeOp = op;
            _currentCalculatedValue = DisplayedInputNum;
            _formulaOutput = $"{_currentCalculatedValue} {op.GetDisplayText()}";
            HandleClearClicked();
        }

        _prevOp = op;
    }

    private enum Operator
    {
        [Display(Name = "+")]
        Add,
        [Display(Name = "-")]
        Subtract,
        [Display(Name = "*")]
        Multiply,
        [Display(Name = "/")]
        Divide,
        [Display(Name = "=")]
        Submit
    }
}


/*
Calculator logic
- We have:
  - Input
  - Active Operator
  - Previous Operator
  - Current Calculated Value
  - Formula Output
  - Dirty Input Flag
  - Decimal Active Flag
- Various states:
  - Initial State - The component has just rendered or Clear All is selected:
    - The input is dirty
    - The active operator is null
    - The previous operator is null
    - The current calculated value is null
    - The formula output is empty
    - The decimal active flag is false
  - A number is selected after initial state:
    - The input is dirty
    - The active operator is null
    - The previous operator is null
    - The current calculated value is null
    - The formula output is empty
    - The decimal active flag is false
  - An operator is selected after initial state:
    - The input is dirty
    - The active operator is the selected operator
    - The previous operator is null
    - The current calculated value is null
    - The formula output is empty
    - The decimal active flag is false
  - A number is selected after an operator is selected:
    - The input is dirty
 */