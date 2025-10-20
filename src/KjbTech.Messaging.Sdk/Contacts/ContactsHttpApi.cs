using KjbTech.Messaging.Sdk.Errors;
using KjbTech.Messaging.Sdk.Exceptions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace KjbTech.Messaging.Sdk.Contacts;

/// <summary>
/// Contacts HTTP Api implementation.
/// </summary>
public sealed class ContactsHttpApi : MessagingHttpApiBase
{
    public ContactsHttpApi(HttpClient httpClient)
        : base(httpClient)
    { }

    public async Task<Result<ExistingContact>> GetAsync(ContactId contactId, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"contacts/{contactId.Value}");

        var response = await ProcessRequestAsync<ExistingContact>(request, cancellationToken);
        if (response.IsSuccess)
        {
            return response.WhenSuccess;
        }

        var errorWhen500Or401 = await response.WhenError.Content.ReadFromJsonAsync<DefaultError>(cancellationToken);
        if (errorWhen500Or401 is null)
        {
            throw new MessagingException("It seems that the error format got from the API has changed.");
        }

        return new Error(errorWhen500Or401.Message);
    }

    public async Task<Result<ContactList>> ListAsync(PaginationParameter paginationParameter, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"contacts?pageIndex={paginationParameter.PageNumber}&max={paginationParameter.PageSize}");

        var response = await ProcessRequestAsync<ContactList>(request, cancellationToken);
        if (response.IsSuccess)
        {
            return response.WhenSuccess;
        }

        var errorWhen500Or401 = await response.WhenError.Content.ReadFromJsonAsync<DefaultError>(cancellationToken);
        if (errorWhen500Or401 is null)
        {
            throw new MessagingException("It seems that the error format got from the API has changed.");
        }

        return new Error(errorWhen500Or401.Message);
    }

    public async Task<Result<ExistingContact>> CreateAsync(ContactToCreate contactToCreate, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    $"contacts")
        {
            Content = JsonContent.Create(contactToCreate, MediaTypeHeaderValue.Parse("application/json"))
        };

        var response = await ProcessRequestAsync<ExistingContact>(request, cancellationToken);
        if (response.IsSuccess)
        {
            return response.WhenSuccess;
        }

        if (response.WhenError.StatusCode == HttpStatusCode.BadRequest)
        {
            var errorWhenContactCreation = await response.WhenError.Content.ReadFromJsonAsync<ErrorWhenTryingCreation>(cancellationToken);
            if (errorWhenContactCreation is null)
            {
                throw new MessagingException("It seems that the error format got from the API has changed.");
            }

            return new Error(errorWhenContactCreation.Message);
        }
        else
        {
            var errorWhen500Or401 = await response.WhenError.Content.ReadFromJsonAsync<DefaultError>(cancellationToken);
            if (errorWhen500Or401 is null)
            {
                throw new MessagingException("It seems that the error format got from the API has changed.");
            }

            return new Error(errorWhen500Or401.Message);
        }
    }

    public async Task<Result<bool>> DeleteAsync(ContactId contactId, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(
                    HttpMethod.Delete,
                    $"contacts/{contactId.Value}");

        var response = await ProcessRequestAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return true;
        }

        var defaultError = await response.Content.ReadFromJsonAsync<DefaultError>(cancellationToken);
        if (defaultError is null)
        {
            throw new MessagingException("It seems that the error format got from the API has changed.");
        }

        return new Error(defaultError.Message);
    }
}
