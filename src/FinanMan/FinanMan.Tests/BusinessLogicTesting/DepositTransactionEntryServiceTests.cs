using FinanMan.Database;
using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.ServiceInterfaces;
using FinanMan.SharedServer.Extensions;
using FinanMan.SharedServer.Services;
using FinanMan.Tests.Helpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.InMemory.Storage.Internal;
using System.Threading.Tasks;

namespace FinanMan.Tests.BusinessLogicTesting;

public class DepositTransactionEntryServiceTests : ClassContext<TransactionEntryService<DepositEntryViewModel>>
{
    private readonly ITransactionEntryService<DepositEntryViewModel> _depositsService;
    private readonly IDbContextFactory<FinanManContext> _dbContextFactory;

    public DepositTransactionEntryServiceTests()
    {
        var services = new ServiceCollection();
        services.AddScoped<ITransactionEntryService<DepositEntryViewModel>, TransactionEntryService<DepositEntryViewModel>>();
        //services.AddDbContextFactory<FinanManContext>(options =>
            
        //);
        services.AddServerServices();

        var serviceProvider = services.BuildServiceProvider();
        
        _depositsService = serviceProvider!.GetRequiredService<ITransactionEntryService<DepositEntryViewModel>>()!;
        _dbContextFactory = serviceProvider!.GetRequiredService<IDbContextFactory<FinanManContext>>()!;
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
