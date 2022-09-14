using FinanMan.Database;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.ServiceInterfaces;
using FinanMan.SharedServer.Extensions;
using FinanMan.SharedServer.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;
using System.Threading.Tasks;

namespace FinanMan.Tests.BusinessLogicTesting;

public class TransactionEntryServiceTests
{
    private readonly ITransactionEntryService<DepositEntryViewModel> _depositsService;
    private readonly ITransactionEntryService<PaymentEntryViewModel> _paymentsService;
    private readonly ITransactionEntryService<TransferEntryViewModel> _transfersService;
    private readonly IDbContextFactory<FinanManContext> _dbContextFactory;

    public TransactionEntryServiceTests()
    {
        var services = new ServiceCollection();
        services.AddScoped<ITransactionEntryService<DepositEntryViewModel>, TransactionEntryService<DepositEntryViewModel>>();
        //services.AddDbContextFactory<FinanManContext>(options =>
            
        //);
        services.AddServerServices();

        var serviceProvider = services.BuildServiceProvider();
        
        _depositsService = serviceProvider!.GetRequiredService<ITransactionEntryService<DepositEntryViewModel>>()!;
        _paymentsService = serviceProvider!.GetRequiredService<ITransactionEntryService<PaymentEntryViewModel>>()!;
        _transfersService = serviceProvider!.GetRequiredService<ITransactionEntryService<TransferEntryViewModel>>()!;
        _dbContextFactory = serviceProvider!.GetRequiredService<IDbContextFactory<FinanManContext>>()!;
    }

    [Fact]
    public async Task GetAllDepositEntries_ReturnsAllDepositEntries()
    {
        // Arrange
        var depositsService = new TransactionEntryService<DepositEntryViewModel>();

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
}
