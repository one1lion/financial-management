namespace FinanMan.Abstractions.ModelInterfaces.DataEntryModels;

public interface ITransferViewModel
{
    int? SourceAccountId { get; set; }
    int? TargetAccountId { get; set; }
    string? Memo { get; set; }
    double? Amount { get; set; }
    DateTime? TransactionDate { get; set; }
    DateTime? PostedDate { get; set; }
}
