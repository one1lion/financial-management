namespace FinanMan.Shared.LookupModels;

public class LookupItemViewModel<TItem> : LookupItemViewModel<int, TItem> { }

public class LookupItemViewModel<TKey, TItem> : ILookupItemViewModel<TKey, TItem>
{
    public Type Type => typeof(TItem);
    public TKey Id { get; set; } = default!;
    public string ListItemId => $"{Type}-{Id}";
    public string DisplayText { get; set; } = default!;
    public string ValueText { get; set; } = default!;
    public int SortOrder { get; set; }
    public DateTime? LastUpdated { get; set; }

    public TItem? Item { get; set; }
}
