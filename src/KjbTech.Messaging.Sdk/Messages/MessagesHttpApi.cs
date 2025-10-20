using KjbTech.Messaging.Sdk.Contacts;
using KjbTech.Messaging.Sdk.Errors;
using KjbTech.Messaging.Sdk.Exceptions;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace KjbTech.Messaging.Sdk.Messages;

/// <summary>
/// Messages HTTP Api implementation.
/// </summary>
public sealed class MessagesHttpApi : MessagingHttpApiBase
{
    private const string ApiRoute = "messages";

    public MessagesHttpApi(HttpClient httpClient)
        : base(httpClient)
    { }

    public async Task<EnqueuedMessageToSend?> GetAsync(MessageId messageId, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{ApiRoute}/{messageId.Value}");

        var response = await ProcessRequestAsync(request, cancellationToken);

        var detailedContact = await response.Content.ReadFromJsonAsync<EnqueuedMessageToSend>();

        return detailedContact;
    }

    public async Task<Result<EnqueuedMessageToSendList>> ListEnqueuedMessageToSendAsync(MessagesPaginationParameter paginationParameter, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{ApiRoute}?page={paginationParameter.PageNumber}&limit={paginationParameter.PageSize}");

        var response = await ProcessRequestAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            var enqueuedMessageToSendList = await response.Content.ReadFromJsonAsync<EnqueuedMessageToSendList>(cancellationToken);
            if (enqueuedMessageToSendList is null)
            {
                throw new MessagingException("It seems that the error format got from the API has changed.");
            }
            return enqueuedMessageToSendList;
        }
        else
        {
            var errorWhen500Or401 = await response.Content.ReadFromJsonAsync<DefaultError>(cancellationToken);
            if (errorWhen500Or401 is null)
            {
                throw new MessagingException("It seems that the error format got from the API has changed.");
            }

            return new Error(errorWhen500Or401.Message);
        }
    }

    public async Task<Result<EnqueuedMessageToSend?>> EnqueueToSendAsync(MessageToSendToARegisteredContact messageToSendToARegisteredContact, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    $"{ApiRoute}")
        {
            Content = JsonContent.Create(messageToSendToARegisteredContact, MediaTypeHeaderValue.Parse("application/json"))
        };

        var response = await ProcessRequestAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<EnqueuedMessageToSend>();
        }
        else
        {
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorWhenContactCreation = await response.Content.ReadFromJsonAsync<ErrorWhenContactCreation>(cancellationToken);
                if (errorWhenContactCreation is null)
                {
                    throw new MessagingException("It seems that the error format got from the API has changed.");
                }

                return new Error(errorWhenContactCreation.Error);
            }
            else
            {
                var errorWhen500Or401 = await response.Content.ReadFromJsonAsync<DefaultError>(cancellationToken);
                if (errorWhen500Or401 is null)
                {
                    throw new MessagingException("It seems that the error format got from the API has changed.");
                }

                return new Error(errorWhen500Or401.Message);
            }
        }
    }

    public async Task<Result<EnqueuedMessageToSend?>> EnqueueToSendAsync(MessageToSendToANumber messageToSendToANumber, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    $"{ApiRoute}")
        {
            Content = JsonContent.Create(messageToSendToANumber, MediaTypeHeaderValue.Parse("application/json"))
        };

        var response = await ProcessRequestAsync(request, cancellationToken);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<EnqueuedMessageToSend>(cancellationToken);
        }
        else
        {
            if (response.StatusCode == HttpStatusCode.BadRequest)
            {
                var errorWhenContactCreation = await response.Content.ReadFromJsonAsync<ErrorWhenContactCreation>(cancellationToken);
                if (errorWhenContactCreation is null)
                {
                    throw new MessagingException("It seems that the error format got from the API has changed.");
                }

                return new Error(errorWhenContactCreation.Error);
            }
            else
            {
                var errorWhen500Or401 = await response.Content.ReadFromJsonAsync<DefaultError>(cancellationToken);
                if (errorWhen500Or401 is null)
                {
                    throw new MessagingException("It seems that the error format got from the API has changed.");
                }

                return new Error(errorWhen500Or401.Message);
            }
        }
    }
}
