using KjbTech.Messaging.Sdk.Exceptions;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace KjbTech.Messaging.Sdk;

public abstract class MessagingHttpApiBase
{
    protected readonly HttpClient _httpClient;

    protected MessagingHttpApiBase(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    protected async Task<Result<TWhenSuccess?, HttpResponseMessage>> ProcessRequestAsync<TWhenSuccess>(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        var response = await ProcessRequestAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var result = await response.Content.ReadFromJsonAsync<TWhenSuccess>(cancellationToken);
            if (result is null)
            {
                throw new MessagingException("It seems that the error format got from the API has changed.");
            }
            return result;
        }

        return response;
    }

    protected async Task<HttpResponseMessage> ProcessRequestAsync(
        HttpRequestMessage request,
        CancellationToken cancellationToken)
    {
        if (!request.Headers.Accept.Any())
        {
            request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        }

        try
        {
            var response = await _httpClient.SendAsync(request, cancellationToken);

            return response;
        }
        catch (OperationCanceledException) when (!cancellationToken.IsCancellationRequested)
        {
            // Distinguish timeout from caller cancellation
            throw new MessagingException("The request timed out.");
        }
        catch (HttpRequestException ex)
        {
            throw new MessagingException($"Network error while calling {Describe(request)}.", ex);
        }
        catch (TaskCanceledException ex)
        {
            // Could be timeout or caller cancellation; message is still helpful.
            throw new MessagingException($"The request was canceled: {Describe(request)}.", ex);
        }
    }

    private static string Describe(HttpRequestMessage httpRequestMessage) => $"{httpRequestMessage.Method} {httpRequestMessage.RequestUri}";
}
