using FinanMan.Abstractions.ModelInterfaces.LookupModels;

namespace FinanMan.Abstractions.StateInterfaces;

public interface ILookupListState
{
    List<ILookupItemViewModel> LookupItemCache { get; }
    bool Initialized { get; set; }
    bool Initializing { get; set; }
    Task Initialize();
    IEnumerable<ILookupItemViewModel<TKey, TLookupItem>> GetLookupItems<TKey, TLookupItem>();
}
