using FinanMan.Database.Models.Tables;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.Enums;
using FinanMan.Shared.General;
using FinanMan.Shared.LookupModels;

namespace FinanMan.BlazorUi.Components.TransactionHistoryComponents;
public partial class EditTransactionModal<T>
    where T : class, ITransactionDataEntryViewModel
{
    [Inject] private ILookupListState LookupListState { get; set; } = default!;
    [Inject] private ITransactionsState TransactionsState { get; set; } = default!;
    [Inject] private ITransactionEntryService<T> TransactionEntryService { get; set; } = default!;

    [Parameter] public T Transaction { get; set; } = default!;
    [Parameter] public bool Show { get; set; }
    [Parameter] public EventCallback<bool> ShowChanged { get; set; }
    [Parameter] public EventCallback OnConfirmClicked { get; set; }
    [Parameter] public EventCallback OnCancelClicked { get; set; }

    private ResponseModelBase<int>? _currentResponse;
    private bool _saving;
    private EntryType EntryType =>
        Transaction switch
        {
            PaymentEntryViewModel payment => EntryType.Payments,
            TransferEntryViewModel transfer => EntryType.Transfers,
            DepositEntryViewModel deposit => EntryType.Deposits,
            _ => EntryType.Payments
        };

    private T? _editTransaction; 

    private List<AccountLookupViewModel>? _accounts;
    private List<PayeeLookupViewModel>? _payees;

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LookupListState.InitializeAsync();
            _accounts = LookupListState.GetLookupItems<AccountLookupViewModel>().ToList();
            _payees = LookupListState.GetLookupItems<PayeeLookupViewModel>().ToList();
        }
    }

    public override async Task SetParametersAsync(ParameterView parameters)
    {
        await base.SetParametersAsync(parameters);

        if (parameters.TryGetValue(nameof(Transaction), out T? transaction))
        {
            _editTransaction = (T?)transaction?.Clone();
        }

        await base.SetParametersAsync(ParameterView.Empty);
    }

    private async Task ConfirmEditTransaction()
    {
        if(_editTransaction is null) { return; }
        _saving = true;

        // TODO: Debug using WASM since it seems to work on Server
        // Also remember to update other lookup items in the view model (e.g. DepositReason), so maybe rename to UpdateLookupNames
        var lookups = (_accounts ?? new()).OfType<ILookupItemViewModel>().Union(_payees ?? new());
        _editTransaction.UpdateAccountName(lookups);
        _currentResponse = await TransactionEntryService.UpdateTransactionAsync(_editTransaction);
        _saving = false;
        if (_currentResponse.WasError)
        {
            await InvokeAsync(StateHasChanged);
            return;
        }

        Transaction.Patch(_editTransaction);
        TransactionsState.NotifyTransactionsChanged();

        Show = false;

        if (ShowChanged.HasDelegate)
        {
            await ShowChanged.InvokeAsync(Show);
        }
        if (OnConfirmClicked.HasDelegate)
        {
            await OnConfirmClicked.InvokeAsync();
        }
    }

    private async Task HandleCancelClicked()
    {
        Show = false;
        if (OnCancelClicked.HasDelegate)
        {
            await OnCancelClicked.InvokeAsync();
        }
        if (ShowChanged.HasDelegate)
        {
            await ShowChanged.InvokeAsync(Show);
        }
    }
}
