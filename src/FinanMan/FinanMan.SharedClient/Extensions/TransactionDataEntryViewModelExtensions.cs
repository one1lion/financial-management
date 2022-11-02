using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.ServiceInterfaces;
using FinanMan.SharedClient.Services;

namespace FinanMan.SharedClient.Extensions;

public static class TransactionDataEntryViewModelExtensions
{
    public static string GetApiEndpoint<TDataEntryViewModel>(this ITransactionEntryService<TDataEntryViewModel> transService)
        where TDataEntryViewModel : class, ITransactionDataEntryViewModel =>
        transService switch
        {
            TransactionEntryService<DepositEntryViewModel> _ => "api/Deposits",
            TransactionEntryService<PaymentEntryViewModel> _ => "api/Payments",
            TransactionEntryService<TransferEntryViewModel> _ => "api/Transfers",
            _ => throw new NotImplementedException()
        };
}
