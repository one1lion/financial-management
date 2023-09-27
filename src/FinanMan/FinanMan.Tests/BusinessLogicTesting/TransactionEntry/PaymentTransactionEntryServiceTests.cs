using FinanMan.Database;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.Extensions;
using FinanMan.SharedServer.Services;
using FinanMan.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FinanMan.Tests.BusinessLogicTesting.TransactionEntry;

public class PaymentTransactionEntryServiceTests
{
    private readonly PaymentEntryViewModelValidator _dataEntryValidator = new();

    #region Helpers
    private (Mock<IDbContextFactory<FinanManContext>> DbContextFactory, DbContextOptions<FinanManContext> Context, TransactionEntryService<PaymentEntryViewModel> Sut) PrepareServiceUnderTest(CancellationToken ct)
    {
        var (dbContextFactory, contextOptions) = MockDataHelpers.PrepareDbContext(ct);
        var loggerFactory = MockDataHelpers.GetLoggerFactory();

        // Prepare a new Payment Entry Service instance
        var sut = new TransactionEntryService<PaymentEntryViewModel>(dbContextFactory.Object, _dataEntryValidator, loggerFactory.Object);

        return (dbContextFactory, contextOptions, sut);
    }
    #endregion Helpers

    [Fact(Skip = "Not fully implemented")]
    public async Task GetAllPaymentEntries_ReturnsAllPaymentEntries()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var ct = cts.Token;

        var (dbContextFactory, contextOptions, sut) = PrepareServiceUnderTest(ct);
        var loggerFactory = MockDataHelpers.GetLoggerFactory();

        // Mock the primary tables/entities that is going to be used by the service
        var transactions = MockDataHelpers.GenerateTransactions<PaymentEntryViewModel>(1, 20);
        var context = new FinanManContext(contextOptions);
        await context.Transactions.AddRangeAsync(transactions);
        await context.SaveChangesAsync(ct);

        var expectedViewModels = transactions.ToViewModel<PaymentEntryViewModel>().ToList();

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
                && evm.PayeeId == rvm.PayeeId
                && evm.Total == rvm.Total))
        );
    }

    [Fact(Skip = "Not fully implemented")]
    public async Task GetPaymentById_ReturnsCorrectPayment()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var ct = cts.Token;

        var (dbContextFactory, contextOptions, sut) = PrepareServiceUnderTest(ct);

        // Mock the primary tables/entities that is going to be used by the service
        var transactions = MockDataHelpers.GenerateTransactions<PaymentEntryViewModel>(1, 20);
        var context = new FinanManContext(contextOptions);
        await context.Transactions.AddRangeAsync(transactions);
        await context.SaveChangesAsync(ct);

        var idCheck = transactions.First().Id;
        var expectedViewModel = (PaymentEntryViewModel)transactions.First().ToViewModel();

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
        Assert.Equal(expectedViewModel.PayeeId, returnedViewModel.PayeeId);
        Assert.Equal(expectedViewModel.Total, returnedViewModel.Total);
    }

    [Fact(Skip = "Not fully implemented")]
    public async Task AddPayment_AddsToTransactionCollection()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var ct = cts.Token;

        var (dbContextFactory, contextOptions, sut) = PrepareServiceUnderTest(ct);

        // Prepare the data to be added
        var toAdd = new PaymentEntryViewModel()
        {
            TransactionDate = new DateTime(2022, 1, 1),
            PayeeValueText = 1.ToString(),
            AccountValueText = 1.ToString(),
            LineItems = new List<PaymentDetailViewModel>()
            {
                new() { LineItemTypeValueText = 1.ToString(), Amount = 100 },
                new() { LineItemTypeValueText = 2.ToString(), Amount = 13 },

            }
        };

        var context = new FinanManContext(contextOptions);

        // Act
        var result = await sut.AddTransactionAsync(toAdd, ct);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.WasError);

        // Get the new record back from in-memory db
        context = new FinanManContext(contextOptions);
        var newPayment = context.Transactions
            .Include(e => e.Payment)
            .ThenInclude(e => e.PaymentDetails)
            .FirstOrDefault(e => e.Id == result.RecordId);

        // The records (Transaction and Payment) exists in the database
        Assert.NotNull(newPayment);
        Assert.NotNull(newPayment.Payment);

        Assert.Equal(toAdd.TransactionDate, newPayment.TransactionDate);
        Assert.Equal(toAdd.Memo, newPayment.Memo);
        Assert.Equal(toAdd.PayeeId, newPayment.Payment.PayeeId);
        Assert.Equal(toAdd.Total, newPayment.Payment.PaymentDetails.Sum(x => x.Amount));
    }

    [Fact(Skip = "Not fully implemented")]
    public async Task UpdatePayment_UpdatesExistingTransactionRecord()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var ct = cts.Token;

        var (dbContextFactory, contextOptions, sut) = PrepareServiceUnderTest(ct);

        // Mock the primary tables/entities that is going to be used by the service
        var transactions = MockDataHelpers.GenerateTransactions<PaymentEntryViewModel>(1, 10);
        var context = new FinanManContext(contextOptions);
        await context.Transactions.AddRangeAsync(transactions);
        await context.SaveChangesAsync(ct);

        var toUpdate = transactions.First();
        var updateValue = (PaymentEntryViewModel)toUpdate.ToViewModel();
        var expectedPostedDate = DateTime.UtcNow;
        updateValue.PostedDate = expectedPostedDate;

        // Act
        var result = await sut.UpdateTransactionAsync(updateValue, ct);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.WasError);

        // Get the new record back from in-memory db
        context = new FinanManContext(contextOptions);
        var updatedPayment = context.Transactions.Include(e => e.Payment).FirstOrDefault(e => e.Id == result.RecordId);

        // The records (Transaction and Payment) exists in the database
        Assert.NotNull(updatedPayment);
        Assert.NotNull(updatedPayment.Payment);

        Assert.Equal(toUpdate.Id, updatedPayment.Id);
        Assert.Equal(expectedPostedDate, updatedPayment.PostingDate);
    }

    [Fact(Skip = "Not fully implemented")]
    public async Task DeletePayment_SetsDeletedFlagInTransactionRecord()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var ct = cts.Token;

        var (dbContextFactory, contextOptions, sut) = PrepareServiceUnderTest(ct);

        // Mock the primary tables/entities that is going to be used by the service
        var transactions = MockDataHelpers.GenerateTransactions<PaymentEntryViewModel>(1, 10);
        var context = new FinanManContext(contextOptions);
        await context.Transactions.AddRangeAsync(transactions);
        await context.SaveChangesAsync(ct);

        var idCheck = transactions.First().Id;
        var expectedViewModel = (PaymentEntryViewModel)transactions.First().ToViewModel();

        // Act
        var result = await sut.DeleteTransactionAsync(idCheck, ct);

        // Assert
        Assert.NotNull(result);
        Assert.False(result.WasError);

        // Get the new record back from in-memory db
        context = new FinanManContext(contextOptions);
        var deletedPayment = context.Transactions.Include(e => e.Payment).FirstOrDefault(e => e.Id == result.RecordId);

        // The records (Transaction and Payment) exists in the database, but has a Purge Date
        Assert.NotNull(deletedPayment);
        Assert.NotNull(deletedPayment.PurgeDate);

        Assert.Equal(idCheck, deletedPayment.Id);
    }
}
