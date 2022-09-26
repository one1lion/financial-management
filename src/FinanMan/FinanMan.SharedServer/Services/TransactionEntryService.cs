using FinanMan.Database;
using FinanMan.Database.Models.Tables;
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
    //private readonly Lazy<TransactionViewModelValidator<TDataEntryViewModel>> _modelValidator;
    private readonly TransactionViewModelValidator<TDataEntryViewModel> _modelValidator;

    public TransactionEntryService(IDbContextFactory<FinanManContext> dbContextFactory, TransactionViewModelValidator<TDataEntryViewModel> modelValidator) //Lazy<TransactionViewModelValidator<TDataEntryViewModel>> modelValidator)
    {
        _dbContextFactory = dbContextFactory;
        _modelValidator = modelValidator;
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

            var newTransaction = new Transaction()
            {
                TransactionDate = dataEntryViewModel.TransactionDate!.Value,
                AccountId = dataEntryViewModel.AccountId!.Value,
                Memo = dataEntryViewModel.Memo,
                PostingDate = dataEntryViewModel.PostedDate,
                DateEntered = DateTime.UtcNow
            };

            switch (dataEntryViewModel)
            {
                case DepositEntryViewModel depositEntryViewModel:
                    newTransaction.Deposit = new Deposit()
                    {
                        DepositReasonId = depositEntryViewModel.DepositReasonId!.Value,
                        Amount = depositEntryViewModel.Amount!.Value
                    };
                    break;
                case PaymentEntryViewModel paymentEntryViewModel:
                    break;
                case TransferEntryViewModel transferEntryViewModel:
                    break;
                default:
                    throw new NotImplementedException();
            }

            await context.Transactions.AddAsync(newTransaction, ct);
            retResp.RecordCount = await context.SaveChangesAsync(ct);
            await trans.CommitAsync(ct);
            retResp.RecordId = newTransaction.Id;
        }
        catch (Exception ex)
        {
            if (trans is not null) { await trans.RollbackAsync(); }
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
            if (trans is not null) { await trans.DisposeAsync(); }
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
