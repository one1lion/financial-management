using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace FinanMan.BlazorUi.Components.CalculatorComponents;
public partial class Calculator
{
    private readonly static int[] _numPadItems = new[]
    {
        7, 8, 9, 4, 5, 6, 1, 2, 3
    };

    private decimal DisplayedInputNum {
        get => decimal.Parse($"{_wholeNumberPart}{(_decimalPart > 0 ? $".{_decimalPart}" : string.Empty)}");
        set
        {
            _wholeNumberPart = (long)Math.Floor(value);

            var numParts = value.ToString().Split('.');
            _decimalPart = numParts.Length > 1 ? long.Parse(numParts.Last()) : 0;
        }
    }

    private long _wholeNumberPart;
    private long _decimalPart;

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
        _decimalActive &= _decimalPart > 0;
        if (_decimalActive)
        {
            _decimalPart = (_decimalPart - _decimalPart % 10) / 10;
        }
        else
        {
            _wholeNumberPart = (_wholeNumberPart - _wholeNumberPart % 10) / 10;
        }
        StateHasChanged();
    }

    private void HandleNumberClicked(int num)
    {
        if(!_inputDirty)
        {
            // Reset the input number to 0
            _wholeNumberPart = 0;
            _decimalPart = 0;
        }
        _inputDirty = true;
        if (_decimalActive)
        {
            _decimalPart = _decimalPart * 10 + num;
        }
        else
        {
            _wholeNumberPart = _wholeNumberPart * 10 + (_wholeNumberPart < 0 ? -1 : 1) * num;
        }
        StateHasChanged();
    }

    private void HandleNegateClicked()
    {
        _wholeNumberPart *= -1;
    }

    private void HandlePeriodClicked()
    {
        _decimalActive = !_decimalActive || _decimalPart > 0;
    }

    private void HandleClearClicked()
    {
        _wholeNumberPart = 0;
        _decimalPart = 0;
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
        _activeOp = op;
        if ((op == Operator.Submit && _currentCalculatedValue is null)
            || (op != Operator.Submit && !_inputDirty)) { return; }

        if (!_prevOp.HasValue)
        {
            _prevOp = op;
            _currentCalculatedValue = DisplayedInputNum;
            _formulaOutput = $"{DisplayedInputNum} {_prevOp.Value.GetDisplayText()} ";
            _inputDirty = false;
            return;
        }

        if (_currentCalculatedValue.HasValue && _prevOp.HasValue)
        {
            _formulaOutput = $"{_currentCalculatedValue} {_prevOp.Value.GetDisplayText()} {DisplayedInputNum} = ";
        }

        _currentCalculatedValue ??= 0;

        if (_prevOp == Operator.Divide && DisplayedInputNum == 0)
        {
            _currentCalculatedValue = 0;
            _formulaOutput += "#DIV/0!";
        }
        else
        {
            _currentCalculatedValue = _prevOp switch
            {
                Operator.Subtract => _currentCalculatedValue - DisplayedInputNum,
                Operator.Multiply => _currentCalculatedValue * DisplayedInputNum,
                Operator.Divide => _currentCalculatedValue / DisplayedInputNum,
                _ => _currentCalculatedValue + DisplayedInputNum // Defaults to add
            };
        }

        _inputDirty = false;
        if (op == Operator.Submit)
        {
            _activeOp = null;
            DisplayedInputNum = _currentCalculatedValue ?? 0;
            _decimalActive = false;
            _inputDirty = true;
            _prevOp = null;
        }
        else
        {
            _prevOp = op;
        }


        if (op != Operator.Submit)
        {
            HandleClearClicked();
        }
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