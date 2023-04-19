using FinanMan.Shared.DataEntryModels;

namespace FinanMan.BlazorUi.Components.TransactionHistoryComponents;
public abstract partial class BaseTransactionRow<T> : ComponentBase
    where T : class, ITransactionDataEntryViewModel
{
    [Parameter] public T Transaction { get; set; } = default!;

    private bool _showTogglePendingDialog;
    private bool _showConfirmDeleteDialog;
    private bool _showEditDialog;

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

    private Task HandleEditClicked()
    {
        _showEditDialog = true;
        return Task.CompletedTask;
    }

    private Task HandleDeleteClicked()
    {
        _showConfirmDeleteDialog = true;
        return Task.CompletedTask;
    }
}
