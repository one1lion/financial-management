using FinanMan.Database;
using FinanMan.Database.Data;
using FinanMan.Database.Models.Tables;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.ServiceInterfaces;
using FinanMan.SharedServer.Services;
using FinanMan.Tests.Helpers;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace FinanMan.Tests.BusinessLogicTesting;

public class DepositTransactionEntryServiceTests : ClassContext<TransactionEntryService<DepositEntryViewModel>>
{
    private readonly ITransactionEntryService<DepositEntryViewModel> _depositsService;
    private readonly IDbContextFactory<FinanManContext> _dbContextFactory;

    [Fact]
    public async Task AddDeposit_AddsToTransactionCollection()
    {
        // Arrange
        using var cts = new CancellationTokenSource();
        var ct = cts.Token;

        // Mock the DB Context
        var dbConextFactory = MockOf<IDbContextFactory<FinanManContext>>();

        // Mock the FluentValidation validator for the Deposit Entry View Model
        var options = new DbContextOptionsBuilder<FinanManContext>()
                  .UseInMemoryDatabase(Guid.NewGuid().ToString())
                  .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
                  .Options;
        var context = new FinanManContext(options);
        dbConextFactory.Setup(e => e.CreateDbContextAsync(ct))
            .ReturnsAsync(context);

        // Mock the DbSets used by the tests using Seed Data for lookup tables
        var mockDepositReasonDbSet = DataHelper.GetSeedData<LuDepositReason>().AsQueryable().BuildMockDbSet();
        //dbContextMock.SetupLookupTables();

        // Prepare the data to be added
        var toAdd = new DepositEntryViewModel()
        {
            TransactionDate = new DateTime(2022, 1, 1),
            TargetAccountValueText = 1.ToString(),
            DepositReasonValueText = 1.ToString(),
            Amount = 239184
        };

        // Prepare a new Deposit Entry Service instance
        var sut = new TransactionEntryService<DepositEntryViewModel>(dbConextFactory.Object, new DepositEntryViewModelValidator());

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
        Assert.NotNull(newDeposit.Deposit)                         ;

        Assert.Equal(toAdd.TransactionDate, newDeposit.TransactionDate);
        Assert.Equal(toAdd.Memo, newDeposit.Memo);
        Assert.Equal(toAdd.DepositReasonId, newDeposit.Deposit.DepositReasonId);
        Assert.Equal(toAdd.Amount, newDeposit.Deposit.Amount);
    }

    [Fact]
    public async Task GetAllDepositEntries_ReturnsAllDepositEntries()
    {
        // Arrange


        // Mock the primary tables/entities that is going to be used by the service
        var transactions = MockDataHelpers.GenerateDepositTransactions(1, 20);
        var deposits = transactions.Select(t => t.Deposit).ToList();
        transactions.ForEach(t => t.Deposit = null);
        var transDetails = transactions.SelectMany(t => t.TransactionDetails).ToList();

        // Call the Adds


        // Act
        var result = await _depositsService.GetTransactionData();


        // Assert

    }

    [Fact]
    public void GetAllPaymentEntries_ReturnsAllPaymentEntries()
    {

    }

    [Fact]
    public void GetAllTransferEntries_ReturnsAllTransferEntries()
    {

    }


    [Fact]
    public async Task AddDeposit_Works()
    {
        // Arrange
        var newTrans = new DepositEntryViewModel() { };

        // Act
        var result = await _depositsService.AddTransactionData(newTrans);


        // Assert

    }

}
