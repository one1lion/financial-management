using FinanMan.Database;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.ServiceInterfaces;
using FinanMan.SharedServer.Services;
using FinanMan.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Internal;
using Moq;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FinanMan.Tests.BusinessLogicTesting;

public class DepositTransactionEntryServiceTests
{
    private readonly DepositEntryViewModelValidator _dataEntryValidator = new DepositEntryViewModelValidator();

    #region Helpers
    private (Mock<IDbContextFactory<FinanManContext>> DbContextFactory, FinanManContext Context, TransactionEntryService<DepositEntryViewModel> Sut) PrepareServiceUnderTest(CancellationToken ct)
    {
        var (dbContextFactory, context) = MockDataHelpers.PrepareDbContext(ct);

        // Prepare a new Deposit Entry Service instance
        var sut = new TransactionEntryService<DepositEntryViewModel>(dbContextFactory.Object, _dataEntryValidator);

        return (dbContextFactory, context, sut);
    }
    #endregion Helpers

    [Fact]
    public async Task AddDeposit_AddsToTransactionCollection()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var ct = cts.Token;

        var (dbContextFactory, context, sut) = PrepareServiceUnderTest(ct);

        // Prepare the data to be added
        var toAdd = new DepositEntryViewModel()
        {
            TransactionDate = new DateTime(2022, 1, 1),
            TargetAccountValueText = 1.ToString(),
            DepositReasonValueText = 1.ToString(),
            Amount = 239184
        };

        // Act
        var result = await sut.AddTransactionData(toAdd, ct);

        // Assert
        // this is if there only a few
        Assert.NotNull(result);
        Assert.False(result.WasError);

        // Get the new record back from in-memory db
        var newDeposit = context.Transactions.Include(e => e.Deposit).FirstOrDefault(e => e.Id == result.RecordId);

        // The records (Transaction and Deposit) exists in the database
        Assert.NotNull(newDeposit);
        Assert.NotNull(newDeposit.Deposit);

        Assert.Equal(toAdd.TransactionDate, newDeposit.TransactionDate);
        Assert.Equal(toAdd.Memo, newDeposit.Memo);
        Assert.Equal(toAdd.DepositReasonId, newDeposit.Deposit.DepositReasonId);
        Assert.Equal(toAdd.Amount, newDeposit.Deposit.Amount);
    }

    [Fact]
    public async Task GetAllDepositEntries_ReturnsAllDepositEntries()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var ct = cts.Token;

        var (dbContextFactory, context, sut) = PrepareServiceUnderTest(ct);

        // Mock the primary tables/entities that is going to be used by the service
        var transactions = MockDataHelpers.GenerateDepositTransactions(1, 20);
        await context.Transactions.AddRangeAsync(transactions);
        await context.SaveChangesAsync(ct);
        
        var deposits = transactions.Select(t => t.Deposit).ToList();
        transactions.ForEach(t => t.Deposit = null);

        // Act
        var result = await sut.GetTransactionData(ct: ct);
        
        // Assert

    }
}
