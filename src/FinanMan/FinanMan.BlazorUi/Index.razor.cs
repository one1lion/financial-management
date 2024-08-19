using FinanMan.BlazorUi.FlyoutContentComponents;

namespace FinanMan.BlazorUi;

public partial class Index
{
#if !DEBUG
    private string? SocialsClass => null;
#endif 

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


#if DEBUG
public partial class Index
{
    private enum TriState
    {
        Neutral,
        Off,
        On
    }

    private TriState _socialDisplayToggle;
    private string? SocialsClass => _socialDisplayToggle switch
    {
        TriState.Neutral => null,
        TriState.Off => "no-title",
        TriState.On => "other-layout",
        _ => throw new ArgumentOutOfRangeException()
    };


}
#endif