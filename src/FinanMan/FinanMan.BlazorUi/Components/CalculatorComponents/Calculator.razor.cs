using FinanMan.BlazorUi.SharedComponents.JsInterop;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace FinanMan.BlazorUi.Components.CalculatorComponents;

public partial class Calculator
{
    [Inject, AllowNull] private MyIsolatedModule MyIsolatedModule { get; set; }

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
    PreviousOperator _lastOperation;
    private ElementReference _focusButtonRef;

    /// <summary>
    /// Used to identify whether the input has been changed since the last calculation.
    /// </summary>
    private bool _inputDirty = true;
    private bool _active;
    private decimal? _currentCalculatedValue;
    private Operator? _prevOp;
    private Operator? _activeOp;

    private string _formulaOutput = string.Empty;

    private bool _decimalActive = false;

    [Parameter] public bool Show { get; set; }
    [Parameter] public EventCallback<bool> ShowChanged { get; set; }
    [Parameter] public EventCallback OnDismissed { get; set; }

    protected override void OnInitialized()
    {
        HandleClearAllClicked();
    }

    #region Handlers
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
        _decimalActive &= _decimalPart.Length > 0;
        if (_decimalPart.Length > 0)
        {
            _decimalPart = _decimalPart.Length == 1 ? string.Empty : _decimalPart[..^1];
        }
        else
        {
            _wholeNumberPart = (_wholeNumberPart - _wholeNumberPart % 10) / 10;
        }
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
        _inputDirty = false;
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
        if (!_inputDirty)
        {
            switch (op)
            {
                case Operator.Submit:
                    // When the displayed formula ends with an operator other than submit,
                    // we want to use the last operator
                    if (_formulaOutput.Trim().EndsWith(Operator.Submit.GetDisplayText()))
                    {
                        DisplayedInputNum = _currentCalculatedValue ?? 0;
                    }
                    else
                    {
                        // Do nothing if the formula ends with an operator and the current input is not dirty
                        return;
                    }
                    break;
                default:
                    _activeOp = op;
                    _prevOp = op;
                    _formulaOutput = $"{_currentCalculatedValue} {op.GetDisplayText()}";
                    return;
            }
        }

        var optToUse = op == Operator.Submit ? _activeOp : _prevOp;
        if (optToUse.HasValue && _currentCalculatedValue.HasValue)
        {
            var calcError = false;
            var valToUse = (_prevOp == Operator.Submit && op == Operator.Submit) ? _lastOperation.LastValue : DisplayedInputNum;
            if (op == Operator.Submit)
            {
                _lastOperation.LastValue = valToUse;
                _formulaOutput = $"{_currentCalculatedValue} {optToUse.Value.GetDisplayText()}";
            }
            else
            {
                _lastOperation.Operator = optToUse.Value;
            }

            // Perform the operation of the stored value with the previous operator and the current input value
            switch (optToUse)
            {
                case Operator.Add:
                    _currentCalculatedValue += valToUse;
                    break;
                case Operator.Subtract:
                    _currentCalculatedValue -= valToUse;
                    break;
                case Operator.Multiply:
                    _currentCalculatedValue *= valToUse;
                    break;
                case Operator.Divide:
                    if (valToUse == 0)
                    {
                        calcError = true;
                        _formulaOutput = $"{_formulaOutput} {valToUse} = #DIV/0!";
                        _currentCalculatedValue = 0;
                        HandleClearClicked();
                    }
                    else
                    {
                        _currentCalculatedValue /= valToUse;
                    }
                    break;
            }

            if (!calcError)
            {
                _formulaOutput = $"{_formulaOutput} {valToUse} = ";
                DisplayedInputNum = _currentCalculatedValue ?? 0m;
            }
        }

        if (op != Operator.Submit)
        {
            _activeOp = op;
            _currentCalculatedValue = DisplayedInputNum;
            _lastOperation.LastValue = _currentCalculatedValue ?? 0;
            _lastOperation.Operator = op;
            _formulaOutput = $"{_currentCalculatedValue} {op.GetDisplayText()}";
            HandleClearClicked();
            DisplayedInputNum = 0m;
        }

        _prevOp = op;
    }

    private async Task HandleDismissClicked()
    {
        await HandleShowChanged(false);
        if (OnDismissed.HasDelegate)
        {
            await OnDismissed.InvokeAsync();
        }
    }

    private async Task HandleContainerClicked()
    {
        if (_focusButtonRef.Context is not null)
        {
            await _focusButtonRef.FocusAsync();
        }
        await InvokeAsync(StateHasChanged);
    }

    private Task HandleMouseDown()
    {
        _active = true;
        return MyIsolatedModule.CapturePointerEvents(_focusButtonRef).AsTask();
    }

    private Task HandleMouseUp()
    {
        _active = false;
        return MyIsolatedModule.ReleasePointerEvents(_focusButtonRef).AsTask();
    }
    #endregion Handlers

    #region Helpers
    private enum Operator
    {
        [Display(Name = "+")]
        Add,
        [Display(Name = "-")]
        Subtract,
        [Display(Name = "*")]
        Multiply,
        [Display(Name = "&div;")]
        Divide,
        [Display(Name = "=")]
        Submit
    }

    private struct PreviousOperator
    {
        public Operator Operator { get; set; }
        public decimal LastValue { get; set; }
    }
    #endregion Helpers
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