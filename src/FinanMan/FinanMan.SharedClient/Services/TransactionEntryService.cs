using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.General;
using FinanMan.Shared.ServiceInterfaces;
using FinanMan.SharedClient.Extensions;
using System.Net.Http.Json;

namespace FinanMan.SharedClient.Services;

public class TransactionEntryService<TDataEntryViewModel> : ITransactionEntryService<TDataEntryViewModel>
    where TDataEntryViewModel : class, ITransactionDataEntryViewModel
{
    private readonly HttpClient _httpClient;
    private readonly string _controllerName;

    public TransactionEntryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _controllerName = this.GetApiEndpoint();
    }

    public Task<ResponseModel<List<TDataEntryViewModel>>?> GetTransactionsAsync(ushort startRecord = 0, ushort pageSize = 100, DateTime? asOfDate = null, bool includeMarkedAsPurge = false, CancellationToken ct = default)
    {
        var asOfDateQs = asOfDate.HasValue ? $"&aod={asOfDate.Value:yyyy-MM-dd'T'HH:mm:ss.fffffff}" : string.Empty;
        return _httpClient.GetFromJsonAsync<ResponseModel<List<TDataEntryViewModel>>>($"{_controllerName}?sr={startRecord}&ps={pageSize}&imp={includeMarkedAsPurge}{asOfDateQs}", ct);
    }

    public Task<ResponseModel<TDataEntryViewModel>?> GetTransactionAsync(int id, CancellationToken ct = default) =>
        _httpClient.GetFromJsonAsync<ResponseModel<TDataEntryViewModel>>($"{_controllerName}/{id}", ct);

    public async Task<ResponseModelBase<int>> AddTransactionAsync(TDataEntryViewModel dataEntryViewModel, CancellationToken ct = default)
    {
        var retResp = new ResponseModelBase<int>();
        try
        {
            var resp = await _httpClient.PostAsJsonAsync($"{_controllerName}", dataEntryViewModel, ct);
            if (resp.IsSuccessStatusCode)
            {
                retResp = await resp.Content.ReadFromJsonAsync<ResponseModelBase<int>>(cancellationToken: ct);
            }
            else
            {
                retResp.AddError($"The request to add the transaction failed.  The server responded with status: {resp.StatusCode} - {resp.ReasonPhrase}");
            }
        }
        catch (Exception ex)
        {
            retResp ??= new();

            var errMessage = ex switch
            {
                TaskCanceledException or OperationCanceledException => "The task was canceled.",
                // TODO: Do something with successStatus here - that is, we got a success response from the server,
                //       but deserializing the response model ended up causing an exception
                _ => "The add transaction operation failed unexpectedly."
            };

            retResp.AddError(errMessage);

#if DEBUG
            retResp.AddError(ex);
#endif
        }
        return retResp ?? new() { ErrorMessages = new() { "The add transaction request was successful, but we did not receive a response." } };
    }

    public async Task<ResponseModelBase<int>> UpdateTransactionAsync(TDataEntryViewModel dataEntryViewModel, CancellationToken ct = default)
    {
        var retResp = new ResponseModelBase<int>();
        try
        {
            var resp = await _httpClient.PutAsJsonAsync($"{_controllerName}", dataEntryViewModel, ct);
            if (resp.IsSuccessStatusCode)
            {
                retResp = await resp.Content.ReadFromJsonAsync<ResponseModelBase<int>>(cancellationToken: ct);
            }
            else
            {
                retResp.AddError($"The request to update the deposit failed.  The server responded with status: {resp.StatusCode} - {resp.ReasonPhrase}");
            }
        }
        catch (Exception ex)
        {
            retResp ??= new();

            var errMessage = ex switch
            {
                TaskCanceledException or OperationCanceledException => "The task was canceled.",
                // TODO: Do something with successStatus here - e.g., we got a success response from the server,
                //       but deserializing the response model ended up causing an exception
                _ => "The update transaction operation failed unexpectedly."
            };

            retResp.AddError(errMessage);

#if DEBUG
            retResp.AddError(ex);
#endif
        }
        return retResp ?? new() { ErrorMessages = new() { "The update transaction request was successful, but we did not receive a response." } };
    }

    public async Task<ResponseModelBase<int>> DeleteTransactionAsync(int id, CancellationToken ct = default)
    {
        var retResp = new ResponseModelBase<int>();
        try
        {
            var resp = await _httpClient.DeleteAsync($"{_controllerName}/{id}", ct);
            if (resp.IsSuccessStatusCode)
            {
                retResp = await resp.Content.ReadFromJsonAsync<ResponseModelBase<int>>(cancellationToken: ct);
            }
            else
            {
                retResp.AddError($"The request to delete the transaction failed.  The server responded with status: {resp.StatusCode} - {resp.ReasonPhrase}");
            }
        }
        catch (Exception ex)
        {
            retResp ??= new();

            var errMessage = ex switch
            {
                TaskCanceledException or OperationCanceledException => "The task was canceled.",
                // TODO: Do something with successStatus here - e.g., we got a success response from the server,
                //       but deserializing the response model ended up causing an exception
                _ => "The delete transaction operation failed unexpectedly."
            };

            retResp.AddError(errMessage);

#if DEBUG
            retResp.AddError(ex);
#endif
        }
        return retResp ?? new() { ErrorMessages = new() { "The delete transaction request was successful, but we did not receive a response." } };
    }
}
