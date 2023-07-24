using FinanMan.Database.Models.Shared;
using FinanMan.Shared.General;
using FinanMan.Shared.LookupModels;

namespace FinanMan.Shared.ServiceInterfaces;

public interface ILookupListService
{
    Task<ResponseModel<List<TLookupItemViewModel>>> GetLookupItemsAsync<TLookupItemViewModel>(int startRecord = 0, int pageSize = 100, DateTime? asOfDate = null, CancellationToken ct = default)
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType, new();
    Task<ResponseModel<TLookupItemViewModel>> GetLookupItemAsync<TLookupItemViewModel>(int id, CancellationToken ct = default)
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType, new();
    Task<ResponseModelBase<int>> CreateLookupItemAsync<TLookupItemViewModel>(TLookupItemViewModel dataEntryViewModel, CancellationToken ct = default)
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType, new();
    Task<ResponseModel<ILookupItemViewModel>> UpdateLookupItemAsync<TLookupItemViewModel>(TLookupItemViewModel dataEntryViewModel, CancellationToken ct = default)
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType, new();
    Task<ResponseModelBase<int>> DeleteLookupItemAsync<TLookupItemViewModel>(int id, CancellationToken ct = default)
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType, new();
}
