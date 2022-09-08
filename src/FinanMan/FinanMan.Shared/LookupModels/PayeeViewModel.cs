using FinanMan.Database.Models.Tables;

namespace FinanMan.Shared.LookupModels;

public class PayeeViewModel
{
    public int PayeeId { get; set; }
    public string PayeeName { get; set; } = default!;
    public IEnumerable<string> Categories { get; set; } = Array.Empty<string>();
}

public static class PayeeViewModelExtensions
{
    public static PayeeViewModel ToViewModel(this Payee payee) =>
        new()
        {
            PayeeId = payee.Id,
            PayeeName = payee.Name,
            Categories = payee.Categories?.Select(x => x.Name)?.ToArray() ?? Array.Empty<string>()
        };
}