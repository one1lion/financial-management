using FinanMan.Database;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.Extensions;
using FinanMan.Shared.General;
using FinanMan.Shared.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace FinanMan.SharedServer.Services;

public class TransactionEntryService<TDataEntryViewModel> : ITransactionEntryService<TDataEntryViewModel>
    where TDataEntryViewModel : class, ITransactionDataEntryViewModel
{
    private readonly IDbContextFactory<FinanManContext> _dbContextFactory;
    private readonly TransactionViewModelValidator<TDataEntryViewModel> _modelValidator;
    private readonly ILoggerFactory _loggerFactory;

    public TransactionEntryService(
        IDbContextFactory<FinanManContext> dbContextFactory, 
        TransactionViewModelValidator<TDataEntryViewModel> modelValidator,
        ILoggerFactory loggerFactory)
    {
        _dbContextFactory = dbContextFactory;
        _modelValidator = modelValidator;
        _loggerFactory = loggerFactory;
    }

    public Task<ResponseModel<List<TDataEntryViewModel>>> GetTransactionsAsync(int startRecord = 0, int pageSize = 100, DateTime? asOfDate = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel<TDataEntryViewModel>> GetTransactionAsync(int id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
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
        FinanManContext context = default!;
        IDbContextTransaction? trans = null;

        try
        {
            ct.ThrowIfCancellationRequested();
            context = await _dbContextFactory.CreateDbContextAsync(ct);
            trans = await context.Database.BeginTransactionAsync(ct);

            var newTransaction = dataEntryViewModel.ToEntityModel();

            await context.Transactions.AddAsync(newTransaction, ct);
            retResp.RecordCount = await context.SaveChangesAsync(ct);
            await trans.CommitAsync(ct);
            retResp.RecordId = newTransaction.Id;
        }
        catch (Exception ex)
        {
            if (trans is not null) { await trans.RollbackAsync(); }
            // Add an error to the return response
            retResp.AddError(ex switch
            {
                TaskCanceledException _ => "The task to save the deposit was canceled",
                OperationCanceledException _ => "The task to save the deposit was canceled",
                _ => "An unexpected error occurred while saving the deposit"
            });
        }
        finally
        {
            if (trans is not null) { await trans.DisposeAsync(); }
        }

        return retResp;
    }

    public Task<ResponseModelBase<int>> UpdateTransactionAsync(TDataEntryViewModel dataEntryViewModel, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseModelBase<int>> DeleteTransactionAsync(int id, CancellationToken ct = default)
    {
        var retResp = new ResponseModelBase<int>() { RecordId = id };

        FinanManContext? context;
        IDbContextTransaction? trans = null;
        try
        {
            context = await _dbContextFactory.CreateDbContextAsync(ct);

            var toDelete = await context.Transactions.FirstOrDefaultAsync(x => x.Id == id);
            if(toDelete is null)
            {
                retResp.AddError("The transaction to delete was not found");
                return retResp;
            }

            trans = await context.Database.BeginTransactionAsync(ct);
            // TODO: Set purge lifetime in application options
            toDelete.PurgeDate = DateTime.UtcNow.AddDays(15);
            retResp.RecordCount = await context.SaveChangesAsync(ct);
            await trans.CommitAsync(ct);
        }
        catch (Exception ex)
        {
            if(trans is not null) { await trans.RollbackAsync(); }
            retResp.AddError("The transaction could not be deleted.");
            var logger = _loggerFactory.CreateLogger<TransactionEntryService<TDataEntryViewModel>>();
            logger.LogError(ex, "An error occurred while deleting transaction {id}", id);
        }
        finally
        {
            if(trans is not null) { await trans.DisposeAsync(); }
        }

        return retResp;
    }
}
