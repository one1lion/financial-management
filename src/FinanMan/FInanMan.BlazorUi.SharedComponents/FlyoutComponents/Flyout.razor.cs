using Microsoft.AspNetCore.Components;

namespace FinanMan.BlazorUi.SharedComponents.FlyoutComponents;

public partial class Flyout
{
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public bool Show { get; set; }
    [Parameter] public EventCallback<bool> ShowChanged { get; set; }

    public Func<Task<bool>>? CanClose { get; set; }

    private bool _show;
    protected override Task OnParametersSetAsync()
    {
        if (Show != _show)
        {
            return UpdateShowAsync(Show);
        }
        return Task.CompletedTask;
    }

    private async Task UpdateShowAsync(bool show)
    {
        var canCloseTask = CanClose?.Invoke() ?? Task.FromResult(true);
        if (_show && !show && !await canCloseTask)
        {
            Show = true;
            return;
        }
        _show = show;
        if (Show != show) { Show = show; }
        await InvokeAsync(StateHasChanged);
        await ShowChanged.InvokeAsync(show);
    }
}