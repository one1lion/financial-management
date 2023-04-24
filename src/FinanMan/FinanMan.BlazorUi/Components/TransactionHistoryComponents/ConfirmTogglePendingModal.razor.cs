using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.General;

namespace FinanMan.BlazorUi.Components.TransactionHistoryComponents;

public partial class ConfirmTogglePendingModal<T>
    where T : class, ITransactionDataEntryViewModel
{
    [Inject] private ITransactionsState TransactionsState { get; set; } = default!;
    [Inject] private ITransactionEntryService<T> TransactionEntryService { get; set; } = default!;

    [Parameter] public T Transaction { get; set; } = default!;
    [Parameter] public bool Show { get; set; }
    [Parameter] public EventCallback<bool> ShowChanged { get; set; }
    [Parameter] public EventCallback OnConfirmClicked { get; set; }
    [Parameter] public EventCallback OnCancelClicked { get; set; }

    private ElementReference _postedDateInput;
    private DateTime? _enteredPostedDate;
    private ResponseModelBase<int>? _currentResponse;
    private bool _doSetFocus = true;

    private bool IsCurrentlyPending => !Transaction.PostedDate.HasValue;

    private string PayeeValue => Transaction switch
    {
        PaymentEntryViewModel payment => payment.PayeeName,
        TransferEntryViewModel transfer => transfer.TargetAccountName,
        DepositEntryViewModel deposit => deposit.DepositReasonDisplayText,
        _ => "&nbsp;"
    } ?? string.Empty;

    public override Task SetParametersAsync(ParameterView parameters)
    {
        _doSetFocus = !Show && parameters.TryGetValue(nameof(Show), out bool newShow) && newShow;

        return base.SetParametersAsync(parameters);
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (Show && !Transaction.PostedDate.HasValue && _doSetFocus && _postedDateInput.Context is not null)
        {
            _doSetFocus = false;
            await _postedDateInput.FocusAsync();
        }
        await base.OnAfterRenderAsync(firstRender);
    }

    protected async Task TogglePending(bool force = false)
    {
        if (Transaction.PostedDate.HasValue)
        {
            _enteredPostedDate = Transaction.PostedDate;
            // TODO: Add a "never ask" option to the confirmation dialog for deleting a posted date (setting to pending)
            if (force)
            {
                var forUpdate = (T)Transaction.Clone();
                forUpdate.PostedDate = null;
                _currentResponse = await TransactionEntryService.UpdateTransactionAsync(forUpdate);
                if (!(_currentResponse?.WasError ?? false))
                {
                    Transaction.Patch(forUpdate);
                    Show = false;
                    TransactionsState.NotifyTransactionsChanged();
                    if (ShowChanged.HasDelegate)
                    {
                        await ShowChanged.InvokeAsync(false);
                    }
                    if (OnConfirmClicked.HasDelegate)
                    {
                        await OnConfirmClicked.InvokeAsync();
                    }
                }
                else
                {
                    await InvokeAsync(StateHasChanged);
                }
            }
        }
        else
        {
            if (force && _enteredPostedDate.HasValue)
            {
                _doSetFocus = true;
                var forUpdate = (T)Transaction.Clone();
                forUpdate.PostedDate = _enteredPostedDate;
                _currentResponse = await TransactionEntryService.UpdateTransactionAsync(forUpdate);
                // Make sure the dialog remains showing if there was an error
                if (!(_currentResponse?.WasError ?? false))
                {
                    Transaction.Patch(forUpdate);
                    Show = false;
                    if (ShowChanged.HasDelegate)
                    {
                        await ShowChanged.InvokeAsync(false);
                    }
                    if (OnConfirmClicked.HasDelegate)
                    {
                        await OnConfirmClicked.InvokeAsync();
                    }
                    TransactionsState.NotifyTransactionsChanged();
                }
                else
                {
                    await InvokeAsync(StateHasChanged);
                }
            }
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