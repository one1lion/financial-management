using System.Diagnostics;

namespace FinanMan.BlazorUi.Components.AccountSummaryComponents;

public partial class AccountSummary : IDisposable
{
    [Inject] private IAccountService AccountService { get; set; } = default!;

    private CancellationTokenSource? _cts;
    
    protected override async Task OnInitializedAsync()
    {
        _cts = new CancellationTokenSource();
        var accountData = await AccountService.GetAccountSummariesAsync(_cts.Token);
        Debug.WriteLine(accountData.WasError);
    }

    public void Dispose()
    {
        _cts?.Cancel();
    }
}
