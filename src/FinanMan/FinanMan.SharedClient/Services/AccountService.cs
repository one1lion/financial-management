using FinanMan.Shared.AccountSummaryModels;
using FinanMan.Shared.General;
using FinanMan.Shared.LookupModels;
using FinanMan.Shared.ServiceInterfaces;
using System.Net.Http.Json;

namespace FinanMan.SharedClient.Services;

public class AccountService : IAccountService
{
    private readonly HttpClient _httpClient;

    public AccountService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<ResponseModel<IEnumerable<AccountLookupViewModel>>?> GetAccountsAsync(CancellationToken ct = default)
        => _httpClient.GetFromJsonAsync<ResponseModel<IEnumerable<AccountLookupViewModel>>>("api/Accounts", ct);

    public Task<ResponseModel<AccountLookupViewModel>?> GetAccountAsync(int accountId, CancellationToken ct = default)
        => _httpClient.GetFromJsonAsync<ResponseModel<AccountLookupViewModel>>($"api/Accounts/{accountId}", ct);

    public Task<ResponseModel<IEnumerable<AccountSummaryViewModel>>?> GetAccountSummariesAsync(CancellationToken ct = default)
        => _httpClient.GetFromJsonAsync<ResponseModel<IEnumerable<AccountSummaryViewModel>>>("api/Accounts/Summaries", ct);

    public Task<ResponseModel<AccountSummaryViewModel>?> GetAccountSummaryAsync(int accountId, CancellationToken ct = default)
        => _httpClient.GetFromJsonAsync<ResponseModel<AccountSummaryViewModel>>($"api/Accounts/Summaries/{accountId}", ct);
    
    public Task<ResponseModel<AccountLookupViewModel>?> CreateAccountAsync(AccountLookupViewModel accountModel, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel<AccountLookupViewModel>?> UpdateAccountAsync(AccountLookupViewModel accountModel, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModelBase<int>?> DeleteAccountAsync(int accountId, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
