using AngleSharp.Dom;
using FinanMan.Database;
using FinanMan.Database.Data;
using FinanMan.Database.Models.Tables;
using FinanMan.Shared.DataEntryModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.Logging;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace FinanMan.Tests.Helpers;

public static class MockDataHelpers
{
    public static DbContextOptions<FinanManContext> FinanManContextOptions { get; } = new DbContextOptionsBuilder<FinanManContext>()
        .UseInMemoryDatabase(Guid.NewGuid().ToString())
        .ConfigureWarnings(x => x.Ignore(InMemoryEventId.TransactionIgnoredWarning))
        .Options;

    public static FinanManContext FinanManContext => new(FinanManContextOptions);

    private static readonly DateTime _earliestDate = new(2019, 1, 1);
    private static readonly DateTime _latestDate = DateTime.UtcNow.AddDays(10);

    public static Mock<FinanManContext> SetupLookupTables(this Mock<FinanManContext> dbContextMock)
    {
        dbContextMock.Setup(e => e.AccountTypes)
            .Returns(DataHelper.GetSeedData<LuAccountType>().AsQueryable().BuildMockDbSet().Object);
        dbContextMock.Setup(e => e.Categories)
            .Returns(DataHelper.GetSeedData<LuCategory>().AsQueryable().BuildMockDbSet().Object);
        dbContextMock.Setup(e => e.DepositReasons)
            .Returns(DataHelper.GetSeedData<LuDepositReason>().AsQueryable().BuildMockDbSet().Object);
        dbContextMock.Setup(e => e.LineItemTypes)
            .Returns(DataHelper.GetSeedData<LuLineItemType>().AsQueryable().BuildMockDbSet().Object);
        dbContextMock.Setup(e => e.RecurrenceTypes)
            .Returns(DataHelper.GetSeedData<LuRecurrenceType>().AsQueryable().BuildMockDbSet().Object);

        return dbContextMock;
    }

    public static List<Transaction> GenerateDepositTransactions(int startId, int count = 1) =>
        Enumerable.Range(0, count - 1)
            .Select(e => new Transaction()
            {
                Id = startId + e,
                TransactionDate = GenerateRandomDate(),
                DateEntered = DateTime.UtcNow,
                PostingDate = Random.Shared.NextDouble() < .5 ? DateTime.UtcNow : null,
                Memo = Random.Shared.NextDouble() < .2 ? Guid.NewGuid().ToString() : null,
                Deposit = new Deposit()
                {
                    Id = startId + e,
                    DepositReasonId = 1,
                    TransactionId = startId + e,
                    Amount = Random.Shared.NextDouble() * 23415.23 + 1,
                }
            })
            .ToList();

    public static List<Transaction> GenerateTransactions<TViewModel>(int startId, int count = 1)
        where TViewModel : ITransactionDataEntryViewModel =>
        Enumerable.Range(0, count - 1)
            .Select(e => new Transaction()
            {
                Id = startId + e,
                TransactionDate = GenerateRandomDate(),
                DateEntered = DateTime.UtcNow,
                PostingDate = Random.Shared.NextDouble() < .5 ? DateTime.UtcNow : null,
                Memo = Random.Shared.NextDouble() < .2 ? Guid.NewGuid().ToString() : null,
                AccountId = 1,
                Deposit = typeof(TViewModel) == typeof(DepositEntryViewModel) ? new Deposit()
                {
                    Id = startId + e,
                    TransactionId = startId + e,
                    DepositReasonId = 1,
                    Amount = Random.Shared.NextDouble() * 23415.23 + 1,
                } : null,
                Payment = typeof(TViewModel) == typeof(PaymentEntryViewModel) ? new Payment()
                {
                    Id = startId + e,
                    TransactionId = startId + e,
                    PaymentDetails = new List<PaymentDetail>() {
                        new() {
                            Id = (startId + e) * 100 + 1,
                            Amount = Random.Shared.NextDouble() * 23415.23 + 1 ,
                            LineItemTypeId = 1
                        },
                        new() {
                            Id = (startId + e) * 100 + 2,
                            Amount = Random.Shared.NextDouble() * 23415.23 + 1 ,
                            LineItemTypeId = 2
                        }
                    }
                } : null,
                Transfer = typeof(TViewModel) == typeof(TransferEntryViewModel) ? new Transfer()
                {
                    Id = startId + e,
                    TransactionId = startId + e,
                    TargetAccountId = 2,
                    Amount = Random.Shared.NextDouble() * 23415.23 + 1,
                } : null
            })
            .ToList();

    public static DateTime GenerateRandomDate(DateTime? earliestDate = null, DateTime? latestDate = null)
    {
        if (!earliestDate.HasValue) { earliestDate = _earliestDate; }
        if (!latestDate.HasValue) { latestDate = _latestDate; }
        var randomTest = new Random();
        var timeSpan = latestDate - earliestDate;
        var newSpan = new TimeSpan(0, randomTest.Next(0, (int)timeSpan.Value.TotalMinutes), 0);
        return earliestDate.Value + newSpan;
    }


    public static (Mock<IDbContextFactory<FinanManContext>> dbContextFactory, FinanManContext context) PrepareDbContext(CancellationToken ct)
    {
        // Mock the DB Context
        var dbContextFactory = new Mock<IDbContextFactory<FinanManContext>>();

        // Mock the FluentValidation validator for the Deposit Entry View Model
        var context = FinanManContext;
        dbContextFactory.Setup(e => e.CreateDbContextAsync(ct))
            .ReturnsAsync(context);

        return (dbContextFactory, context);
    }

    public static Mock<ILoggerFactory> GetLoggerFactory()
    {
        var loggerFactory = new Mock<ILoggerFactory>();
        var logger = new Mock<ILogger>();
        loggerFactory.Setup(e => e.CreateLogger(It.IsAny<string>()))
            .Returns(logger.Object);
        //logger.Setup(e => e.LogError(It.IsAny<Exception>(), It.IsAny<string>(), new[] { It.IsAny<object>() }));
        return loggerFactory;
    }
}
