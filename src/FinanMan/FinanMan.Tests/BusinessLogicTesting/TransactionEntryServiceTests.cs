using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
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
    public async Task Ensure()
    {
        CancellationToken ct = new CancellationToken();
        var dbConext = MockOf<IDbContextFactory<FinanManContext>>();
        var mockDb = new Mock<FinanManContext>();           // I tend to pass context in here so wouldn't usally have to do a seperate mock
        dbConext.Setup(e => e.CreateDbContextAsync(ct))
            .ReturnsAsync(mockDb.Object);

        var mockDbSet = new List<Transaction>().AsQueryable().BuildMockDbSet();
        mockDb.Setup(e => e.Transactions)
            .Returns(mockDbSet.Object);

        var result = await Sut.AddTransactionData(new DepositEntryViewModel(),ct);

        // you can test mappings etc
        mockDbSet.Verify(e =>
            e.Add(It.Is<Transaction>(e => e.Id == 532 && e.Account.Name == "WhatEver")), Times.Once());
        
        mockDb.Verify(e=>e.SaveChangesAsync(ct),Times.Once);
    }
}
public abstract class ClassContext<T> where T : class
{
    private readonly Dictionary<Type, BuiltMocks> _mocks = new Dictionary<Type, BuiltMocks>();

    private T _sut = null;
    protected T Sut => _sut ??= Resolve(_mocks);

    private class BuiltMocks
    {
        public object Mock { get; set; }
        public object MockValue { get; set; }
    }

    protected Mock<TE> MockOf<TE>() where TE : class
    {
        if (_mocks.ContainsKey(typeof(TE)))
        {
            return (Mock<TE>) _mocks[typeof(TE)].Mock;
        }

        Mock<TE> mock = null;
        if (typeof(TE).IsClass)
        {
            var parameters = GetParamInfoForConstructorOfType<TE>(false);

            if (parameters.Length > 0)
            {
                mock = new Mock<TE>(parameters.Select(t => GetDefault(t.ParameterType)).ToArray());
            }
        }

        mock ??= new Mock<TE>();

        _mocks.Add(typeof(TE), new BuiltMocks
        {
            Mock = mock,
            MockValue = mock.Object
        });
        return MockOf<TE>();
    }

    private static T Resolve(IReadOnlyDictionary<Type, BuiltMocks> dictionary)
    {
        var allTypes = GetParamInfoForConstructorOfType<T>()
            .Select(t => t.ParameterType)
            .Select(t => dictionary.ContainsKey(t) ? dictionary[t].MockValue : null)
            .ToArray();

        return allTypes.Any()
            ? (T) Activator.CreateInstance(typeof(T), allTypes)
            : (T) Activator.CreateInstance(typeof(T));
    }


    private static ParameterInfo[] GetParamInfoForConstructorOfType(Type type, bool largest = true)
    {
        var entryPoints = type.GetConstructors().Where(t => t.IsPublic);
        var part = largest
            ? entryPoints.OrderByDescending(t => t.GetParameters().Length)
            : entryPoints.OrderBy(t => t.GetParameters().Length);

        return part.FirstOrDefault()?.GetParameters() ?? Array.Empty<ParameterInfo>();
    }

    private static object GetDefault(Type type) => type.IsValueType ? Activator.CreateInstance(type) : null;

    private static ParameterInfo[] GetParamInfoForConstructorOfType<TE>(bool largest = true)
        => GetParamInfoForConstructorOfType(typeof(TE), largest);
}


// public class TransactionEntryServiceTests
// {
//     private readonly ITransactionEntryService<DepositEntryViewModel> _depositsService;
//     private readonly ITransactionEntryService<PaymentEntryViewModel> _paymentsService;
//     private readonly ITransactionEntryService<TransferEntryViewModel> _transfersService;
//     private readonly IDbContextFactory<FinanManContext> _dbContextFactory;
//
//     public TransactionEntryServiceTests()
//     {
//         var services = new ServiceCollection();
//         services.AddScoped<ITransactionEntryService<DepositEntryViewModel>, TransactionEntryService<DepositEntryViewModel>>();
//         //services.AddDbContextFactory<FinanManContext>(options =>
//             
//         //);
//         services.AddServerServices();
//
//         var serviceProvider = services.BuildServiceProvider();
//         
//         _depositsService = serviceProvider!.GetRequiredService<ITransactionEntryService<DepositEntryViewModel>>()!;
//         _paymentsService = serviceProvider!.GetRequiredService<ITransactionEntryService<PaymentEntryViewModel>>()!;
//         _transfersService = serviceProvider!.GetRequiredService<ITransactionEntryService<TransferEntryViewModel>>()!;
//         _dbContextFactory = serviceProvider!.GetRequiredService<IDbContextFactory<FinanManContext>>()!;
//     }
//
//     [Fact]
//     public async Task GetAllDepositEntries_ReturnsAllDepositEntries()
//     {
//         // Arrange
//         var depositsService = new TransactionEntryService<DepositEntryViewModel>();
//
//         // Act
//         var result = await _depositsService.GetTransactionData();
//
//         // Assert
//
//     }
//
//     [Fact]
//     public void GetAllPaymentEntries_ReturnsAllPaymentEntries()
//     {
//
//     }
//
//     [Fact]
//     public void GetAllTransferEntries_ReturnsAllTransferEntries()
//     {
//
//     }
// }