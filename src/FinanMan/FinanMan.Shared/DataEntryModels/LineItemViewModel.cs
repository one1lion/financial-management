using FinanMan.Abstractions.ModelInterfaces.DataEntryModels;

namespace FinanMan.Shared.DataEntryModels;

public class LineItemViewModel : ILineItemViewModel
{
    public int? LineItemTypeId { get; set; }
    public string? Detail { get; set; }
    public double? Amount { get; set; }
}
