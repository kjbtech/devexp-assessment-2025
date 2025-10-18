using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace KjbTech.Messaging.Sdk.Contacts;

public sealed class ContactsApi : MessagingApiBase
{
    public ContactsApi(HttpClient httpClient)
        : base(httpClient)
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

    public async Task<Result<ContactList>> ListAsync(PaginationParameter paginationParameter)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"contacts?pageIndex={paginationParameter.PageNumber}&max={paginationParameter.PageSize}");

        var response = await ProcessRequestAsync(request);

        if (response.IsSuccessStatusCode)
        {
            var contactList = await response.Content.ReadFromJsonAsync<ContactList>();
            if (contactList is null)
            {
                throw new MessagingException("It seems that the error format got from the API has changed.");
            }
            return contactList;
        }
        else
        {
            var errorWhen500Or401 = await response.Content.ReadFromJsonAsync<DefaultError>();
            if (errorWhen500Or401 is null)
            {
                throw new MessagingException("It seems that the error format got from the API has changed.");
            }

            return new Error(errorWhen500Or401.Message);
        }
    }

    public async Task<Result<ContactDetails?>> CreateAsync(ContactToCreate contactToCreate)
    {
        var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    $"contacts")
        {
            Content = JsonContent.Create(contactToCreate, MediaTypeHeaderValue.Parse("application/json"))
        };

        var response = await ProcessRequestAsync(request);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<ContactDetails>();
        }
        else
        {
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorWhenContactCreation = await response.Content.ReadFromJsonAsync<ErrorWhenContactCreation>();
                if (errorWhenContactCreation is null)
                {
                    throw new MessagingException("It seems that the error format got from the API has changed.");
                }

                return new Error(errorWhenContactCreation.Error);
            }
            else
            {
                var errorWhen500Or401 = await response.Content.ReadFromJsonAsync<DefaultError>();
                if (errorWhen500Or401 is null)
                {
                    throw new MessagingException("It seems that the error format got from the API has changed.");
                }

                return new Error(errorWhen500Or401.Message);
            }
        }
    }

    public async Task<Result<bool>> DeleteAsync(ContactId contactId)
    {
        var request = new HttpRequestMessage(
                    HttpMethod.Delete,
                    $"contacts/{contactId.Value}");

        var response = await ProcessRequestAsync(request);

        if (response.IsSuccessStatusCode)
        {
            return true;
        }
        else
        {
            var defaultError = await response.Content.ReadFromJsonAsync<DefaultError>();
            if (defaultError is null)
            {
                throw new MessagingException("It seems that the error format got from the API has changed.");
            }

            return new Error(defaultError.Message);
        }
    }
}
