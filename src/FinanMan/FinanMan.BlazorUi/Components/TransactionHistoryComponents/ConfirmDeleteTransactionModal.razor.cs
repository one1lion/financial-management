using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.General;

namespace FinanMan.BlazorUi.Components.TransactionHistoryComponents;

public partial class ConfirmDeleteTransactionModal<T>
    where T : class, ITransactionDataEntryViewModel
{
    [Inject] private ITransactionsState TransactionsState { get; set; } = default!;
    [Inject] private ITransactionEntryService<T> TransactionEntryService { get; set; } = default!;

    [Parameter] public T Transaction { get; set; } = default!;
    [Parameter] public bool Show { get; set; }
    [Parameter] public EventCallback<bool> ShowChanged { get; set; }
    [Parameter] public EventCallback OnConfirmClicked { get; set; }
    [Parameter] public EventCallback OnCancelClicked { get; set; }

    private ResponseModelBase<int>? _currentResponse;
    private bool _deleting;
    
    private string PayeeValue => Transaction switch
    {
        PaymentEntryViewModel payment => payment.PayeeName,
        TransferEntryViewModel transfer => transfer.TargetAccountName,
        DepositEntryViewModel deposit => deposit.DepositReasonDisplayText,
        _ => "&nbsp;"
    } ?? string.Empty;

    private async Task DeleteTransaction()
    {
        _deleting = true;
        var forDeleteId = Transaction.TransactionId;
        _currentResponse = await TransactionEntryService.DeleteTransactionAsync(forDeleteId);

        _deleting = true;
        if (_currentResponse?.WasError ?? false)
        {
            await InvokeAsync(StateHasChanged);
            return;
        }

        TransactionsState.RemoveTransaction(forDeleteId);
        Show = false;
        if (ShowChanged.HasDelegate)
        {
            await ShowChanged.InvokeAsync(Show);
        }
        if(OnConfirmClicked.HasDelegate)
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
