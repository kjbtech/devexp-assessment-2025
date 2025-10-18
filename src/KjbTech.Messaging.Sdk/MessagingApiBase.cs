namespace KjbTech.Messaging.Sdk;

public abstract class MessagingApiBase
{
    protected readonly HttpClient _httpClient;

    protected MessagingApiBase(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    protected async Task<HttpResponseMessage> ProcessRequestAsync(HttpRequestMessage request)
    {
        var response = await _httpClient.SendAsync(request);

        if (response.IsSuccessStatusCode)
        {
            return response;
        }

        throw new MessagingException(
            $"Messaging API was not successful for reasons : " +
            $"Code: {response.StatusCode} " +
            $"Content: '{await response.Content.ReadAsStringAsync()}'."
            );
    }
}
