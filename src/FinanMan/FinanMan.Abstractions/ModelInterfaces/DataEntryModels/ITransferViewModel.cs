namespace FinanMan.Abstractions.ModelInterfaces.DataEntryModels;

public interface ITransferViewModel
{
    string? SourceAccountValueText { get; set; }
    string? TargetAccountValueText { get; set; }
    string? Memo { get; set; }
    double? Amount { get; set; }
    DateTime? TransactionDate { get; set; }
    DateTime? PostedDate { get; set; }
}
