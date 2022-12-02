using FinanMan.Database.Models.Tables;

namespace FinanMan.Shared.LookupModels;

public class AccountViewModel : LookupItemViewModel<Account>
{
    public string AccountName { get; set; } = default!;
    public string AccountType { get; set; } = default!;
}

public static class AccountViewModelExtensions
{
    public static AccountViewModel ToViewModel(this Account account) =>
        new() { 
            Id = account.Id,
            AccountName = account.Name,
            AccountType = account.AccountType?.Name ?? string.Empty,
            DisplayText = account.Name,
            ValueText = account.Id.ToString(),
            SortOrder = account.Id,
            Item = account
        };
}
