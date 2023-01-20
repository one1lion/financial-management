using FinanMan.Database.Models.Shared;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FinanMan.Shared.DataEntryModels;

/// <summary>
/// The view model that holds deposit information
/// </summary>
public class DepositEntryViewModel : ITransactionDataEntryViewModel
{
    public TransactionType TransactionType => TransactionType.Deposit;

    public int TransactionId { get; set; }

    [Required]
    public DateTime? TransactionDate { get; set; }
    public DateTime? PostedDate { get; set; }

    public int? AccountId { get; set; }
    [Required]
    public string? TargetAccountValueText
    {
        get => AccountId?.ToString();
        set
        {
            AccountId = int.TryParse(value ?? string.Empty, out var taid) ? taid : default;
        }
    }
    public string? AccountName { get; set; }
    [Required]
    public string? DepositReasonValueText { get; set; }
    public string? DepositReasonDisplayText { get; set; }
    public string? Memo { get; set; }
    [Required]
    public decimal? Amount { get; set; }

    [JsonIgnore]
    public decimal Total => Amount ?? 0;

    [JsonIgnore]
    public int? TargetAccountId => int.TryParse(TargetAccountValueText ?? string.Empty, out var taid) ? taid : default;
    [JsonIgnore]
    public int? DepositReasonId => int.TryParse(DepositReasonValueText ?? string.Empty, out var drid) ? drid : default;
}
