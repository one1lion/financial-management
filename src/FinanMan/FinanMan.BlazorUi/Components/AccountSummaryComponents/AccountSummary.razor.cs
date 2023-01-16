using FinanMan.Shared.AccountSummaryModels;
using FinanMan.Shared.General;
using System.Diagnostics;

namespace FinanMan.BlazorUi.Components.AccountSummaryComponents;

public partial class AccountSummary : IDisposable
{
    [Inject] private IAccountService AccountService { get; set; } = default!;

    private ResponseModel<IEnumerable<AccountSummaryViewModel>>? _accountSummariesResp;
    private CancellationTokenSource? _cts;
    
    protected override async Task OnInitializedAsync()
    {
        _cts = new CancellationTokenSource();
        _accountSummariesResp = await AccountService.GetAccountSummariesAsync(_cts.Token);
    }

    public void Dispose()
    {
        _cts?.Cancel();
    }
}
