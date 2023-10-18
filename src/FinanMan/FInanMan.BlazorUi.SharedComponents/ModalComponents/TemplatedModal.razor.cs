using Microsoft.AspNetCore.Components;

namespace FinanMan.BlazorUi.SharedComponents.ModalComponents;

/// <summary>
/// A shareable Modal component that will render html markup in a templated way
/// </summary>
public partial class TemplatedModal
{
    /// <summary>
    /// Whether or not the modal is shown
    /// </summary>
    [Parameter] public bool Show { get; set; }
    /// <summary>
    /// The event callback for when the show property changes
    /// </summary>
    [Parameter] public EventCallback<bool> ShowChanged { get; set; }
    /// <summary>
    /// The content to display in the modal's header
    /// </summary>
    [Parameter] public RenderFragment? ModalHeader { get; set; }
    /// <summary>
    /// The content to display in the modal's body
    /// </summary>
    [Parameter] public RenderFragment ModalContent { get; set; } = default!;
    /// <summary>
    /// The content to display in the modal's footer
    /// </summary>
    [Parameter] public RenderFragment? ModalFooter { get; set; }
    /// <summary>
    /// Whether or not to show the dismiss button
    /// </summary>
    [Parameter] public bool ShowDismiss { get; set; } = true;

    private bool _prevShow;
    private bool _notifyShowChanged;
    /// <inheritdoc/>
    public override Task SetParametersAsync(ParameterView parameters)
    {
        if (_prevShow != parameters.GetValueOrDefault<bool>(nameof(Show)))
        {
            _notifyShowChanged = true;
        }

        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc/>
    protected override async Task OnParametersSetAsync()
    {
        if (_notifyShowChanged && ShowChanged.HasDelegate)
        {
            await ShowChanged.InvokeAsync(Show);
            _prevShow = Show;
            _notifyShowChanged = false;
        }
    }

    private void HandleDismissClicked()
    {
        ShowDismiss = false;
        StateHasChanged();
    }
}