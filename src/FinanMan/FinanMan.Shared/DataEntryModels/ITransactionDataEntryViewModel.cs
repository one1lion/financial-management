using FinanMan.Database.Models.Shared;
using FinanMan.Shared.LookupModels;

namespace FinanMan.Shared.DataEntryModels;

public interface ITransactionDataEntryViewModel : ICloneable
{
    int TransactionId { get; set; }
    TransactionType TransactionType { get; }
    public DateTime DateEntered { get; set; }
    public DateTime? TransactionDate { get; set; }
    public DateTime? PostedDate { get; set; }
    public string? AccountName { get; set; }
    public int? AccountId { get; set; }
    public string? Memo { get; set; }
    public decimal Total { get; }

    void Patch(ITransactionDataEntryViewModel source);
    void UpdateAccountName(IEnumerable<ILookupItemViewModel> accounts);
}
