using FinanMan.BlazorUi.JsInterop;
using FinanMan.BlazorUi.SharedComponents.DraggableComponents;
using FinanMan.BlazorUi.SharedComponents.JsInterop;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using System.Text.RegularExpressions;

namespace FinanMan.BlazorUi.Components.CalculatorComponents;

public partial class Calculator : IDisposable
{
    private static readonly Regex _allowedKeys = AllowedKeysRegEx();

    [Inject, AllowNull] private IMyIsolatedModule MyIsolatedModule { get; set; }
    [Inject, AllowNull] private IJSRuntime JsRuntime { get; set; }

    [CascadingParameter] public DraggableContainer? DraggableContainer { get; set; }
    [Parameter] public bool Show { get; set; }
    [Parameter] public EventCallback<bool> ShowChanged { get; set; }
    [Parameter] public EventCallback OnDismissed { get; set; }

    private decimal DisplayedInputNum
    {
        get => decimal.Parse($"{_wholeNumberPart}{(!string.IsNullOrWhiteSpace(_decimalPart) ? $".{_decimalPart}" : string.Empty)}");
        set
        {
            _wholeNumberPart = (long)Math.Round(value, MidpointRounding.ToZero);
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

    protected override void OnInitialized()
    {
        HandleClearAllClicked();
        if (DraggableContainer is not null)
        {
            DraggableContainer.OnDragEnd += HandleContainerClicked;
        }
    }

    private bool _prevShow;
    private bool _resetFocus;
    protected override void OnParametersSet()
    {
        base.OnParametersSet();
        if (Show != _prevShow)
        {
            _prevShow = Show;

            if (Show)
            {
                _resetFocus = true;
            }
        }
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if(_resetFocus && _focusButtonRef.Context is not null)
        {
            _resetFocus = false;
            await _focusButtonRef.FocusAsync();
        }
        await base.OnAfterRenderAsync(firstRender);
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

    private void HandleDecimalClicked()
    {
        if (!_inputDirty)
        {
            HandleClearClicked();
        }

        _inputDirty = true;

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
                    // we want to use the current value in the value display
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

    private void HandleKeyPress(KeyboardEventArgs e)
    {
        var key = e.Key.ToLower();
        if (!_allowedKeys.IsMatch(key))
        {
            return;
        }

        switch (key)
        {
            case "escape":
                HandleClearAllClicked();
                break;
            case "delete":
                HandleClearClicked();
                break;
            case "backspace":
                HandleRemove();
                break;
            case "enter":
                HandleOperatorClicked(Operator.Submit);
                break;
            case "c":
                if (!e.CtrlKey) { return; }
                SuperDukaSoftInterop.CopyTextToClipboard(JsRuntime, DisplayedInputNum.ToString());
                break;
            case "n":
                HandleNegateClicked();
                break;
            case "+":
                HandleOperatorClicked(Operator.Add);
                break;
            case "-":
                HandleOperatorClicked(Operator.Subtract);
                break;
            case "*":
                HandleOperatorClicked(Operator.Multiply);
                break;
            case "/":
                HandleOperatorClicked(Operator.Divide);
                break;
            case "=":
                HandleOperatorClicked(Operator.Submit);
                break;
            case ".":
            case ",":
                HandleDecimalClicked();
                break;
            default:
                if (key.StartsWith("arrow"))
                {
                    switch (key.Replace("arrow", string.Empty))
                    {
                        case "up":
                            Console.WriteLine("Moving on up");
                            break;
                        case "down":
                            Console.WriteLine("Moving on down");
                            break;
                        case "left":
                            Console.WriteLine("Moving on left");
                            break;
                        case "right":
                            Console.WriteLine("Moving on right");
                            break;
                    }
                    break;
                }
                if (int.TryParse(e.Key, out var num))
                {
                    HandleNumberClicked(num);
                }
                break;
        }
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

    [GeneratedRegex(@"^([0-9\.,\+\-\*\/\=]|escape|delete|enter|backspace|c|n|arrow.+)$")]
    private static partial Regex AllowedKeysRegEx();
    #endregion Helpers

    public void Dispose()
    {
        if (DraggableContainer is not null)
        {
            DraggableContainer.OnDragEnd -= HandleContainerClicked;
        }
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