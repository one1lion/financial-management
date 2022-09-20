using FinanMan.Shared.Enums;

namespace FinanMan.Shared.DataEntryModels;

public interface ITransactionDataEntryViewModel
{
    TransactionType TransactionType { get; }
    public DateTime? TransactionDate { get; set; }
    public DateTime? PostedDate { get; set; }
    public int? AccountId { get; set; }
    public string? Memo { get; set; }
}