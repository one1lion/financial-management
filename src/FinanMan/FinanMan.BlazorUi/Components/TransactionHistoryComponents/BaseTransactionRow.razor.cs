using FinanMan.Shared.DataEntryModels;

namespace FinanMan.BlazorUi.Components.TransactionHistoryComponents;
public abstract partial class BaseTransactionRow<T> : ComponentBase
    where T : class, ITransactionDataEntryViewModel
{
    [Parameter] public T Transaction { get; set; } = default!;

    private bool _showTogglePendingDialog;

    private string PayeeValue => Transaction switch
    {
        PaymentEntryViewModel payment => payment.PayeeName,
        TransferEntryViewModel transfer => transfer.TargetAccountName,
        DepositEntryViewModel deposit => deposit.DepositReasonDisplayText,
        _ => "&nbsp;"
    } ?? string.Empty;

    private Task HandlePendingValueClicked()
    {
        _showTogglePendingDialog = true;
        return Task.CompletedTask;
    }
}
