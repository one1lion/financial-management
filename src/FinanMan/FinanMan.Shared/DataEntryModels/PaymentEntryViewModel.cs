using FinanMan.Abstractions.ModelInterfaces.DataEntryModels;

namespace FinanMan.Shared.DataEntryModels;

public class PaymentEntryViewModel : IPaymentEntryViewModel<LineItemViewModel>
{
    public int? AccountId { get;set; }
    public ICollection<LineItemViewModel> LineItems { get; init; } = new List<LineItemViewModel>();
    public string? Memo { get; set; }
    public int? PayeeId { get; set; }
    public DateTime? PostedDate { get; set; }
    public DateTime? TransactionDate { get; set; }

    public double Total => Math.Round(LineItems.Where(x => x.Amount.HasValue).Sum(x => x.Amount!.Value), 2);
}
