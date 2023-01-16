using FinanMan.Shared.AccountSummaryModels;
using FinanMan.Shared.General;
using FinanMan.Shared.LookupModels;

namespace FinanMan.Shared.ServiceInterfaces;
public interface IAccountService
{
    Task<ResponseModel<IEnumerable<AccountLookupViewModel>>?> GetAccountsAsync(CancellationToken ct = default);
    Task<ResponseModel<AccountLookupViewModel>?> GetAccountAsync(int accountId, CancellationToken ct = default);
    Task<ResponseModel<IEnumerable<AccountSummaryViewModel>>?> GetAccountSummariesAsync(CancellationToken ct = default);
    Task<ResponseModel<AccountSummaryViewModel>?> GetAccountSummaryAsync(int accountId, CancellationToken ct = default);
    Task<ResponseModel<AccountLookupViewModel>?> CreateAccountAsync(AccountLookupViewModel accountModel, CancellationToken ct = default);
    Task<ResponseModel<AccountLookupViewModel>?> UpdateAccountAsync(AccountLookupViewModel accountModel, CancellationToken ct = default);
    Task<ResponseModelBase<int>?> DeleteAccountAsync(int accountId, CancellationToken ct = default);
}
