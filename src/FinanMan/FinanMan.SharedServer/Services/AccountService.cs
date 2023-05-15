using FinanMan.Database;
using FinanMan.Shared.AccountSummaryModels;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.General;
using FinanMan.Shared.LookupModels;
using FinanMan.Shared.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinanMan.SharedServer.Services;

public class AccountService : IAccountService
{
    private readonly IDbContextFactory<FinanManContext> _dbContextFactory;
    private readonly ILoggerFactory _loggerFactory;

    public AccountService(IDbContextFactory<FinanManContext> dbContextFactory, ILoggerFactory loggerFactory)
    {
        _dbContextFactory = dbContextFactory;
        _loggerFactory = loggerFactory;
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

        retResp.Data = (await context.Accounts.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == accountId, cancellationToken: ct))?.ToViewModel();

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

        foreach (var account in accounts)
        {
            var accountSummary = account.ToAccountSummaryModel();
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

    public async Task<ResponseModelBase<int>?> CreateAccountAsync(AccountLookupViewModel accountModel, CancellationToken ct = default)
    {
        var retResp = new ResponseModelBase<int>();

        // TODO: Create a model validator and validate the view model

        // If valid, perform the add
        try
        {
            ct.ThrowIfCancellationRequested();
            using var context = await _dbContextFactory.CreateDbContextAsync(ct);
            using var trans = await context.Database.BeginTransactionAsync(ct);

            var newAccount = accountModel.Item;
            newAccount.AccountType = default!;
            await context.Accounts.AddAsync(newAccount, ct);
            retResp.RecordCount = await context.SaveChangesAsync(ct);
            await trans.CommitAsync(ct);
            retResp.RecordId = newAccount.Id;
        }
        catch (Exception ex)
        {
            // Add an error to the return response
            var msg = ex switch
            {
                TaskCanceledException _ => "The task to save the account was canceled",
                OperationCanceledException _ => "The task to save the account was canceled",
                _ => "An unexpected error occurred while saving the account"
            };
            retResp.AddError(msg);

            var logger = _loggerFactory.CreateLogger<AccountService>();
            logger.LogError(ex, "An error occurred while trying to add a new account: {msg}", msg);
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

