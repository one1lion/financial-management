using FinanMan.Abstractions.ModelInterfaces.DataEntryModels;

namespace FinanMan.Shared.DataEntryModels;

/// <summary>
/// The view model that holds deposit information
/// </summary>
public class DepositViewModel : IDepositViewModel
{
    public DateTime? TransactionDate { get; set; }
    public DateTime? PostedDate { get; set; }
    public int? TargetAccountId { get; set; }
    public int? DepositReasonId { get; set; }
    public string? Memo { get; set; }
    public double? Amount { get; set; }
}
