using FinanMan.BlazorUi.Services;
using System.ComponentModel.DataAnnotations;

namespace FinanMan.BlazorUi.Components.CalculatorComponents;
public partial class Calculator
{
    private CalculatorService _calculator = new();

    private readonly static int[] _numPadItems = new[]
    {
        7, 8, 9, 6, 5, 4, 3, 2, 1
    };


    private decimal NumOutput => decimal.Parse($"{_wholeNumber}{(_decimalPart > 0 ? $".{_decimalPart}" : string.Empty)}");

    private long _wholeNumber = 0;
    private long _decimalPart = 0;

    private bool _inputDirty = true;

    private decimal? _currentValue;
    private Operator? _prevOp;
    private Operator? _activeOp;

    private string _formulaOutput = string.Empty;
    private string NextFormula => $"{_currentValue} {_activeOp?.GetDisplayText()} {NumOutput}";

    private bool _decimalActive = false;

    [Parameter] public bool Show { get; set; }
    [Parameter] public EventCallback<bool> ShowChanged { get; set; }

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
            _wholeNumber = (_wholeNumber - _wholeNumber % 10) / 10;
        }
        StateHasChanged();
    }

    private void HandleNumberClicked(int num)
    {
        _inputDirty = true;
        if (_decimalActive)
        {
            _decimalPart = _decimalPart * 10 + num;
        }
        else
        {
            _wholeNumber = _wholeNumber * 10 + (_wholeNumber < 0 ? -1 : 1) * num;
        }
        StateHasChanged();
    }

    private void HandleNegateClicked()
    {
        _wholeNumber *= -1;
    }

    private void HandlePeriodClicked()
    {
        _decimalActive = !_decimalActive || _decimalPart > 0;
    }

    private void HandleClearClicked()
    {
        _wholeNumber = 0;
        _decimalPart = 0;
        _decimalActive = false;
    }

    private void HandleClearAllClicked()
    {
        HandleClearClicked();
        _inputDirty = true;
        _activeOp = null;
        _formulaOutput = string.Empty;
        _currentValue = null;
        _prevOp = null;
    }

    private void HandleOperatorClicked(Operator op)
    {
        try
        {
            // TODO: After a submit, the next number pressed should replace current input

            // If we're submitting, use the previous operator if it exists, otherwise use the current operator
            _activeOp = op == Operator.Submit ? _prevOp : op;
            if ((op == Operator.Submit && _currentValue is null) 
                || (op != Operator.Submit && !_inputDirty)) { return; }

            if (_currentValue.HasValue)
            {
                var opToUse = (op == Operator.Submit ? _activeOp : _prevOp) ?? op;
                _formulaOutput = $"{_currentValue} {opToUse.GetDisplayText()} {NumOutput} = ";
            }

            _currentValue ??= 0;

            _currentValue = _prevOp switch
            {
                Operator.Subtract => _currentValue - NumOutput,
                Operator.Multiply => _currentValue * NumOutput,
                Operator.Divide => NumOutput == 0 ? 0 : _currentValue / NumOutput,
                _ => _currentValue + NumOutput // Defaults to add
            };

            _inputDirty = false;
            if (op == Operator.Submit)
            {
                _activeOp = null;
            }
            else
            {
                _prevOp = op;
            }
        }
        catch (Exception ex)
        {
            switch (ex)
            {
                case DivideByZeroException:
                    _formulaOutput = "#Div/0!";
                    break;
                default:
                    throw;
            }
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
