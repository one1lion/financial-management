using FinanMan.BlazorUi.State;

namespace FinanMan.BlazorUi;

public partial class DataEntry
{
    [Inject] private NavigationManager NavigationManager { get; set; } = default!;
    [Inject] private IUiState UiState { get; set; } = default!;

    [Parameter] public string EntType { get; set; } = default!;

    private EntryType _entryType = EntryType.Payments;

    protected override void OnAfterRender(bool firstRender)
    {
        if (firstRender)
        {
            UiState.RaiseInitialUiLoadComplete();
        }
    }

    public override Task SetParametersAsync(ParameterView parameters)
    {
        var entTypeParam = parameters.TryGetValue<string>(nameof(EntType), out var entType);
        if (entTypeParam && !string.IsNullOrWhiteSpace(entType))
        {
            var entTypeParsed = Enum.GetValues<EntryType>().FirstOrDefault(x => x.ToString().ToLower() == entType.ToLower());
            if (entType.ToLower() != entTypeParsed.ToString().ToLower())
            {
                NavigationManager.NavigateTo("data-entry");
                return Task.CompletedTask;
            }
            else
            {
                _entryType = entTypeParsed;
            }
        }
        else
        {
            _entryType = EntryType.Payments;
        }

        return base.SetParametersAsync(parameters);
    }

    private void HandleEntryTabClicked(EntryType entryType)
    {
        if (_entryType == entryType)
        {
            return;
        }

        NavigationManager.NavigateTo($"data-entry/{entryType}");
    }
}