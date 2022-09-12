using FinanMan.Shared.Enums;
using System.ComponentModel.DataAnnotations;

namespace FinanMan.Shared.DataEntryModels;

public class LineItemViewModel
{
    [Required]
    public string? LineItemTypeValueText { get; set; }
    public string? Detail { get; set; }
    [Required]
    public double? Amount { get; set; }

}
