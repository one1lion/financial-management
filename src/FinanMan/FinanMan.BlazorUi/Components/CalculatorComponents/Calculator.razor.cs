namespace FinanMan.BlazorUi.Components.CalculatorComponents;
public partial class Calculator
{
    private decimal NumOutput => decimal.Parse($"{_wholeNumber}{(_decimalPart > 0 ? $".{_decimalPart}" : string.Empty)}");

    private long _wholeNumber = 0;
    private long _decimalPart = 0;

    private string _formulaOutput;

    private bool _decimalActive = false;

    [Parameter] public bool Show { get; set; }
    [Parameter] public EventCallback<bool> ShowChanged { get; set; }

    private Task HandleShowChanged(bool newShow)
    {
        if (Show == newShow) { return Task.CompletedTask; }

        Show = newShow;
        if (ShowChanged.HasDelegate)
        {
            return ShowChanged.InvokeAsync(Show);
        }

        return Task.CompletedTask;
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
        Console.WriteLine($"{num} clicked");
        if(_decimalActive)
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

    private void HandleClearAllClicked()
    {
        _wholeNumber = 0;
        _decimalPart = 0;
        _decimalActive = false;
    }
}
