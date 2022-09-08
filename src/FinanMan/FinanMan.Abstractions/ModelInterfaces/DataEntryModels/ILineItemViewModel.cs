namespace FinanMan.Abstractions.ModelInterfaces.DataEntryModels;

public interface ILineItemViewModel
{
    string? LineItemTypeValueText { get; set; }
    string? Detail { get; set; }
    double? Amount { get; set; }
}
