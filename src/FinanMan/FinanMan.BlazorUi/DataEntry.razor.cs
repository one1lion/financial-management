using FinanMan.BlazorUi.State;

namespace FinanMan.BlazorUi;

public partial class DataEntry
{
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private IUiState UiState { get; set; } = default!;

    [Parameter] public string EntType { get; set; } = string.Empty;
    [Parameter] public string ListType { get; set; } = string.Empty;

    private LookupListType ListTypeVal => Enum.TryParse<LookupListType>(ListType, out var t) ? t : LookupListType.AccountTypes;

    private EntryType EntTypeVal => Enum.TryParse<EntryType>(EntType, out var t) ? t : EntryType.Payments;

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            UiState.RaiseInitialUiLoadComplete();
        }
    }

    public override Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);
        var entTypeParam = parameters.TryGetValue<string>(nameof(EntType), out var entType);
        if (entTypeParam && !string.IsNullOrWhiteSpace(EntType) && !Enum.TryParse<EntryType>(entType, out var _))
        {
            NavigationManager.NavigateTo("data-entry");
        }

        return base.SetParametersAsync(ParameterView.Empty);
    }

    private void HandleEntryTabClicked(EntryType entryType)
    {
        if (EntTypeVal == entryType)
        {
            return;
        }

        NavigationManager.NavigateTo($"data-entry/{entryType}");
    }
}