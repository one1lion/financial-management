using FinanMan.Shared.AccountSummaryModels;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.General;
using FinanMan.Shared.LookupModels;
using FinanMan.Shared.ServiceInterfaces;
using System.Collections.Concurrent;
using System.Net.Http.Json;

namespace FinanMan.SharedClient.Services;

public class AccountService : IAccountService
{
    private readonly HttpClient _httpClient;

    public readonly ICollection<object> MyCollection;

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
    
    public async Task<ResponseModelBase<int>?> CreateAccountAsync(AccountLookupViewModel accountModel, CancellationToken ct = default)
    {
        var retResp = new ResponseModelBase<int>();
        try
        {
            var resp = await _httpClient.PostAsJsonAsync("api/Accounts", accountModel, ct);
            if (resp.IsSuccessStatusCode)
            {
                retResp = await resp.Content.ReadFromJsonAsync<ResponseModelBase<int>>(cancellationToken: ct);
            }
            else
            {
                retResp.AddError($"The request to create the account failed.  The server responded with status: {resp.StatusCode} - {resp.ReasonPhrase}");
            }
        }
        catch (Exception ex)
        {
            retResp ??= new();

            var errMessage = ex switch
            {
                TaskCanceledException or OperationCanceledException => "The task was canceled.",
                // TODO: Do something with successStatus here - that is, we got a success response from the server,
                //       but deserializing the response model ended up causing an exception
                _ => "The create account operation failed unexpectedly."
            };

            retResp.AddError(errMessage);

#if DEBUG
            retResp.AddError(ex);
#endif
        }
        return retResp;
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
