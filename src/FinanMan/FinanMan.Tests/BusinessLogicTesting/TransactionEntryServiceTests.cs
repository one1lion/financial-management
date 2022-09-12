using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.ServiceInterfaces;
using FinanMan.SharedServer.Services;

namespace FinanMan.Tests.BusinessLogicTesting;

public class TransactionEntryServiceTests
{
    private readonly ITransactionEntryService<DepositEntryViewModel> _depositsService;
    private readonly ITransactionEntryService<PaymentEntryViewModel> _paymentsService;
    private readonly ITransactionEntryService<TransferEntryViewModel> _transfersService;

    public TransactionEntryServiceTests(
        ITransactionEntryService<DepositEntryViewModel> depositsService,
        ITransactionEntryService<PaymentEntryViewModel> paymentsService,
        ITransactionEntryService<TransferEntryViewModel> transfersService)
    {
        _depositsService = depositsService;
        _paymentsService = paymentsService;
        _transfersService = transfersService;
    }

    [Fact]
    public void GetAllDepositEntries_ReturnsAllDepositEntries()
    {
        // Arrange

        // Act
        _depositsService.GetTransactionData();

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
