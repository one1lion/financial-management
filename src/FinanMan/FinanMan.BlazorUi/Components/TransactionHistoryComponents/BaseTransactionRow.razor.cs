using FinanMan.Shared.DataEntryModels;

namespace FinanMan.BlazorUi.Components.TransactionHistoryComponents;
public abstract partial class BaseTransactionRow<T> : ComponentBase
    where T : class, ITransactionDataEntryViewModel
{
    [Inject] private ITransactionEntryService<T> TransactionEntryService { get; set; } = default!;

    [Parameter] public T Transaction { get; set; } = default!;

    private bool _showConfirmMarkPending;
    private bool _showPostedDateEntry;
    private DateTime? _enteredPostedDate;

    protected virtual async Task TogglePending(bool force = false)
    {
        _showConfirmMarkPending = false;
        _showPostedDateEntry = false;
        
        if (Transaction.PostedDate.HasValue)
        {
            _enteredPostedDate = Transaction.PostedDate;
            // TODO: If the passed in Transaction has a posted date, then display a modal to confirm the change unless the user has already selected to never ask
            if (force)
            {
                Transaction.PostedDate = null;
                await TransactionEntryService.UpdateTransactionAsync(Transaction);
                return;
            }
            _showConfirmMarkPending = true;
            return;
        }
        else
        {
            // TODO: Otherwise, if the passed in Transaction does not have a posted date, display a modal asking the user to enter the posted date
            if (force && _enteredPostedDate.HasValue)
            {
                Transaction.PostedDate = _enteredPostedDate;
                await TransactionEntryService.UpdateTransactionAsync(Transaction);
                return;
            }
            _showPostedDateEntry = true;
            return;
        }
    }
}
