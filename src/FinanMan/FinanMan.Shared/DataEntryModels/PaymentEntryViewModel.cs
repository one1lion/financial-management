using FinanMan.Database.Models.Shared;
using FinanMan.Shared.Enums;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FinanMan.Shared.DataEntryModels;

public class PaymentEntryViewModel : ITransactionDataEntryViewModel
{
    public TransactionType TransactionType => TransactionType.Payment;

    public int TransactionId { get; set; }
    public DateTime DateEntered { get; set; } = DateTime.UtcNow;

    public int PaymentId { get; set; }
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
    public ICollection<PaymentDetailViewModel> LineItems { get; set; } = new List<PaymentDetailViewModel>();
    public string? Memo { get; set; }
    [Required]
    public string? PayeeValueText { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? PayeeName { get; set; }
    public DateTime? PostedDate { get; set; }
    [Required]
    public DateTime? TransactionDate { get; set; }

    [JsonIgnore]
    public decimal Total => Math.Round(LineItems.Where(x => x.Amount.HasValue).Sum(x => x.Amount!.Value), 2);

    [JsonIgnore]
    public int? PayeeId => int.TryParse(PayeeValueText ?? string.Empty, out var pid) ? pid : default;

    public object Clone()
    {
        var clonedPayment = (PaymentEntryViewModel)MemberwiseClone();
        clonedPayment.LineItems = LineItems.Select(x => x.Clone()).OfType<PaymentDetailViewModel>().ToList();

        return clonedPayment;
    }

    public void Patch(ITransactionDataEntryViewModel source)
    {
        if (source is PaymentEntryViewModel sourceModel)
        {
            PaymentId = sourceModel.PaymentId;
            TransactionId = sourceModel.TransactionId;
            AccountId = sourceModel.AccountId;
            AccountName = sourceModel.AccountName;
            LineItems = sourceModel.LineItems.Select(x => x.Clone()).OfType<PaymentDetailViewModel>().ToList();
            Memo = sourceModel.Memo;
            PayeeValueText = sourceModel.PayeeValueText;
            PayeeName = sourceModel.PayeeName;
            PostedDate = sourceModel.PostedDate;
            TransactionDate = sourceModel.TransactionDate;
        }
    }

}
