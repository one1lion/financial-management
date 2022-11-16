using FinanMan.Database.Models.Shared;

namespace FinanMan.Shared.LookupModels;

public class LookupItemViewModel<TItem> : LookupItemViewModel<int, TItem>
    where TItem : class, IHasLookupListType, new() { }

public class LookupItemViewModel<TKey, TItem> : ILookupItemViewModel<TKey, TItem>, IHasLookupListType
    where TItem : class, IHasLookupListType, new()
{
    public LookupListType ListType => Item?.ListType ?? LookupListType.Accounts;
    public Type Type => typeof(TItem);
    public TKey Id { get; set; } = default!;
    public string ListItemId => $"{Type}-{Id}";
    public string DisplayText { get; set; } = default!;
    public string ValueText { get; set; } = default!;
    public int SortOrder { get; set; }
    public DateTime? LastUpdated { get; set; }

    public TItem Item { get; set; } = new TItem();
}
