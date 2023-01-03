using FinanMan.Database;
using FinanMan.Database.Models.Shared;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.Extensions;
using FinanMan.Shared.General;
using FinanMan.Shared.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FinanMan.SharedServer.Services;

public class TransactionEntryService<TDataEntryViewModel> : ITransactionEntryService<TDataEntryViewModel>
    where TDataEntryViewModel : class, ITransactionDataEntryViewModel
{
    private readonly IDbContextFactory<FinanManContext> _dbContextFactory;
    private readonly TransactionViewModelValidator<TDataEntryViewModel> _modelValidator;
    private readonly ILoggerFactory _loggerFactory;
    private readonly TransactionType _transactionType;

    public TransactionEntryService(
        IDbContextFactory<FinanManContext> dbContextFactory,
        TransactionViewModelValidator<TDataEntryViewModel> modelValidator,
        ILoggerFactory loggerFactory)
    {
        _dbContextFactory = dbContextFactory;
        _modelValidator = modelValidator;
        _loggerFactory = loggerFactory;
        var dummyModel = Activator.CreateInstance<TDataEntryViewModel>();
        dummyModel.TransactionDate = DateTime.UtcNow;
        dummyModel.AccountId = 0;
        _transactionType = dummyModel.ToEntityModel().TransactionType;
    }

    public async Task<ResponseModel<List<TDataEntryViewModel>>?> GetTransactionsAsync(ushort startRecord = 0, ushort pageSize = 100, DateTime? asOfDate = null, CancellationToken ct = default)
    {
        pageSize = Math.Clamp(pageSize, (ushort)5, (ushort)500);
        var retResp = new ResponseModel<List<TDataEntryViewModel>>();

        try
        {
            using var context = await _dbContextFactory.CreateDbContextAsync(ct);

            var transactions = context.Transactions.AsNoTracking()
                .Include(x => x.Account)
                .Include(x => x.Deposit)
                .Include(x => x.Payment)
                .ThenInclude(x => x.PaymentDetails)
                .Include(x => x.Payment)
                .ThenInclude(x => x.Payee)
                .Include(x => x.Transfer)
                .Where(x =>
                    _transactionType == TransactionType.Deposit && x.Deposit != null
                    || _transactionType == TransactionType.Payment && x.Payment != null
                    || _transactionType == TransactionType.Transfer && x.Transfer != null)
                .AsQueryable();

            if (asOfDate.HasValue)
            {
                transactions = transactions.Where(x => x.TransactionDate <= asOfDate.Value);
            }

            var pagedResult = await transactions
                .OrderByDescending(x => x.TransactionDate)
                .Skip(startRecord)
                .Take(pageSize)
                .ToListAsync(ct);

            retResp.Data = pagedResult.Select(x => x.ToViewModel())
                .OfType<TDataEntryViewModel>()
                .ToList();
        }
        catch (Exception ex)
        {
            retResp.AddError("An error occurred while trying to get the list of transaction.");

            var logger = _loggerFactory.CreateLogger<TransactionEntryService<TDataEntryViewModel>>();
            logger.LogError(ex, "An error occurred while trying to get the list of {transType}.", typeof(TDataEntryViewModel));
        }

        return retResp;
    }

    public async Task<ResponseModel<TDataEntryViewModel>?> GetTransactionAsync(int id, CancellationToken ct = default)
    {
        var retResp = new ResponseModel<TDataEntryViewModel>();

        try
        {
            using var context = await _dbContextFactory.CreateDbContextAsync(ct);

            var transaction = await context.Transactions.AsNoTracking()
                .Include(x => x.Account)
                .Include(x => x.Deposit)
                .Include(x => x.Payment)
                .ThenInclude(x => x.PaymentDetails)
                .Include(x => x.Transfer)
                .FirstOrDefaultAsync(x => x.Id == id, ct);

            retResp.Data = (TDataEntryViewModel?)transaction?.ToViewModel();
        }
        catch (Exception ex)
        {
            retResp.AddError("An error occurred while trying to get the specified transaction.");

            var logger = _loggerFactory.CreateLogger<TransactionEntryService<TDataEntryViewModel>>();
            logger.LogError(ex, "An error occurred while trying to get the {transType} with ID {id}.", typeof(TDataEntryViewModel), id);
        }

        return retResp;
    }

    public async Task<ResponseModelBase<int>> AddTransactionAsync(TDataEntryViewModel dataEntryViewModel, CancellationToken ct = default)
    {
        var retResp = new ResponseModelBase<int>();

        // Validate the view model
        var validResult = await _modelValidator.ValidateAsync(dataEntryViewModel, ct);
        if (!validResult.IsValid)
        {
            retResp.AddErrors(validResult.Errors);
            return retResp;
        }

        // Otherwise, perform the add
        try
        {
            ct.ThrowIfCancellationRequested();
            using var context = await _dbContextFactory.CreateDbContextAsync(ct);
            using var trans = await context.Database.BeginTransactionAsync(ct);

            var newTransaction = dataEntryViewModel.ToEntityModel();

            await context.Transactions.AddAsync(newTransaction, ct);
            retResp.RecordCount = await context.SaveChangesAsync(ct);
            await trans.CommitAsync(ct);
            retResp.RecordId = newTransaction.Id;
        }
        catch (Exception ex)
        {
            // Add an error to the return response
            var msg = ex switch
            {
                TaskCanceledException _ => "The task to save the deposit was canceled",
                OperationCanceledException _ => "The task to save the deposit was canceled",
                _ => "An unexpected error occurred while saving the deposit"
            };
            retResp.AddError(msg);

            var logger = _loggerFactory.CreateLogger<TransactionEntryService<TDataEntryViewModel>>();
            logger.LogError(ex, "An error occurred while trying to add a new {transType}: {msg}", typeof(TDataEntryViewModel), msg);
        }

        return retResp;
    }

    public async Task<ResponseModelBase<int>> UpdateTransactionAsync(TDataEntryViewModel dataEntryViewModel, CancellationToken ct = default)
    {
        var retResp = new ResponseModelBase<int>();

        // Validate the view model
        var validResult = await _modelValidator.ValidateAsync(dataEntryViewModel, ct);
        if (!validResult.IsValid)
        {
            retResp.AddErrors(validResult.Errors);
            return retResp;
        }

        // Otherwise, perform the update
        try
        {
            ct.ThrowIfCancellationRequested();
            using var context = await _dbContextFactory.CreateDbContextAsync(ct);
            using var trans = await context.Database.BeginTransactionAsync(ct);

            // Find the matching record in the database
            if (!await context.Transactions.AnyAsync(x => x.Id == dataEntryViewModel.TransactionId, ct))
            {
                retResp.AddError("The specified transaction does not exist.");
                return retResp;
            }

            var updatedRecord = dataEntryViewModel.ToEntityModel();

            context.Transactions.Update(updatedRecord);

            retResp.RecordCount = await context.SaveChangesAsync(ct);
            await trans.CommitAsync(ct);
            retResp.RecordId = updatedRecord.Id;
        }
        catch (Exception ex)
        {
            // Add an error to the return response
            var msg = ex switch
            {
                TaskCanceledException _ => "The task to save the deposit was canceled",
                OperationCanceledException _ => "The task to save the deposit was canceled",
                _ => "An unexpected error occurred while saving the deposit"
            };
            retResp.AddError(msg);

            var logger = _loggerFactory.CreateLogger<TransactionEntryService<TDataEntryViewModel>>();
            logger.LogError(ex, "An error occurred while trying to add a new {transType}: {msg}", typeof(TDataEntryViewModel), msg);
        }

        return retResp;
    }

    public async Task<ResponseModelBase<int>> DeleteTransactionAsync(int id, CancellationToken ct = default)
    {
        var retResp = new ResponseModelBase<int>() { RecordId = id };

        try
        {
            using var context = await _dbContextFactory.CreateDbContextAsync(ct);

            using var trans = await context.Database.BeginTransactionAsync(ct);
            var toDelete = await context.Transactions.FirstOrDefaultAsync(x => x.Id == id, cancellationToken: ct);
            if (toDelete is null)
            {
                retResp.AddError("The transaction to delete was not found");
                return retResp;
            }

            // TODO: Set purge lifetime in application options
            toDelete.PurgeDate = DateTime.UtcNow.AddDays(15);
            retResp.RecordCount = await context.SaveChangesAsync(ct);
            await trans.CommitAsync(ct);
        }
        catch (Exception ex)
        {
            retResp.AddError("The transaction could not be deleted.");
            var logger = _loggerFactory.CreateLogger<TransactionEntryService<TDataEntryViewModel>>();
            logger.LogError(ex, "An error occurred while deleting transaction {id}", id);
        }

        return retResp;
    }
}
