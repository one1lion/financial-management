using Microsoft.AspNetCore.Components;

namespace FinanMan.BlazorUi.SharedComponents.ModalComponents;

/// <summary>
/// A shareable Modal component that will render html markup
/// </summary>
public partial class Modal
{
    /// <summary>
    /// Whether or not the modal is shown
    /// </summary>
    [Parameter] public bool Show { get; set; }
    /// <summary>
    /// The event callback for when the show property changes
    /// </summary>
    [Parameter] public EventCallback<bool>? ShowChanged { get; set; }
    /// <summary>
    /// The content to display in the modal
    /// </summary>
    [Parameter] public RenderFragment? ChildContent { get; set; }
    /// <summary>
    /// Whether or not to show the dismiss button
    /// </summary>
    [Parameter] public bool ShowDismiss { get; set; } = true;

    private bool _prevShow;
    private bool _notifyShowChanged;

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        if (_prevShow != parameters.GetValueOrDefault<bool>(nameof(Show)))
        {
            _notifyShowChanged = true;
        }
        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        if (_notifyShowChanged && (ShowChanged?.HasDelegate ?? false))
        {
            await ShowChanged.Value.InvokeAsync(Show);
            _prevShow = Show;
            _notifyShowChanged = false;
        }
    }

    private void HandleDismissClicked()
    {
        Show = false;
        StateHasChanged();
    }
}
