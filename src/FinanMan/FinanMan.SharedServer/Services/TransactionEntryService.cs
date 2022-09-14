using FinanMan.Database;
using FinanMan.Database.Models.Tables;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.General;
using FinanMan.Shared.ServiceInterfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;

namespace FinanMan.SharedServer.Services;

public class TransactionEntryService<TDataEntryViewModel> : ITransactionEntryService<TDataEntryViewModel>
    where TDataEntryViewModel : class, ITransactionDataEntryViewModel
{
    private readonly IDbContextFactory<FinanManContext> _dbContextFactory;

    public TransactionEntryService(IDbContextFactory<FinanManContext> dbContextFactory)
    {
        _dbContextFactory = dbContextFactory;
    }

    public async Task<ResponseModel<List<TDataEntryViewModel>>> GetTransactionData(int startRecord = 0,
        int pageSize = 100, DateTime? asOfDate = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel<TDataEntryViewModel>> GetTransactionData(int id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseModelBase<int>> AddTransactionData(TDataEntryViewModel dataEntryViewModel,
        CancellationToken ct = default)
    {
        // simple implemenation as example
        // obviously this is not really doubt you creat dbcontext on every call etc and not even model you want but will give the idea
        var context = await _dbContextFactory.CreateDbContextAsync(ct);
        context.Transactions.Add(new Transaction
        {
            Id = 532,
            Account = new Account
            {
                Name = "WhatEver"
            }
        });
        await context.SaveChangesAsync(ct);
        return default;
    }

    public Task<ResponseModelBase<int>> UpdateTransactionData(TDataEntryViewModel dataEntryViewModel,
        CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModelBase<int>> DeleteTransactionData(int id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}