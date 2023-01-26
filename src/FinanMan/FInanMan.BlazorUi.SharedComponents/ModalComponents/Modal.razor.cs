using Microsoft.AspNetCore.Components;

namespace FinanMan.BlazorUi.SharedComponents.ModalComponents;
public partial class Modal
{
    [Parameter] public bool Show { get; set; }
    [Parameter] public EventCallback<bool>? ShowChanged { get; set; }
    [Parameter] public RenderFragment? ChildContent { get; set; }
    [Parameter] public bool ShowDismiss { get; set; } = true;

    private bool _prevShow;
    private bool _notifyShowChanged;

    /// <inheritdoc />
    public override Task SetParametersAsync(ParameterView parameters)
    {
        if(_prevShow != parameters.GetValueOrDefault<bool>(nameof(Show)))
        {
            _notifyShowChanged = true;
        }
        return base.SetParametersAsync(parameters);
    }

    /// <inheritdoc />
    protected override async Task OnParametersSetAsync()
    {
        if(_notifyShowChanged && (ShowChanged?.HasDelegate ?? false))
        {
            await ShowChanged.Value.InvokeAsync(Show);
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
