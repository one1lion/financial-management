namespace FinanMan.Database.Models.Shared;

public interface ILookupItem : IHasLookupListType
{
    string Name { get; set; }
    int SortOrder { get; set; }
    DateTime LastUpdated { get; set; }
    bool Deleted { get; set; }
}

public interface ILookupItem<TKey> : ILookupItem
{
    TKey Id { get; set; }
}
