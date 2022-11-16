using FinanMan.Database.Models.Shared;
using FinanMan.Database.Models.Tables;
using FinanMan.Shared.General;
using FinanMan.Shared.LookupModels;
using FinanMan.Shared.ServiceInterfaces;
using System.Net.Http.Json;

namespace FinanMan.SharedClient.Services;

public class LookupItemService : ILookupListService
{
    private const string _urlPattern = "api/Lookups/{0}";
    private readonly HttpClient _httpClient;

    public LookupItemService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<ResponseModel<List<TLookupItemViewModel>>> GetLookupItemsAsync<TLookupItemViewModel>(int startRecord, int pageSize, DateTime? asOfDate, CancellationToken ct)
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType
    {
        var retResp = new ResponseModel<List<TLookupItemViewModel>>();
        var typeInst = Activator.CreateInstance<TLookupItemViewModel>();
        var endpointType = typeof(TLookupItemViewModel) switch
        {
            var t when t == typeof(AccountViewModel) => LookupListType.Accounts,
            var t when t == typeof(LookupItemViewModel<LuAccountType>) => LookupListType.AccountTypes,
            var t when t == typeof(LookupItemViewModel<LuCategory>) => LookupListType.Categories,
            var t when t == typeof(LookupItemViewModel<LuDepositReason>) => LookupListType.DepositReasons,
            var t when t == typeof(LookupItemViewModel<LuLineItemType>) => LookupListType.LineItemTypes,
            var t when t == typeof(PayeeViewModel) => LookupListType.Payees,
            var t when t == typeof(LookupItemViewModel<LuRecurrenceType>) => LookupListType.RecurrenceTypes,
            _ => throw new NotImplementedException(),
        };

        try
        {
            var resp = await _httpClient.GetFromJsonAsync<ResponseModel<List<TLookupItemViewModel>>>(string.Format(_urlPattern, endpointType.ToString()), ct);
            if (resp is not null)
            {
                retResp = resp;
            }
        }
        catch (Exception ex)
        {
            retResp.AddError($"The request to get the list of {typeInst.ListType} failed.");
#if DEBUG
            retResp.AddError(ex);
#endif
        }
        return retResp;
    }

    public async Task<ResponseModel<TLookupItemViewModel>> GetLookupItem<TLookupItemViewModel>(int id, CancellationToken ct)
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType
    {
        var retResp = new ResponseModel<TLookupItemViewModel>();
        var typeInst = Activator.CreateInstance<TLookupItemViewModel>();

        try
        {
            var resp = await _httpClient.GetFromJsonAsync<ResponseModel<TLookupItemViewModel>>(string.Format(_urlPattern, typeInst.ListType.ToString()), ct);
            if (resp is not null)
            {
                retResp = resp;
            }
        }
        catch (Exception ex)
        {
            retResp.AddError($"The request to get the specified {typeInst.ListType} failed.");
#if DEBUG
            retResp.AddError(ex);
#endif
        }
        return retResp;
    }

    public Task<ResponseModelBase<int>> AddLookupItem<TLookupItemViewModel>(TLookupItemViewModel dataEntryViewModel, CancellationToken ct)
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType
    {
        var typeInst = Activator.CreateInstance<TLookupItemViewModel>();
        throw new NotImplementedException();
    }

    public Task<ResponseModelBase<int>> UpdateLookupItem<TLookupItemViewModel>(TLookupItemViewModel dataEntryViewModel, CancellationToken ct)
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType
    {
        var typeInst = Activator.CreateInstance<TLookupItemViewModel>();
        throw new NotImplementedException();
    }

    public Task<ResponseModelBase<int>> DeleteLookupItem<TLookupItemViewModel>(int id, CancellationToken ct)
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType
    {
        var typeInst = Activator.CreateInstance<TLookupItemViewModel>();
        throw new NotImplementedException();
    }
}
