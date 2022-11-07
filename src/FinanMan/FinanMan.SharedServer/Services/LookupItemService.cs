using FinanMan.Shared.General;
using FinanMan.Shared.LookupModels;
using FinanMan.Shared.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanMan.SharedServer.Services;

public class LookupItemService : ILookupListService
{
    public Task<ResponseModel<TLookupItemViewModel>> GetLookupItem<TLookupItemViewModel>(int id, CancellationToken ct = default)
        where TLookupItemViewModel : class, ILookupItemViewModel
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel<List<TLookupItemViewModel>>> GetLookupItemsAsync<TLookupItemViewModel>(int startRecord = 0, int pageSize = 100, DateTime? asOfDate = null, CancellationToken ct = default)
        where TLookupItemViewModel : class, ILookupItemViewModel
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModelBase<int>> AddLookupItem<TLookupItemViewModel>(TLookupItemViewModel dataEntryViewModel, CancellationToken ct = default)
        where TLookupItemViewModel : class, ILookupItemViewModel
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModelBase<int>> UpdateLookupItem<TLookupItemViewModel>(TLookupItemViewModel dataEntryViewModel, CancellationToken ct = default)
        where TLookupItemViewModel : class, ILookupItemViewModel
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModelBase<int>> DeleteLookupItem<TLookupItemViewModel>(int id, CancellationToken ct = default)
        where TLookupItemViewModel : class, ILookupItemViewModel
    {
        throw new NotImplementedException();
    }
}
