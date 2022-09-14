using FinanMan.Shared.Enums;

namespace FinanMan.Shared.DataEntryModels;

public interface ITransactionDataEntryViewModel
{
    TransactionType TransactionType { get; }
}