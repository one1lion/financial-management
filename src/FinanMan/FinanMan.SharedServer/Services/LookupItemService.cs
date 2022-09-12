using FinanMan.Shared.General;
using FinanMan.Shared.LookupModels;
using FinanMan.Shared.ServiceInterfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FinanMan.SharedServer.Services;

public class LookupItemService<TLookupItemViewModel> : ILookupItemService<TLookupItemViewModel>
    where TLookupItemViewModel : class, ILookupItemViewModel
{
    public Task<ResponseModel<TLookupItemViewModel>> GetLookupItem(int id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel<List<TLookupItemViewModel>>> GetLookupItems(int startRecord = 0, int pageSize = 100, DateTime? asOfDate = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModelBase<int>> AddLookupItem(TLookupItemViewModel dataEntryViewModel, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModelBase<int>> UpdateLookupItem(TLookupItemViewModel dataEntryViewModel, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModelBase<int>> DeleteLookupItem(int id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
