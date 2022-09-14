using System.Collections.Generic;
using System.Linq;
using System.Threading;
using FinanMan.Database;
using FinanMan.Shared.DataEntryModels;
using FinanMan.SharedServer.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using FinanMan.Database.Models.Tables;
using MockQueryable.Moq;
using Moq;

namespace FinanMan.Tests.BusinessLogicTesting;

public class TransactionEntryServiceDepositEntry : ClassContext<TransactionEntryService<DepositEntryViewModel>>
{
    [Fact]
    public async Task Example1_Ensure_with_verify_great_if_you_only_need_test_a_few_things()
    {
        CancellationToken ct = new CancellationToken();
        var dbConext = MockOf<IDbContextFactory<FinanManContext>>();
        var mockDb =
            new Mock<FinanManContext>(); // I tend to pass context in here so wouldn't usally have to do a seperate mock
        dbConext.Setup(e => e.CreateDbContextAsync(ct))
            .ReturnsAsync(mockDb
                .Object); // would normall pass the dbconext in not IDbContextFactory so this wouldn't be required would move to static func

        var mockDbSet = new List<Transaction>().AsQueryable().BuildMockDbSet(); // will need this to mock ef
        mockDb.Setup(e => e.Transactions)
            .Returns(mockDbSet.Object);

        var result = await Sut.AddTransactionData(new DepositEntryViewModel(), ct);

        // you can test mappings etc as don't know what your going to use and from sound neither do you hehehehe

        // this is if there only a few
        mockDbSet.Verify(e =>
            e.Add(It.Is<Transaction>(e => e.Id == 532 && e.Account.Name == "WhatEver")), Times.Once());

        mockDb.Verify(e => e.SaveChangesAsync(ct), Times.Once);
    }

    [Fact]
    public async Task Example2_Ensure_with_call_back_great_if_you_need_what_was_added_back()
    {
        // with this we get a call back to get the entire model. . verify above great but can get messy if checking 100's of things
        CancellationToken ct = new CancellationToken();
        var dbConext = MockOf<IDbContextFactory<FinanManContext>>();
        var mockDb =
            new Mock<FinanManContext>(); // I tend to pass context in here so wouldn't usally have to do a seperate mock
        dbConext.Setup(e => e.CreateDbContextAsync(ct))
            .ReturnsAsync(mockDb
                .Object); // would normall pass the dbconext in not IDbContextFactory so this wouldn't be required would move to static func

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
}