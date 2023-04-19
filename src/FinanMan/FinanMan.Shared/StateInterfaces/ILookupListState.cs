using FinanMan.Database.Models.Shared;
using FinanMan.Shared.LookupModels;
using System.ComponentModel;

namespace FinanMan.Shared.StateInterfaces;

public interface ILookupListState : INotifyPropertyChanged, INotifyPropertyChanging
{
    List<ILookupItemViewModel> LookupItemCache { get; }
    bool Initialized { get; set; }
    bool Initializing { get; set; }
    Task InitializeAsync();
    
    IEnumerable<TLookupItem> GetLookupItems<TLookupItem>()
         where TLookupItem : class, ILookupItemViewModel, IHasLookupListType, new();
}
