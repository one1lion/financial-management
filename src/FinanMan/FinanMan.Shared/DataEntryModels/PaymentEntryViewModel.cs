using FinanMan.Abstractions.ModelInterfaces.DataEntryModels;

namespace FinanMan.Shared.DataEntryModels;

public class PaymentEntryViewModel : IPaymentEntryViewModel
{
    public int? AccountId { get;set; }
    public ICollection<ILineItemViewModel> LineItems { get; init; } = new List<ILineItemViewModel>();
    public string? Memo { get; set; }
    public int? PayeeId { get; set; }
    public DateTime? PostedDate { get; set; }
    public DateTime? TransactionDate { get; set; }

    public double Total => Math.Round(LineItems.Where(x => x.Amount.HasValue).Sum(x => x.Amount!.Value), 2);
}
