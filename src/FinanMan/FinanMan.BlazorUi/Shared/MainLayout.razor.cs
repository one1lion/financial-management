using FinanMan.BlazorUi.JsInterop;

using Timr = System.Timers.Timer;

namespace FinanMan.BlazorUi.Shared;

public partial class MainLayout : IDisposable
{
    [Inject, AllowNull] private IJSRuntime JsRuntime { get; set; }
    [Inject, AllowNull] private IUiState UiState { get; set; }
    [Inject, AllowNull] private NavigationManager NavigationManager { get; set; }

    private bool _remakeStyles;

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
        NavigationManager.LocationChanged += NavigationManager_LocationChanged;
        _remakeStyles = NavigationManager.Uri.StartsWith($"{NavigationManager.BaseUri}remake-", StringComparison.OrdinalIgnoreCase);
    }

    private void NavigationManager_LocationChanged(object? sender, LocationChangedEventArgs e)
    {
        if (!string.IsNullOrEmpty(e.Location) && e.Location != NavigationManager.Uri)
        {
            // Only change if we are on a page that starts with "remake-"
            _remakeStyles = e.Location.StartsWith($"{NavigationManager.BaseUri}remake-", StringComparison.OrdinalIgnoreCase);
            StateHasChanged();
        }
    }

    private void HandleDebounceTimeout(object? sender, System.Timers.ElapsedEventArgs e)
    {
        _collapseDebounce.Stop();
        _moving = false;
    }

    private void HandleUiPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(UiState.FlyoutVisible) || e.PropertyName == nameof(UiState.MessageDialogVisible))
        {
            StateHasChanged();
        }
    }

    private async Task HandleCollapseSelectLists()
    {
        _collapseDebounce.Stop();
        _collapseDebounce.Start();

        if (_moving) { return; }
        _moving = true;
        await SuperDukaSoftInterop.CollapseSelectLists(JsRuntime).AsTask();
    }

    private void HandleFlyoutShowChanged(bool newShow)
    {
        if (newShow)
        {
            UiState.DisplayFlyout(UiState.FlyoutContent);
        }
        else
        {
            UiState.CollapseFlyout();
        }
    }

    public void Dispose()
    {
        _collapseDebounce.Stop();
        _collapseDebounce.Elapsed -= HandleDebounceTimeout;
        _collapseDebounce.Dispose();
        UiState.PropertyChanged -= HandleUiPropertyChanged;
        UiState.CollapseSelectLists -= HandleCollapseSelectLists;
        NavigationManager.LocationChanged -= NavigationManager_LocationChanged;
    }
}