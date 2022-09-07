namespace FinanMan.Abstractions.ModelInterfaces.DataEntryModels;

public interface IDepositViewModel
{
    DateTime? TransactionDate { get; set; }
    DateTime? PostedDate { get; set; }
    int? TargetAccountId { get; set; }
    int? DepositReasonId { get; set; }
    string? Memo { get; set; }
    double? Amount { get; set; }
}
