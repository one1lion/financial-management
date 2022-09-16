using FinanMan.Database;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.General;
using FinanMan.Shared.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;

namespace FinanMan.SharedServer.Services;

public class TransactionEntryService<TDataEntryViewModel> : ITransactionEntryService<TDataEntryViewModel>
    where TDataEntryViewModel : class, ITransactionDataEntryViewModel
{
    private readonly IDbContextFactory<FinanManContext> _dbContextFactory;

    public TransactionEntryService(IDbContextFactory<FinanManContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<ResponseModel<List<TDataEntryViewModel>>> GetTransactionData(int startRecord = 0, int pageSize = 100, DateTime? asOfDate = null, CancellationToken ct = default)
    {

        throw new NotImplementedException();
    }

    public Task<ResponseModel<TDataEntryViewModel>> GetTransactionData(int id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseModelBase<int>> AddTransactionData(TDataEntryViewModel dataEntryViewModel, CancellationToken ct = default)
    {
        var retResp = new ResponseModelBase<int>();
        // TODO: Validate the view model
        // if viewmodel is not valid, then add an error to retResp and return

        // Otherwise
        FinanManContext context = default!;
        IDbContextTransaction? trans = null;

        try
        {
            ct.ThrowIfCancellationRequested();
            context = await _dbContextFactory.CreateDbContextAsync(ct);
            trans = await context.Database.BeginTransactionAsync(ct);


            await context.SaveChangesAsync(ct);
            await trans.CommitAsync(ct);
        }
        catch (Exception ex)
        {
            if(trans is not null) { await trans.RollbackAsync(); }
            // TODO: Add an error to the return response
            retResp.AddError(ex switch
            { 
                TaskCanceledException _ => "The task to save the deposit was canceled",
                OperationCanceledException _ => "The task to save the deposit was canceled",
                _ => "An unexpected error occurred while saving the deposit"
            });
        }
        finally
        {
            if(trans is not null) { await trans.DisposeAsync(); }
        }

        return retResp;
    }

    public Task<ResponseModelBase<int>> UpdateTransactionData(TDataEntryViewModel dataEntryViewModel, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModelBase<int>> DeleteTransactionData(int id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
