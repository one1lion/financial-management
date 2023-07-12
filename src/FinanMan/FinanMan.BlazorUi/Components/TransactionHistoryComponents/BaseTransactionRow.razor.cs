using FinanMan.Database.Models.Tables;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.LookupModels;
using System.Diagnostics.CodeAnalysis;

namespace FinanMan.BlazorUi.Components.TransactionHistoryComponents;
public abstract partial class BaseTransactionRow<T> : ComponentBase
    where T : class, ITransactionDataEntryViewModel
{
    [Inject, AllowNull] private ILookupListState LookupListState { get; set; }
    [Parameter] public T Transaction { get; set; } = default!;

    private bool _showTogglePendingDialog;
    private bool _showConfirmDeleteDialog;
    private bool _showEditDialog;

    private string PayeeValue => Transaction switch
    {
        PaymentEntryViewModel payment => LookupListState.GetLookupItems<PayeeLookupViewModel>().FirstOrDefault(x => x.ValueText == payment.PayeeValueText)?.PayeeName,
        TransferEntryViewModel transfer => LookupListState.GetLookupItems<AccountLookupViewModel>().FirstOrDefault(x => x.ValueText == transfer.TargetAccountValueText)?.AccountName,
        DepositEntryViewModel deposit => LookupListState.GetLookupItems<LookupItemViewModel<LuDepositReason>>().FirstOrDefault(x => x.ValueText == deposit.DepositReasonValueText)?.DisplayText,
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
