namespace FinanMan.BlazorUi;

public partial class Routes
{
    [Inject] private IJSRuntime JsRuntime { get; set; } = default!;
    [Inject] private IUiState UiState { get; set; } = default!;

    protected override void OnInitialized()
    {
    }

    public void Dispose()
    {
    }
}
