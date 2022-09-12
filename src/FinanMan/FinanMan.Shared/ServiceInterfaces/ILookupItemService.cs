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
