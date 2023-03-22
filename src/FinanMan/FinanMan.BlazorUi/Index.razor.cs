using FinanMan.BlazorUi.FlyoutContentComponents;
using FinanMan.Shared.DataEntryModels;

namespace FinanMan.BlazorUi;

public partial class Index
{
    [Inject] private IUiState UiState { get; set; } = default!;

    private DepositEntryViewModel _newDeposit = new();
    private EditContext _editContext = default!;

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
        UiState.ShowFlyout(
            (builder) =>
            {
                builder.OpenComponent<ContactUsFlyoutContent>(0);
                builder.CloseComponent();
            }); 
    }

    public void Dispose()
    {
    }
}
