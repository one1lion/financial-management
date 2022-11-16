using FinanMan.Database.Models.Shared;
using FinanMan.Shared.LookupModels;
using System.ComponentModel;

namespace FinanMan.Shared.StateInterfaces;

public interface ILookupListState : INotifyPropertyChanged, INotifyPropertyChanging
{
    List<ILookupItemViewModel> LookupItemCache { get; }
    bool Initialized { get; set; }
    bool Initializing { get; set; }
    Task Initialize();
    IEnumerable<ILookupItemViewModel<TKey, TLookupItem>> GetLookupItems<TKey, TLookupItem>()
         where TLookupItem : class, IHasLookupListType, new();
}
