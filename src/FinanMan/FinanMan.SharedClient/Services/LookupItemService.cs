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
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType, new()
    {
        var retResp = new ResponseModel<List<TLookupItemViewModel>>();
        var typeInst = new TLookupItemViewModel();

        try
        {
            var resp = await _httpClient.GetFromJsonAsync<ResponseModel<List<TLookupItemViewModel>>>(string.Format(_urlPattern, typeInst.ListType.ToString()), ct);
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
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType, new()
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

    public async Task<ResponseModelBase<int>> AddLookupItem<TLookupItemViewModel>(TLookupItemViewModel dataEntryViewModel, CancellationToken ct)
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType, new()
    {
        var retResp = new ResponseModelBase<int>();
        var typeInst = Activator.CreateInstance<TLookupItemViewModel>();

        try
        {
            var resp = await _httpClient.PostAsJsonAsync(string.Format(_urlPattern, dataEntryViewModel.ListType.ToString()), dataEntryViewModel, ct);
            if(!resp.IsSuccessStatusCode)
            {
                retResp.AddError($"The request to add the specified {dataEntryViewModel.ListType} failed.  The server responded with {resp.StatusCode}: {resp.ReasonPhrase}");
                return retResp;
            }

            retResp = await resp.Content.ReadFromJsonAsync<ResponseModelBase<int>>(cancellationToken: ct) ?? new()
            {
                ErrorMessages = new() { $"The request to add the specified {dataEntryViewModel.ListType} was successful, however the response could not be parsed." }
            };
        }
        catch (Exception ex)
        {
            retResp = new();
            retResp.AddError($"The request to add the specified {dataEntryViewModel.ListType} failed.");
#if DEBUG
            retResp.AddError(ex);
#endif
        }

        return retResp;
    }

    public async Task<ResponseModelBase<int>> UpdateLookupItem<TLookupItemViewModel>(TLookupItemViewModel dataEntryViewModel, CancellationToken ct)
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType, new()
    {
        var retResp = new ResponseModelBase<int>();
        var typeInst = Activator.CreateInstance<TLookupItemViewModel>();

        try
        {
            var resp = await _httpClient.PutAsJsonAsync(string.Format(_urlPattern, dataEntryViewModel.ListType.ToString()), dataEntryViewModel, ct);
            if (!resp.IsSuccessStatusCode)
            {
                retResp.AddError($"The request to add the specified {dataEntryViewModel.ListType} failed.  The server responded with {resp.StatusCode}: {resp.ReasonPhrase}");
                return retResp;
            }

            retResp = await resp.Content.ReadFromJsonAsync<ResponseModelBase<int>>(cancellationToken: ct) ?? new()
            {
                ErrorMessages = new() { $"The request to add the specified {dataEntryViewModel.ListType} was successful, however the response could not be parsed." }
            };
        }
        catch (Exception ex)
        {
            retResp = new();
            retResp.AddError($"The request to add the specified {dataEntryViewModel.ListType} failed.");
#if DEBUG
            retResp.AddError(ex);
#endif
        }

        return retResp;
    }

    public async Task<ResponseModelBase<int>> DeleteLookupItem<TLookupItemViewModel>(int id, CancellationToken ct)
        where TLookupItemViewModel : class, ILookupItemViewModel, IHasLookupListType, new()
    {
        var retResp = new ResponseModelBase<int>();
        var typeInst = Activator.CreateInstance<TLookupItemViewModel>();

        try
        {
            var resp = await _httpClient.DeleteFromJsonAsync<ResponseModelBase<int>>($"{string.Format(_urlPattern, typeInst.ListType.ToString())}/{id}", ct);
            if (resp is not null)
            {
                retResp = resp;
            }
        }
        catch (Exception ex)
        {
            retResp.AddError($"The request to delete the specified {typeInst.ListType} failed.");
#if DEBUG
            retResp.AddError(ex);
#endif
        }
        return retResp;
    }
}
