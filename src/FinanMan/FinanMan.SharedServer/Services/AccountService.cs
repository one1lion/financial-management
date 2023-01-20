using FinanMan.Database;
using FinanMan.Database.Models.Tables;
using FinanMan.Shared.AccountSummaryModels;
using FinanMan.Shared.General;
using FinanMan.Shared.LookupModels;
using FinanMan.Shared.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;

namespace FinanMan.SharedServer.Services;

public class AccountService : IAccountService
{
    private readonly IDbContextFactory<FinanManContext> _dbContextFactory;

    public AccountService(IDbContextFactory<FinanManContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }
    
    public async Task<ResponseModel<IEnumerable<AccountLookupViewModel>>?> GetAccountsAsync(CancellationToken ct = default)
    {
        var retResp = new ResponseModel<IEnumerable<AccountLookupViewModel>>();
        using var context = await _dbContextFactory.CreateDbContextAsync(ct);

        retResp.Data = await context.Accounts.AsNoTracking()
            .Select(x => x.ToViewModel())
            .ToListAsync(ct);

        return retResp;
    }

    public async Task<ResponseModel<AccountLookupViewModel>?> GetAccountAsync(int accountId, CancellationToken ct = default)
    {
        var retResp = new ResponseModel<AccountLookupViewModel>();
        using var context = await _dbContextFactory.CreateDbContextAsync(ct);

        

        return retResp;
    }

    public async Task<ResponseModel<IEnumerable<AccountSummaryViewModel>>?> GetAccountSummariesAsync(CancellationToken ct = default)
    {
        var retResp = new ResponseModel<IEnumerable<AccountSummaryViewModel>>();
        using var context = await _dbContextFactory.CreateDbContextAsync(ct);

        var accounts = context.Accounts.AsNoTracking()
            .Include(x => x.AccountType)
            .Include(x => x.Transactions)
                .ThenInclude(x => x.Payment)
                    .ThenInclude(x => x.PaymentDetails)
            .Include(x => x.Transactions)
                .ThenInclude(x => x.Deposit)
            .Include(x => x.Transactions)
                .ThenInclude(x => x.Transfer)
            .Include(x => x.Transfers)
                .ThenInclude(x => x.Transaction);

        var retModel = new List<AccountSummaryViewModel>();

        foreach(var account in accounts)
        {
            AccountSummaryViewModel accountSummary = account.ToAccountSummaryModel();
            retModel.Add(accountSummary);
        }

        retResp.Data = retModel;
        retResp.RecordCount = retModel.Count;

        return retResp;
    }

    public async Task<ResponseModel<AccountSummaryViewModel>?> GetAccountSummaryAsync(int accountId, CancellationToken ct = default)
    {
        var retResp = new ResponseModel<AccountSummaryViewModel>();
        using var context = await _dbContextFactory.CreateDbContextAsync(ct);

        retResp.Data = (await context.Accounts.AsNoTracking()
            .Include(x => x.Transactions)
                .ThenInclude(x => x.Payment)
                .ThenInclude(x => x.PaymentDetails)
            .Include(x => x.Transactions)
                .ThenInclude(x => x.Deposit)
            .Include(x => x.Transactions)
                .ThenInclude(x => x.Transfer)
            .FirstOrDefaultAsync(ct))?
            .ToAccountSummaryModel();

        return retResp;
    }

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

