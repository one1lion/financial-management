using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.General;
using FinanMan.Shared.LookupModels;

namespace FinanMan.BlazorUi.Components.DataEntryComponents;

public partial class TransferEntry
{
    [Inject] private ILookupListState LookupListState { get; set; } = default!;
    [Inject] private ITransactionEntryService<TransferEntryViewModel> TransferEntryService { get; set; } = default!;
    [Inject] private ITransactionsState TransactionsState { get; set; } = default!;

    [Parameter] public TransferEntryViewModel? Transfer { get; set; }

    private ResponseModelBase<int>? _currentResponse;
    private bool _submitting;
#pragma warning disable IDE0044 // Add readonly modifier
#pragma warning disable CS0649 // Field 'TransferEntry._pmtDateInputRef' is never assigned to, and will always have its default value null
    private InputDate<DateTime?>? _transDateInputRef;
#pragma warning restore CS0649 // Field 'TransferEntry._pmtDateInputRef' is never assigned to, and will always have its default value null
#pragma warning restore IDE0044 // Add readonly modifier

    private TransferEntryViewModel _editTransfer = new();
    private List<AccountLookupViewModel>? _accounts;
    protected override async Task OnInitializedAsync()
    {
        await LookupListState.InitializeAsync();
        _accounts = LookupListState.GetLookupItems<AccountLookupViewModel>().ToList();
    }

    public override Task SetParametersAsync(ParameterView parameters)
    {
        parameters.SetParameterProperties(this);
        if (parameters.TryGetValue<TransferEntryViewModel>(nameof(Transfer), out var transfer))
        {
            _editTransfer = transfer;
        }
        return base.SetParametersAsync(ParameterView.Empty);
    }

    private async Task HandleTransferSubmitted()
    {
        if (_submitting) { return; }

        _currentResponse = default;
        _submitting = true;
        _editTransfer.UpdateAccountName(_accounts ?? new());
        _currentResponse = await TransferEntryService.AddTransactionAsync(_editTransfer);
        if (!_currentResponse.WasError)
        {
            _editTransfer = new();
            if (_transDateInputRef is not null && _transDateInputRef.Element.HasValue)
            {
                await _transDateInputRef.Element.Value.FocusAsync();
            }
            await TransactionsState.RefreshTransactionHistoryAsync();
        }
        _submitting = false;

        return;
    }
}
