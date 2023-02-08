using FinanMan.Database.Models.Shared;
using FinanMan.Database.Models.Tables;
using System.Text.Json.Serialization;

namespace FinanMan.Shared.DataEntryModels;

public class TransferEntryViewModel : ITransactionDataEntryViewModel
{
    public TransactionType TransactionType => TransactionType.Transfer;

    public int TransferId { get; set; }
    public int TransactionId { get; set; }

    public int? AccountId { get; set; }
    public string? SourceAccountValueText
    {
        get => AccountId?.ToString();
        set
        {
            AccountId = int.TryParse(value ?? string.Empty, out var taid) ? taid : default;
            if (TargetAccountValueText == value) { TargetAccountValueText = null; }
        }
    }
    public string? AccountName { get; set; }
    public string? TargetAccountValueText { get; set; }
    [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
    public string? TargetAccountName { get; set; }
    public string? Memo { get; set; }
    public decimal? Amount { get; set; }
    public DateTime? TransactionDate { get; set; }
    public DateTime? PostedDate { get; set; }
    
    [JsonIgnore]
    public decimal Total => Amount ?? 0;
    [JsonIgnore]
    public int? SourceAccountId => int.TryParse(SourceAccountValueText ?? string.Empty, out var aid) ? aid : default;
    [JsonIgnore]
    public int? TargetAccountId => int.TryParse(TargetAccountValueText ?? string.Empty, out var pid) ? pid : default;

    public object Clone() => MemberwiseClone();

    public void Patch(ITransactionDataEntryViewModel source)
    {
        if (source is TransferEntryViewModel sourceModel)
        {
            TransferId = sourceModel.TransferId;
            TransactionId = sourceModel.TransactionId;
            AccountId = sourceModel.AccountId;
            AccountName = sourceModel.AccountName;
            TargetAccountValueText = sourceModel.TargetAccountValueText;
            TargetAccountName = sourceModel.TargetAccountName;
            Memo = sourceModel.Memo;
            Amount = sourceModel.Amount;
            TransactionDate = sourceModel.TransactionDate;
            PostedDate = sourceModel.PostedDate;
        }
    }

}
