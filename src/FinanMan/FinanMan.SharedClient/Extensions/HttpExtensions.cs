using System.Net.Http.Json;
using System.Runtime.CompilerServices;

using FinanMan.Shared.General;

namespace FinanMan.SharedClient.Extensions;

public static class HttpExtensions
{
    /// <inheritdoc />
    public static async Task<TResponse> ExecuteRequestAsync<TResponse>(this HttpClient httpClient, HttpMethod httpMethod, string requestUrl, CancellationToken ct,
            object? payload = null, [CallerMemberName] string methodName = "")
        where TResponse : IResponseModel, new()
    {
        var retResp = new TResponse();
        try
        {
            requestUrl = $"{requestUrl.TrimEnd('/')}/{methodName.ToLowerInvariant()}";
            var httpRequest = new HttpRequestMessage(httpMethod, requestUrl);
            if ((httpMethod == HttpMethod.Post || httpMethod == HttpMethod.Put) && payload is not null)
            {
                httpRequest.Content = JsonContent.Create(payload, payload.GetType());
            }
            var resp = await httpClient.SendAsync(httpRequest, cancellationToken: ct);

            if (resp is null)
            {
                retResp.AddError($"Invalid request to {methodName} the data. The server responded failed to provide a resopnse.");
                return retResp;
            }

            if (!resp.IsSuccessStatusCode)
            {
                retResp.AddError($"Invalid request to {methodName} the data. The server responded with {resp.StatusCode}: {resp.ReasonPhrase}");
                return retResp;
            }

            var forRet = await resp.Content.ReadFromJsonAsync<TResponse>(cancellationToken: ct).ConfigureAwait(false);
            return retResp ?? new() { ErrorMessages = ["Failed to receive a response."] };
        }
        catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
        {
            retResp.AddError($"Invalid request to {methodName} the data. Not Found.");
            return retResp;
        }
        catch (Exception ex)
        {
            retResp.AddError($"An error occurred while trying to {methodName} the data. {ex.Message}");
            return retResp;
        }
    }
}