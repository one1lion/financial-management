namespace FinanMan.Abstractions.ModelInterfaces.DataEntryModels;

public interface ILineItemViewModel
{
    int? LineItemTypeId { get; set; }
    string? Detail { get; set; }
    double? Amount { get; set; }
}
