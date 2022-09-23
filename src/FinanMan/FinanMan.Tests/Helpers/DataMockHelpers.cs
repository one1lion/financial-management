using FinanMan.Database;
using FinanMan.Database.Data;
using FinanMan.Database.Models.Tables;
using MockQueryable.Moq;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FinanMan.Tests.Helpers;

public static class MockDataHelpers
{
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
    
    //public static Mock<FinanManContext> SetupLookupTables(this Mock<FinanManContext> dbContextMock)
    //{
    //    dbContextMock.Setup(e => e.AccountTypes)
    //        .Returns(DataHelper.GetSeedData<LuAccountType>().AsQueryable().BuildMockDbSet().Object);
    //    dbContextMock.Setup(e => e.Categories)
    //        .Returns(DataHelper.GetSeedData<LuCategory>().AsQueryable().BuildMockDbSet().Object);
    //    dbContextMock.Setup(e => e.DepositReasons)
    //        .Returns(DataHelper.GetSeedData<LuDepositReason>().AsQueryable().BuildMockDbSet().Object);
    //    dbContextMock.Setup(e => e.LineItemTypes)
    //        .Returns(DataHelper.GetSeedData<LuLineItemType>().AsQueryable().BuildMockDbSet().Object);
    //    dbContextMock.Setup(e => e.RecurrenceTypes)
    //        .Returns(DataHelper.GetSeedData<LuRecurrenceType>().AsQueryable().BuildMockDbSet().Object);

    //    return dbContextMock;
    //}

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
                    TransactionId = startId + e
                },
                TransactionDetails = new List<TransactionDetail>() 
                { 
                    new ()
                    {
                        Id = (startId + e) * 100 + 1,
                        Amount = Random.Shared.NextDouble() * 23415.23 + 1,
                        Description = Random.Shared.NextDouble() < .2 ? Guid.NewGuid().ToString() : null,
                        LineItemTypeId = 1,
                        TransactionId = startId + e
                    }
                }
            })
            .ToList();

    public static DateTime GenerateRandomDate(DateTime? earliestDate = null, DateTime? latestDate = null)
    {
        if(!earliestDate.HasValue) { earliestDate = _earliestDate; }
        if(!latestDate.HasValue) { latestDate = _latestDate; }
        var randomTest = new Random();
        var timeSpan = latestDate - earliestDate;
        var newSpan = new TimeSpan(0, randomTest.Next(0, (int)timeSpan.Value.TotalMinutes), 0);
        return earliestDate.Value + newSpan;
    }
}