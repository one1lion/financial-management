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

namespace FinanMan.Tests.BusinessLogicTesting.TransactionEntry;

public class TransferTransactionEntryServiceTests
{
    private readonly TransferEntryViewModelValidator _dataEntryValidator = new();

    #region Helpers
    private (Mock<IDbContextFactory<FinanManContext>> DbContextFactory, DbContextOptions<FinanManContext> Context, TransactionEntryService<TransferEntryViewModel> Sut) PrepareServiceUnderTest(CancellationToken ct)
    {
        var (dbContextFactory, contextOptions) = MockDataHelpers.PrepareDbContext(ct);
        var loggerFactory = MockDataHelpers.GetLoggerFactory();

        // Prepare a new Transfer Entry Service instance
        var sut = new TransactionEntryService<TransferEntryViewModel>(dbContextFactory.Object, _dataEntryValidator, loggerFactory.Object);

        return (dbContextFactory, contextOptions, sut);
    }
    #endregion Helpers

    [Fact]
    public async Task GetAllTransferEntries_ReturnsAllTransferEntries()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var ct = cts.Token;

        var (dbContextFactory, contextOptions, sut) = PrepareServiceUnderTest(ct);
        var loggerFactory = MockDataHelpers.GetLoggerFactory();

        // Mock the primary tables/entities that is going to be used by the service
        var transactions = MockDataHelpers.GenerateTransactions<TransferEntryViewModel>(1, 20);
        var context = new FinanManContext(contextOptions);
        await context.Transactions.AddRangeAsync(transactions);
        await context.SaveChangesAsync(ct);

        var expectedViewModels = transactions.ToViewModel<TransferEntryViewModel>().ToList();

        // Act
        var result = await sut.GetTransactionsAsync(ct: ct);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.WasError);

        Assert.NotNull(result.Data);
        Assert.NotEmpty(result.Data);

        var returnedViewModels = result.Data.ToList();
        Assert.Equal(expectedViewModels.Count, returnedViewModels.Count);
        Assert.All(expectedViewModels, evm =>
            Assert.NotNull(returnedViewModels.FirstOrDefault(rvm =>
                evm.TransactionDate == rvm.TransactionDate
                && evm.Memo == rvm.Memo
                && evm.SourceAccountId == rvm.SourceAccountId
                && evm.TargetAccountId == rvm.TargetAccountId
                && evm.Amount == rvm.Amount))
        );
    }

    [Fact]
    public async Task GetTransferById_ReturnsCorrectTransfer()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var ct = cts.Token;

        var (dbContextFactory, contextOptions, sut) = PrepareServiceUnderTest(ct);

        // Mock the primary tables/entities that is going to be used by the service
        var transactions = MockDataHelpers.GenerateTransactions<TransferEntryViewModel>(1, 20);
        var context = new FinanManContext(contextOptions);
        await context.Transactions.AddRangeAsync(transactions);
        await context.SaveChangesAsync(ct);

        var idCheck = transactions.First().Id;
        var expectedViewModel = (TransferEntryViewModel)transactions.First().ToViewModel();

        // Act
        var result = await sut.GetTransactionAsync(idCheck, ct: ct);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.WasError);

        Assert.NotNull(result.Data);

        var returnedViewModel = result.Data;
        Assert.Equal(expectedViewModel.AccountId, returnedViewModel.AccountId);
        Assert.Equal(expectedViewModel.TransactionDate, returnedViewModel.TransactionDate);
        Assert.Equal(expectedViewModel.Memo, returnedViewModel.Memo);
        Assert.Equal(expectedViewModel.SourceAccountId, returnedViewModel.SourceAccountId);
        Assert.Equal(expectedViewModel.TargetAccountId, returnedViewModel.TargetAccountId);
        Assert.Equal(expectedViewModel.Amount, returnedViewModel.Amount);
    }

    [Fact]
    public async Task AddTransfer_AddsToTransactionCollection()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var ct = cts.Token;

        var (dbContextFactory, contextOptions, sut) = PrepareServiceUnderTest(ct);

        // Prepare the data to be added
        var toAdd = new TransferEntryViewModel()
        {
            TransactionDate = new DateTime(2022, 1, 1),
            SourceAccountValueText = 1.ToString(),
            TargetAccountValueText = 2.ToString(),
            Amount = 239184
        };

        var context = new FinanManContext(contextOptions);

        // Act
        var result = await sut.AddTransactionAsync(toAdd, ct);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.WasError);

        // Get the new record back from in-memory db
        context = new FinanManContext(contextOptions);
        var newTransfer = context.Transactions.Include(e => e.Transfer).FirstOrDefault(e => e.Id == result.RecordId);

        // The records (Transaction and Transfer) exists in the database
        Assert.NotNull(newTransfer);
        Assert.NotNull(newTransfer.Transfer);

        Assert.Equal(toAdd.TransactionDate, newTransfer.TransactionDate);
        Assert.Equal(toAdd.Memo, newTransfer.Memo);
        Assert.Equal(toAdd.SourceAccountId, newTransfer.AccountId);
        Assert.Equal(toAdd.TargetAccountId, newTransfer.Transfer.TargetAccountId);
        Assert.Equal(toAdd.Amount, newTransfer.Transfer.Amount);
    }

    [Fact]
    public async Task UpdateTransfer_UpdatesExistingTransactionRecord()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var ct = cts.Token;

        var (dbContextFactory, contextOptions, sut) = PrepareServiceUnderTest(ct);

        // Mock the primary tables/entities that is going to be used by the service
        var transactions = MockDataHelpers.GenerateTransactions<TransferEntryViewModel>(1, 10);
        var context = new FinanManContext(contextOptions);
        await context.Transactions.AddRangeAsync(transactions);
        await context.SaveChangesAsync(ct);

        var toUpdate = transactions.First();
        var updateValue = (TransferEntryViewModel)toUpdate.ToViewModel();
        var expectedPostedDate = DateTime.UtcNow;
        updateValue.PostedDate = expectedPostedDate;

        // Act
        var result = await sut.UpdateTransactionAsync(updateValue, ct);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.WasError);

        // Get the new record back from in-memory db
        context = new FinanManContext(contextOptions);
        var updatedTransfer = context.Transactions.Include(e => e.Transfer).FirstOrDefault(e => e.Id == result.RecordId);

        // The records (Transaction and Transfer) exists in the database
        Assert.NotNull(updatedTransfer);
        Assert.NotNull(updatedTransfer.Transfer);

        Assert.Equal(toUpdate.Id, updatedTransfer.Id);
        Assert.Equal(expectedPostedDate, updatedTransfer.PostingDate);
    }

    [Fact]
    public async Task DeleteTransfer_SetsDeletedFlagInTransactionRecord()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var ct = cts.Token;

        var (dbContextFactory, contextOptions, sut) = PrepareServiceUnderTest(ct);

        // Mock the primary tables/entities that is going to be used by the service
        var transactions = MockDataHelpers.GenerateTransactions<TransferEntryViewModel>(1, 10);
        var context = new FinanManContext(contextOptions);
        await context.Transactions.AddRangeAsync(transactions);
        await context.SaveChangesAsync(ct);

        var idCheck = transactions.First().Id;
        var expectedViewModel = (TransferEntryViewModel)transactions.First().ToViewModel();

        // Act
        var result = await sut.DeleteTransactionAsync(idCheck, ct);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.WasError);

        // Get the new record back from in-memory db
        context = new FinanManContext(contextOptions);
        var deletedTransfer = context.Transactions.Include(e => e.Transfer).FirstOrDefault(e => e.Id == result.RecordId);

        // The records (Transaction and Transfer) exists in the database, but has a Purge Date
        Assert.NotNull(deletedTransfer);
        Assert.NotNull(deletedTransfer.PurgeDate);

        Assert.Equal(idCheck, deletedTransfer.Id);
    }

    

}
