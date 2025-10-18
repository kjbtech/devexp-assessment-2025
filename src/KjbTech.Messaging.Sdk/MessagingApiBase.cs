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

        return response;
    }
}
