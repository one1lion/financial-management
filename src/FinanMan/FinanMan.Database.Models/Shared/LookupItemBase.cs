namespace FinanMan.Database.Models.Shared;

public abstract class LookupItemBase : LookupItemBase<int> { }

public abstract class LookupItemBase<TKey> : ILookupItem<TKey>, IHasLookupListType
{
    public abstract LookupListType ListType { get; }
    public virtual TKey Id { get; set; } = default!;
    public virtual string Name { get; set; } = default!;
    public virtual int SortOrder { get; set; }
    public virtual DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    public virtual bool Deleted { get; set; }
}
