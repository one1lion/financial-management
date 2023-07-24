using FinanMan.Database.Models.Shared;
using FinanMan.Shared.General;
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
    Task<ResponseModel<ILookupItemViewModel>> CreateLookupItemAsync<TLookupItem>(TLookupItem lookupItem)
        where TLookupItem : class, ILookupItemViewModel, IHasLookupListType, new();
    Task<ResponseModel<ILookupItemViewModel>> UpdateLookupItemAsync<TLookupItemViewModel>(TLookupItemViewModel dataEntryViewModel, CancellationToken ct = default)
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType, new();
    Task<ResponseModelBase<int>> DeleteLookupItemAsync<TLookupItemViewModel>(TLookupItemViewModel item, CancellationToken ct = default)
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType, new();
    Task RefreshListAsync<TLookupItem>()
        where TLookupItem : class, ILookupItemViewModel, IHasLookupListType, new();
}
