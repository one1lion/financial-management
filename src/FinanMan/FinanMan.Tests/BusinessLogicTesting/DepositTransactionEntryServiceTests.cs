using FinanMan.Database;
using FinanMan.Database.Data;
using FinanMan.Database.Models.Tables;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.ServiceInterfaces;
using FinanMan.SharedServer.Services;
using FinanMan.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking.Internal;
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
        
        var dbConextFactory = MockOf<IDbContextFactory<FinanManContext>>();

        var dbContextMock = new Mock<FinanManContext>();
        // Mock the primary tables/entities that is going to be used by the service
        var transactions = MockDataHelpers.GenerateDepositTransactions(1, 20);
        var deposits = transactions.Select(t => t.Deposit).ToList();
        transactions.ForEach(t => t.Deposit = null);
        var transDetails = transactions.SelectMany(t => t.TransactionDetails).ToList();
        
        var mockDepositDbSet = deposits.AsQueryable().BuildMockDbSet();
        dbContextMock.Setup(e => e.Deposits).Returns(mockDepositDbSet.Object);

        var mockTransactionDbSet = new List<Transaction>().AsQueryable().BuildMockDbSet();
        dbContextMock.Setup(e => e.Transactions).Returns(mockTransactionDbSet.Object);

        // Mock the DbSets used by the tests using Seed Data for lookup tables
        var mockDepositReasonDbSet = DataHelper.GetSeedData<LuDepositReason>().AsQueryable().BuildMockDbSet();
        dbContextMock.SetupLookupTables();

        // Use Mock Data for Accounts
        var mockAccountDbSet = new List<Account>().AsQueryable().BuildMockDbSet();
        dbContextMock.Setup(e => e.Accounts).Returns(mockAccountDbSet.Object);

        // I would normally pass the DBContext in instead of IDbContextFactory
        // so this wouldn't be required. I would move to static func
        cts.CancelAfter(5000);
        var ct = cts.Token;

        dbConextFactory.Setup(e => e.CreateDbContextAsync(ct))
            .ReturnsAsync(dbContextMock.Object);

        var toAdd = new DepositEntryViewModel()
        {
            TransactionDate = new DateTime(2022, 1, 1),
            TargetAccountValueText = 1.ToString(),
            DepositReasonValueText = 1.ToString(),
            Amount = 239184
        };

        // Act
        var result = await Sut.AddTransactionData(toAdd, ct);

        // you can test mappings etc as don't know what your going to use and from sound neither do you hehehehe

        // Assert
        // this is if there only a few
        Assert.NotNull(result);
        mockTransactionDbSet.Verify(e =>
            e.AddAsync(
                It.Is<Transaction>(e => 
                    e.AccountId == 1 
                    && e.TransactionDate == new DateTime(2022, 1, 1) 
                    && e.Deposit.DepositReasonId == 1
                    && e.TransactionDetails.First().Amount == 239184), ct), Times.Once());

        Assert.False(result.WasError);
        dbContextMock.Verify(e => e.SaveChangesAsync(ct), Times.Once);
    }

    [Fact]
    public async Task Example2_Ensure_with_call_back_great_if_you_need_what_was_added_back()
    {
        // with this we get a call back to get the entire model. . verify above great but can get messy if checking 100's of things
        var ct = new CancellationToken();
        var dbConext = MockOf<IDbContextFactory<FinanManContext>>();
        var mockDb = new Mock<FinanManContext>(); // I tend to pass context in here so wouldn't usally have to do a seperate mock
        // would normall pass the dbconext in not IDbContextFactory so this wouldn't be required would move to static func
        dbConext
            .Setup(e => e.CreateDbContextAsync(ct))
            .ReturnsAsync(mockDb.Object); 

        var mockDbSet = new List<Transaction>().AsQueryable().BuildMockDbSet(); // will need this to mock ef
        mockDb.Setup(e => e.Transactions)
            .Returns(mockDbSet.Object);

        Transaction? transaction = null;
        mockDbSet.Setup(e => e.Add(It.IsAny<Transaction>()))
            .Callback((Transaction e) => { transaction = e; });

        var result = await Sut.AddTransactionData(new DepositEntryViewModel(), ct);

        // you can test mappings etc as don't know what your going to use and from sound neither do you hehehehe

        // this is if there only a few
        Assert.Equal(532, transaction.Id);
        Assert.Equal("WhatEver", transaction.Account.Name);
        mockDbSet.Verify(e => e.Add(It.IsAny<Transaction>()), Times.Once());

        mockDb.Verify(e => e.SaveChangesAsync(ct), Times.Once);
    }

    [Fact]
    public async Task GetAllDepositEntries_ReturnsAllDepositEntries()
    {
        // Arrange
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
