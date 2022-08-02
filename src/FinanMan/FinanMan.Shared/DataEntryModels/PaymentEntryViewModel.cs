namespace FinanMan.Shared.DataEntryModels;

public class PaymentEntryViewModel
{
    public int? AccountId { get;set; }
    public List<LineItemViewModel> LineItems { get; init; } = new();
    public string? Memo { get; set; }
    public int? PayeeId { get; set; }
    public DateTime? PostedDate { get; set; }
    public DateTime? TransactionDate { get; set; }

    public double Total => Math.Round(LineItems.Where(x => x.Amount.HasValue).Sum(x => x.Amount!.Value), 2);
}
