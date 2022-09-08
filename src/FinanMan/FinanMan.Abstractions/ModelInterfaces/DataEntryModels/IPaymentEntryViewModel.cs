namespace FinanMan.Abstractions.ModelInterfaces.DataEntryModels;

public interface IPaymentEntryViewModel
{
    string? AccountValueText { get; set; }
    ICollection<ILineItemViewModel> LineItems { get; init; }
    string? Memo { get; set; }
    string? PayeeValueText { get; set; }
    DateTime? PostedDate { get; set; }
    DateTime? TransactionDate { get; set; }

    double Total { get; }
}
