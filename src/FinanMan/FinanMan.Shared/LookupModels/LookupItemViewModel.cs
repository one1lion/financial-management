using FinanMan.Database.Models.Shared;
using FinanMan.Database.Models.Tables;
using System.Text.Json.Serialization;

namespace FinanMan.Shared.LookupModels;

public class LookupItemViewModel<TItem> : LookupItemViewModel<int, TItem>
    where TItem : class, IHasLookupListType, new() { }

public class LookupItemViewModel<TKey, TItem> : ILookupItemViewModel<TKey, TItem>, IHasLookupListType
    where TItem : class, IHasLookupListType, new()
{
    [JsonIgnore]
    public LookupListType ListType => Type switch
    {
        var t when t == typeof(Account) => LookupListType.Accounts,
        var t when t == typeof(LuAccountType) => LookupListType.AccountTypes,
        var t when t == typeof(LuCategory) => LookupListType.Categories,
        var t when t == typeof(LuDepositReason) => LookupListType.DepositReasons,
        var t when t == typeof(LuLineItemType) => LookupListType.LineItemTypes,
        var t when t == typeof(Payee) => LookupListType.Payees,
        var t when t == typeof(LuRecurrenceType) => LookupListType.RecurrenceTypes,
        _ => LookupListType.Accounts
    };
    [JsonIgnore]
    public Type Type => typeof(TItem);
    public TKey Id { get; set; } = default!;
    public string ListItemId => $"{Type}-{Id}";
    public string DisplayText { get; set; } = default!;
    public string ValueText { get; set; } = default!;
    public int SortOrder { get; set; }
    public DateTime? LastUpdated { get; set; }

    public TItem Item { get; set; } = new TItem();
}
