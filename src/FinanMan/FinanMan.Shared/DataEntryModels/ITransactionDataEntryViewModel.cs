using FinanMan.Database.Models.Shared;

namespace FinanMan.Shared.DataEntryModels;

public interface ITransactionDataEntryViewModel : ICloneable
{
    int TransactionId { get; set; }
    TransactionType TransactionType { get; }
    public DateTime? TransactionDate { get; set; }
    public DateTime? PostedDate { get; set; }
    public string? AccountName { get; set; }
    public int? AccountId { get; set; }
    public string? Memo { get; set; }
    public decimal Total { get; }

    void Patch(ITransactionDataEntryViewModel source);
}
