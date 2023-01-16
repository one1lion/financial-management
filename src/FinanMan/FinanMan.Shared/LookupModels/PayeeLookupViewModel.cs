using FinanMan.Database.Models.Tables;

namespace FinanMan.Shared.LookupModels;

public class PayeeLookupViewModel : LookupItemViewModel<Payee>
{
    public string PayeeName { get => DisplayText; set => DisplayText = value; } 
    public IEnumerable<string> Categories { get; set; } = Array.Empty<string>();
}

public static class PayeeViewModelExtensions
{
    public static PayeeLookupViewModel ToViewModel(this Payee payee) =>
        new()
        {
            Id = payee.Id,
            DisplayText = payee.Name,
            ValueText = payee.Id.ToString(),
            Categories = payee.Categories?.Select(x => x.Name)?.ToArray() ?? Array.Empty<string>(),
            LastUpdated = payee.LastUpdated,
            SortOrder = payee.SortOrder,
            Item = payee
        };
}