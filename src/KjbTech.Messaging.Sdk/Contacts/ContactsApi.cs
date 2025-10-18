using Microsoft.Extensions.Logging;
using System.Net.Http.Json;

namespace KjbTech.Messaging.Sdk.Contacts;

public sealed class ContactsApi : MessagingApiBase
{
    public ContactsApi(
        HttpClient httpClient,
        ILogger logger)
        : base(logger, httpClient)
    { }

    public async Task<ContactDetails?> GetAsync(ContactId contactId)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"contacts/{contactId.Value}");

        var response = await ProcessRequestAsync(request);

        var detailedContact = await response.Content.ReadFromJsonAsync<ContactDetails>();

        return detailedContact;
    }
}
