using FinanMan.Database;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.Extensions;
using FinanMan.SharedServer.Services;
using FinanMan.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
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
    public async Task GetAllDepositEntries_ReturnsAllDepositEntries()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var ct = cts.Token;

        var (dbContextFactory, context, sut) = PrepareServiceUnderTest(ct);

        // Mock the primary tables/entities that is going to be used by the service
        var transactions = MockDataHelpers.GenerateTransactions<DepositEntryViewModel>(1, 20);
        await context.Transactions.AddRangeAsync(transactions);
        await context.SaveChangesAsync(ct);

        var expectedViewModels = transactions.ToViewModel<DepositEntryViewModel>().ToList();

        // Act
        var result = await sut.GetTransactionsAsync(ct: ct);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.WasError);

        Assert.NotNull(result.ReturnObject);
        Assert.NotEmpty(result.ReturnObject);

        var returnedViewModels = result.ReturnObject.ToList();
        Assert.Equal(expectedViewModels.Count, returnedViewModels.Count);
        Assert.All(expectedViewModels, evm =>
            Assert.NotNull(returnedViewModels.FirstOrDefault(rvm =>
                evm.TransactionDate == rvm.TransactionDate
                && evm.Memo == rvm.Memo
                && evm.DepositReasonId == rvm.DepositReasonId
                && evm.Amount == rvm.Amount))
        );
    }

    [Fact]
    public async Task GetDepositById_ReturnsCorrectDeposit()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var ct = cts.Token;

        var (dbContextFactory, context, sut) = PrepareServiceUnderTest(ct);

        // Mock the primary tables/entities that is going to be used by the service
        var transactions = MockDataHelpers.GenerateTransactions<DepositEntryViewModel>(1, 20);
        await context.Transactions.AddRangeAsync(transactions);
        await context.SaveChangesAsync(ct);

        var idCheck = transactions.First().Id;
        var expectedViewModel = (DepositEntryViewModel)transactions.First().ToViewModel();

        // Act
        var result = await sut.GetTransactionAsync(idCheck, ct: ct);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.WasError);

        Assert.NotNull(result.ReturnObject);

        var returnedViewModel = result.ReturnObject;
        Assert.Equal(expectedViewModel.AccountId, returnedViewModel.AccountId);
        Assert.Equal(expectedViewModel.TransactionDate, returnedViewModel.TransactionDate);
        Assert.Equal(expectedViewModel.Memo, returnedViewModel.Memo);
        Assert.Equal(expectedViewModel.DepositReasonId, returnedViewModel.DepositReasonId);
        Assert.Equal(expectedViewModel.Amount, returnedViewModel.Amount);
    }

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
        var result = await sut.AddTransactionAsync(toAdd, ct);

        // Assert
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
    public async Task UpdateDeposit_UpdatesExistingTransactionRecord()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var ct = cts.Token;

        var (dbContextFactory, context, sut) = PrepareServiceUnderTest(ct);

        // Mock the primary tables/entities that is going to be used by the service
        var transactions = MockDataHelpers.GenerateTransactions<DepositEntryViewModel>(1, 10);
        await context.Transactions.AddRangeAsync(transactions);
        await context.SaveChangesAsync(ct);

        var toUpdate = transactions.First();
        var updateValue = (DepositEntryViewModel)toUpdate.ToViewModel();
        var expectedPostedDate = DateTime.UtcNow;
        updateValue.PostedDate = expectedPostedDate;

        // Act
        var result = await sut.UpdateTransactionAsync(updateValue, ct);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.WasError);

        // Get the new record back from in-memory db
        var updatedDeposit = context.Transactions.Include(e => e.Deposit).FirstOrDefault(e => e.Id == result.RecordId);

        // The records (Transaction and Deposit) exists in the database
        Assert.NotNull(updatedDeposit);
        Assert.NotNull(updatedDeposit.Deposit);

        Assert.Equal(toUpdate.Id, updatedDeposit.Id);
        Assert.Equal(expectedPostedDate, updatedDeposit.PostingDate);
    }
    
    [Fact]
    public async Task DeleteDeposit_SetsDeletedFlagInTransactionRecord()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var ct = cts.Token;

        var (dbContextFactory, context, sut) = PrepareServiceUnderTest(ct);

        // Mock the primary tables/entities that is going to be used by the service
        var transactions = MockDataHelpers.GenerateTransactions<DepositEntryViewModel>(1, 10);
        await context.Transactions.AddRangeAsync(transactions);
        await context.SaveChangesAsync(ct);

        var idCheck = transactions.First().Id;
        var expectedViewModel = (DepositEntryViewModel)transactions.First().ToViewModel();

        // Act
        var result = await sut.DeleteTransactionAsync(idCheck, ct);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.WasError);

        // Get the new record back from in-memory db
        var deletedDeposit = context.Transactions.Include(e => e.Deposit).FirstOrDefault(e => e.Id == result.RecordId);

        // The records (Transaction and Deposit) exists in the database, but has a Purge Date
        Assert.NotNull(deletedDeposit);
        Assert.NotNull(deletedDeposit.PurgeDate);

        Assert.Equal(idCheck, deletedDeposit.Id);
    }

}
