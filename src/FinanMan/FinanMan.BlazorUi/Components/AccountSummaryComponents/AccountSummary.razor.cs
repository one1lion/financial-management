using FinanMan.Shared.AccountSummaryModels;
using FinanMan.Shared.General;

namespace FinanMan.BlazorUi.Components.AccountSummaryComponents;

public partial class AccountSummary : IDisposable
{
    [Inject] private IAccountService AccountService { get; set; } = default!;
    [Inject] private ITransactionsState TransactionsState { get; set; } = default!;
    
    private ResponseModel<IEnumerable<AccountSummaryViewModel>>? _accountSummariesResp;
    private CancellationTokenSource? _cts;

    protected override async Task OnInitializedAsync()
    {
        await RefreshAccountSummaryAsync();
        TransactionsState.OnTransactionHistoryChanged += HandleTransactionHistoryChanged;
        await InvokeAsync(StateHasChanged);
    }

    private async Task HandleTransactionHistoryChanged()
    {
        await RefreshAccountSummaryAsync();
        await InvokeAsync(StateHasChanged);
    }

    private async Task RefreshAccountSummaryAsync()
    {
        _cts?.Cancel();
        _cts = new CancellationTokenSource();
        var thisToken = _cts.Token;
        try
        {
            _accountSummariesResp = await AccountService.GetAccountSummariesAsync(thisToken);
            await InvokeAsync(StateHasChanged);
        }
        catch (Exception ex)
        {
            switch(ex)
            {
                case TaskCanceledException:
                case OperationCanceledException:
                    if(!thisToken.IsCancellationRequested)
                    {
                        // This is potentially a HTTP request timeout
                        throw;
                    }
                    // This is us purposefully canceling the request
                    break;
                default:
                    throw;
            }
        }
    }

    public void Dispose()
    {
        _cts?.Cancel();
        TransactionsState.OnTransactionHistoryChanged -= HandleTransactionHistoryChanged;
    }
}
