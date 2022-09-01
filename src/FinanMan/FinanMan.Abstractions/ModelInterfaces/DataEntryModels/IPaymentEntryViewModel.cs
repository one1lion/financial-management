namespace FinanMan.Abstractions.ModelInterfaces.DataEntryModels;

public interface IPaymentEntryViewModel
{
    int? AccountId { get; set; }
    ICollection<ILineItemViewModel> LineItems { get; init; }
    string? Memo { get; set; }
    int? PayeeId { get; set; }
    DateTime? PostedDate { get; set; }
    DateTime? TransactionDate { get; set; }

    double Total { get; }
}
