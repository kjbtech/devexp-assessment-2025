namespace KjbTech.Messaging.Sdk;

public abstract class MessagingHttpApiBase
{
    protected readonly HttpClient _httpClient;

    protected MessagingHttpApiBase(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    protected async Task<HttpResponseMessage> ProcessRequestAsync(HttpRequestMessage request)
    {
        var response = await _httpClient.SendAsync(request);

        return response;
    }
}
