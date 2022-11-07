namespace FinanMan.Shared.LookupModels;

public interface ILookupItemViewModel
{
    string ListItemId { get; }
    string DisplayText { get; set; }
    string ValueText { get; set; }
    int SortOrder { get; set; }
    DateTime? LastUpdated { get; set; }
}


public interface ILookupItemViewModel<TKey> : ILookupItemViewModel
{
    TKey Id { get; set; }

}

public interface ILookupItemViewModel<TKey, TItem> : ILookupItemViewModel<TKey>
{
    TItem? Item { get; set; }
    Type Type { get; }
}
