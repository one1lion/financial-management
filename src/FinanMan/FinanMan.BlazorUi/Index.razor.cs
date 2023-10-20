using FinanMan.BlazorUi.FlyoutContentComponents;
using FinanMan.Shared.DataEntryModels;

namespace FinanMan.BlazorUi;

public partial class Index
{
    [Inject] private IUiState UiState { get; set; } = default!;

    protected override void OnInitialized()
    {
    }

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            UiState.RaiseInitialUiLoadComplete();
        }
    }

    private void HandleContactUsClicked()
    {
        UiState.CollapseFlyout();
        UiState.DisplayFlyout(
            (builder) =>
            {
                builder.OpenComponent<ContactUsFlyoutContent>(0);
                builder.CloseComponent();
            }); 
    }

    private bool _doClickChangeThing;
    private Task HandleComboClicked()
    {
        _doClickChangeThing = !_doClickChangeThing;
        return InvokeAsync(StateHasChanged);
    }

    public void Dispose()
    {
    }
}
