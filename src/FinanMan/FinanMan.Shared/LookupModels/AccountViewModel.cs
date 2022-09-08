using FinanMan.Database.Models.Tables;

namespace FinanMan.Shared.LookupModels;

public class AccountViewModel
{
    public int AccountId { get; set; }
    public string AccountName { get; set; } = default!;
    public string AccountType { get; set; } = default!;
}

public static class AccountViewModelExtensions
{
    public static AccountViewModel ToViewModel(this Account account) =>
        new() { 
            AccountId = account.Id,
            AccountName = account.Name,
            AccountType = account.AccountType?.Name ?? string.Empty
        };
}