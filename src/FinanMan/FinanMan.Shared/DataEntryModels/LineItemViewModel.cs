using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FinanMan.Shared.DataEntryModels;

public class LineItemViewModel
{
    [Required]
    public string? LineItemTypeValueText { get; set; }
    public string? Detail { get; set; }
    [Required]
    public double? Amount { get; set; }

    [JsonIgnore]
    public int? LineItemTypeId => int.TryParse(LineItemTypeValueText ?? string.Empty, out var litid) ? litid : default;
}
