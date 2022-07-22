namespace FinanMan.Database.Models.Shared;

public interface ILookupItem<TKey>
{
    TKey Id { get; set; }
    string Name { get; set; }
    int SortOrder { get; set; }
    DateTime LastUpdated { get; set; }
    bool Deleted { get; set; }
}
