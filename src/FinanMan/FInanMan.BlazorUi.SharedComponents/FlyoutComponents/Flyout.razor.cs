using Microsoft.AspNetCore.Components;

namespace FinanMan.BlazorUi.SharedComponents.FlyoutComponents;

/// <summary>
/// A flyout component that can be shown or hidden
/// </summary>
public partial class Flyout
{
    /// <summary>
    /// The content to display in the flyout
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// Whether or not the flyout is shown
    /// </summary>
    [Parameter] public bool Show { get; set; }
    /// <summary>
    /// The event callback for when the show property changes
    /// </summary>
    [Parameter] public EventCallback<bool> ShowChanged { get; set; }
    /// <summary>
    /// The method indicating whether or not the flyout can be closed
    /// </summary>
    public Func<Task<bool>>? CanClose { get; set; }

    private bool _show;

    /// <inheritdoc/>
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