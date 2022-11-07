using FinanMan.Shared.General;
using FinanMan.Shared.LookupModels;

namespace FinanMan.Shared.ServiceInterfaces;

public interface ILookupListService
{
    Task<ResponseModel<List<TLookupItemViewModel>>> GetLookupItemsAsync<TLookupItemViewModel>(int startRecord = 0, int pageSize = 100, DateTime? asOfDate = null, CancellationToken ct = default)
        where TLookupItemViewModel : class, ILookupItemViewModel;
    Task<ResponseModel<TLookupItemViewModel>> GetLookupItem<TLookupItemViewModel>(int id, CancellationToken ct = default)
        where TLookupItemViewModel : class, ILookupItemViewModel;
    Task<ResponseModelBase<int>> AddLookupItem<TLookupItemViewModel>(TLookupItemViewModel dataEntryViewModel, CancellationToken ct = default)
        where TLookupItemViewModel : class, ILookupItemViewModel;
    Task<ResponseModelBase<int>> UpdateLookupItem<TLookupItemViewModel>(TLookupItemViewModel dataEntryViewModel, CancellationToken ct = default)
        where TLookupItemViewModel : class, ILookupItemViewModel;
    Task<ResponseModelBase<int>> DeleteLookupItem<TLookupItemViewModel>(int id, CancellationToken ct = default)
        where TLookupItemViewModel : class, ILookupItemViewModel;
}
