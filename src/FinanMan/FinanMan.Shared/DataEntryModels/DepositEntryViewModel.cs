using FinanMan.Database.Models.Shared;
using FinanMan.Shared.LookupModels;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FinanMan.Shared.DataEntryModels;

/// <summary>
/// The view model that holds deposit information
/// </summary>
public class DepositEntryViewModel : ITransactionDataEntryViewModel
{
    public TransactionType TransactionType => TransactionType.Deposit;

    public int DepositId { get; set; }
    public int TransactionId { get; set; }
    public DateTime DateEntered { get; set; } = DateTime.UtcNow;

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

    public object Clone() => MemberwiseClone();

    public void Patch(ITransactionDataEntryViewModel source)
    {
        if (source is DepositEntryViewModel sourceModel)
        {
            DepositId = sourceModel.DepositId;
            TransactionId = sourceModel.TransactionId;
            TransactionDate = sourceModel.TransactionDate;
            PostedDate = sourceModel.PostedDate;
            AccountId = sourceModel.AccountId;
            AccountName = sourceModel.AccountName;
            DepositReasonValueText = sourceModel.DepositReasonValueText;
            DepositReasonDisplayText = sourceModel.DepositReasonDisplayText;
            Memo = sourceModel.Memo;
            Amount = sourceModel.Amount;
        }
    }

    public void UpdateAccountName(IEnumerable<AccountLookupViewModel> accounts)
    {
        AccountName = accounts?.FirstOrDefault(x => x.ValueText == TargetAccountValueText)?.DisplayText ?? string.Empty;
    }
}
