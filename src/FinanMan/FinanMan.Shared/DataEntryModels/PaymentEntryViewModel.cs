using FinanMan.Shared.Enums;

namespace FinanMan.Shared.DataEntryModels;

public class PaymentEntryViewModel : ITransactionDataEntryViewModel
{
    public TransactionType TransactionType => TransactionType.Payment;
    
    public string? AccountValueText { get; set; }
    public ICollection<LineItemViewModel> LineItems { get; init; } = new List<LineItemViewModel>();
    public string? Memo { get; set; }
    public string? PayeeValueText { get; set; }
    public DateTime? PostedDate { get; set; }
    public DateTime? TransactionDate { get; set; }

    public double Total => Math.Round(LineItems.Where(x => x.Amount.HasValue).Sum(x => x.Amount!.Value), 2);
}
