using FinanMan.Shared.DataEntryModels;
using FinanMan.Shared.General;
using FinanMan.Shared.ServiceInterfaces;
using System.Net.Http.Json;

namespace FinanMan.SharedClient.Services;

public class TransactionEntryService<TDataEntryViewModel> : ITransactionEntryService<TDataEntryViewModel>
    where TDataEntryViewModel : class, ITransactionDataEntryViewModel
{
    private readonly HttpClient _httpClient;

    public TransactionEntryService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public Task<ResponseModel<List<TDataEntryViewModel>>> GetTransactionsAsync(ushort startRecord = 0, ushort pageSize = 100, DateTime? asOfDate = null, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModel<TDataEntryViewModel>> GetTransactionAsync(int id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public async Task<ResponseModelBase<int>> AddTransactionAsync(TDataEntryViewModel dataEntryViewModel, CancellationToken ct = default)
    {
        var retResp = new ResponseModelBase<int>();
        var successStatus = false;
        try
        {
            var resp = await _httpClient.PostAsJsonAsync("api/Deposits", dataEntryViewModel, ct);
            if (resp.IsSuccessStatusCode)
            {
                successStatus = true;
                retResp = await resp.Content.ReadFromJsonAsync<ResponseModelBase<int>>(cancellationToken: ct);
            }
            else
            {
                retResp.AddError($"The request to add the deposit failed.  The server responded with status: {resp.StatusCode} - {resp.ReasonPhrase}");
            }
        }
        catch (Exception ex)
        {
            retResp ??= new();

            var errMessage = ex switch
            {
                TaskCanceledException or OperationCanceledException => "The task was canceled.",
                _ => "The add deposit transaction operation failed unexpectedly."
            };

            // TODO: Do something with successStatus here - that is, we got a success response from the server,
            //       but deserializing the response model ended up causing an exception
            retResp.AddError(errMessage);

#if DEBUG
            retResp.AddError(ex);
#endif
        }
        return retResp ?? new() { ErrorMessages = new() { "The add deposit transaction request was successful, but we did not receive a response." } };
    }

    public Task<ResponseModelBase<int>> UpdateTransactionAsync(TDataEntryViewModel dataEntryViewModel, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }

    public Task<ResponseModelBase<int>> DeleteTransactionAsync(int id, CancellationToken ct = default)
    {
        throw new NotImplementedException();
    }
}
