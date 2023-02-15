using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.General;
using FinanMan.Shared.LookupModels;

namespace FinanMan.BlazorUi.Components.DataEntryComponents;

public partial class TransferEntry
{
    [Inject] private ILookupListState LookupListState { get; set; } = default!;
    [Inject] private ITransactionEntryService<TransferEntryViewModel> TransferEntryService { get; set; } = default!;
    [Inject] private ITransactionsState TransactionsState { get; set; } = default!;

    private ResponseModelBase<int>? _currentResponse;
    private bool _submitting;
    private InputDate<DateTime?>? _transDateInput;

    private TransferEntryViewModel _newTransfer = new();
    private List<AccountLookupViewModel>? _accounts;
    protected override async Task OnInitializedAsync()
    {
        await LookupListState.Initialize();
        _accounts = LookupListState.GetLookupItems<AccountLookupViewModel>().ToList();
    }

    private async Task HandleTransferSubmitted()
    {
        _currentResponse = default;
        _submitting = true;
        _currentResponse = await TransferEntryService.AddTransactionAsync(_newTransfer);
        if (!_currentResponse.WasError)
        {
            _newTransfer = new();
            if (_transDateInput is not null && _transDateInput.Element.HasValue)
            {
                await _transDateInput.Element.Value.FocusAsync();
            }
            await TransactionsState.RefreshTransactionHistoryAsync();
        }
        _submitting = false;

        return;
    }
}
