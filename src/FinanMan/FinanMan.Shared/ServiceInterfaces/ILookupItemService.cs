using FinanMan.Shared.General;
using FinanMan.Shared.LookupModels;

namespace FinanMan.Shared.ServiceInterfaces;

public interface ILookupItemService<TLookupItemViewModel>
    where TLookupItemViewModel : class, ILookupItemViewModel
{
    Task<ResponseModel<List<TLookupItemViewModel>>> GetLookupItems(int startRecord = 0, int pageSize = 100, DateTime? asOfDate = null, CancellationToken ct = default);
    Task<ResponseModel<TLookupItemViewModel>> GetLookupItem(int id, CancellationToken ct = default);
    Task<ResponseModelBase<int>> AddLookupItem(TLookupItemViewModel dataEntryViewModel, CancellationToken ct = default);
    Task<ResponseModelBase<int>> UpdateLookupItem(TLookupItemViewModel dataEntryViewModel, CancellationToken ct = default);
    Task<ResponseModelBase<int>> DeleteLookupItem(int id, CancellationToken ct = default);
}

public interface ILookupItemService
{
    Task<ResponseModel<List<TLookupItemViewModel>>> GetLookupItems<TLookupItemViewModel>(int startRecord = 0, int pageSize = 100, DateTime? asOfDate = null, CancellationToken ct = default)
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
