using FinanMan.BlazorUi.JsInterop;
using System.Diagnostics;
using Timr = System.Timers.Timer;

namespace FinanMan.BlazorUi.Shared;

public partial class MainLayout
{
    [Inject] private IJSRuntime JsRuntime { get; set; } = default!;
    [Inject] private IUiState UiState { get; set; } = default!;

    private bool _moving;
    private readonly Timr _collapseDebounce = new()
    {
        Interval = 200,
        AutoReset = false
    };

    protected override void OnInitialized()
    {
        _collapseDebounce.Elapsed += HandleDebounceTimeout;
        UiState.PropertyChanged += HandleUiPropertyChanged;
        UiState.CollapseSelectLists += HandleCollapseSelectLists;
        var hold = UiState.SomeNum;
    }

    private void HandleDebounceTimeout(object? sender, System.Timers.ElapsedEventArgs e)
    {
        _collapseDebounce.Stop();
        _moving = false;
    }

    private void HandleUiPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(UiState.FlyoutVisible))
        {
            StateHasChanged();
        }
    }

    long _changeCount;

    private async Task HandleCollapseSelectLists()
    {
        _collapseDebounce.Stop();
        _collapseDebounce.Start();

        if (_moving) { return; }
        _moving = true;
        Debug.WriteLine($"Handling Collapse {++_changeCount}");
        await SuperDukasoftInterop.CollapseSelectLists(JsRuntime).AsTask();
        await InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
        _collapseDebounce.Stop();
        _collapseDebounce.Elapsed -= HandleDebounceTimeout;
        _collapseDebounce.Dispose();
        UiState.PropertyChanged -= HandleUiPropertyChanged;
        UiState.CollapseSelectLists -= HandleCollapseSelectLists;
    }
}