using FinanMan.Abstractions.ModelInterfaces.DataEntryModels;
using System.ComponentModel.DataAnnotations;

namespace FinanMan.Shared.DataEntryModels;

public class LineItemViewModel : ILineItemViewModel
{
    [Required]
    public int? LineItemTypeId { get; set; }
    public string? Detail { get; set; }
    [Required]
    public double? Amount { get; set; }
}
