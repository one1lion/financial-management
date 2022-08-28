namespace FinanMan.Abstractions.ModelInterfaces.DataEntryModels;

public interface IPaymentEntryViewModel<TLineItemViewModel> where TLineItemViewModel : class, ILineItemViewModel
{
    int? AccountId { get; set; }
    ICollection<TLineItemViewModel> LineItems { get; init; }
    string? Memo { get; set; }
    int? PayeeId { get; set; }
    DateTime? PostedDate { get; set; }
    DateTime? TransactionDate { get; set; }

    double Total { get; }
}
