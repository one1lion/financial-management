namespace FinanMan.Abstractions.ModelInterfaces.DataEntryModels;

public interface IDepositViewModel
{
    DateTime? TransactionDate { get; set; }
    DateTime? PostedDate { get; set; }
    string? TargetAccountValueText { get; set; }
    string? DepositReasonValueText { get; set; }
    string? Memo { get; set; }
    double? Amount { get; set; }
}
