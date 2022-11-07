using FinanMan.Database.Models.Shared;
using FinanMan.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FinanMan.Shared.DataEntryModels;

public class PaymentEntryViewModel : ITransactionDataEntryViewModel
{
    public TransactionType TransactionType => TransactionType.Payment;

    public int TransactionId { get; set; }

    public int? AccountId { get; set; }
    [Required]
    public string? AccountValueText
    {
        get => AccountId?.ToString();
        set
        {
            AccountId = int.TryParse(value ?? string.Empty, out var taid) ? taid : default;
        }
    }
    public string? AccountName { get; set; }
    public ICollection<PaymentDetailViewModel> LineItems { get; init; } = new List<PaymentDetailViewModel>();
    public string? Memo { get; set; }
    [Required]
    public string? PayeeValueText { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? PayeeName { get; set; }
    public DateTime? PostedDate { get; set; }
    [Required]
    public DateTime? TransactionDate { get; set; }

    [JsonIgnore]
    public double Total => Math.Round(LineItems.Where(x => x.Amount.HasValue).Sum(x => x.Amount!.Value), 2);

    [JsonIgnore]
    public int? PayeeId => int.TryParse(PayeeValueText ?? string.Empty, out var pid) ? pid : default;

}
