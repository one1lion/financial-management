using FinanMan.Abstractions.ModelInterfaces.DataEntryModels;

namespace FinanMan.Shared.DataEntryModels;

public class TransferViewModel : ITransferViewModel
{
    private int? _sourceAccountId;
    public int? SourceAccountId
    {
        get => _sourceAccountId;
        set
        {
            if (_sourceAccountId == value) { return; }
            _sourceAccountId = value;
            if(TargetAccountId == value) { TargetAccountId = null; }
        }
    }
    public int? TargetAccountId { get; set; }
    public string? Memo { get; set; }
    public double? Amount { get; set; }
    public DateTime? TransactionDate { get; set; }
    public DateTime? PostedDate { get; set; }
}
