using FinanMan.Database.Models.Tables;

namespace FinanMan.Shared.LookupModels;

public class AccountLookupViewModel : LookupItemViewModel<Account>
{
    public string AccountName { get => Item.Name; set => Item.Name = value; }
    public int AccountTypeId
    {
        get => Item.AccountTypeId;
        set
        {
            Item.AccountTypeId = value;
            if (Item.AccountType is null) { Item.AccountType = new(); }
            Item.AccountType.Id = value;
        }
    }
    public string? AccountType
    {
        get => Item.AccountType?.Name ?? string.Empty;
        set
        {
            if (Item.AccountType is null) { Item.AccountType = new(); }
            Item.AccountType.Name = value ?? string.Empty;
        }
    }
}

public static class AccountViewModelExtensions
{
    public static AccountLookupViewModel ToViewModel(this Account account) =>
        new()
        {
            Id = account.Id,
            DisplayText = account.Name,
            ValueText = account.Id.ToString(),
            SortOrder = account.SortOrder,
            LastUpdated = account.LastUpdated,
            Item = account
        };
}
