using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace FinanMan.Shared.DataEntryModels;

public class PaymentDetailViewModel : ICloneable
{
    [Required]
    public string? LineItemTypeValueText { get; set; }
    public string? LineItemTypeName { get; set; }
    public string? Detail { get; set; }
    public int SortOrder { get; set; }
    [Required]
    public decimal? Amount { get; set; }

    [JsonIgnore]
    public int? LineItemTypeId => int.TryParse(LineItemTypeValueText ?? string.Empty, out var litid) ? litid : default;

    public object Clone() => MemberwiseClone();
}
