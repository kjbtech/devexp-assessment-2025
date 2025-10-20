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

    public async Task<Result<EnqueuedMessageToSend?>> GetAsync(MessageId messageId, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{ApiRoute}/{messageId.Value}");

        var response = await ProcessRequestAsync<EnqueuedMessageToSend>(request, cancellationToken);
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

    public async Task<Result<EnqueuedMessageToSendList>> ListEnqueuedMessageToSendAsync(MessagesPaginationParameter paginationParameter, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"{ApiRoute}?page={paginationParameter.PageNumber}&limit={paginationParameter.PageSize}");

        var response = await ProcessRequestAsync<EnqueuedMessageToSendList>(request, cancellationToken);
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

    public async Task<Result<EnqueuedMessageToSend?>> EnqueueToSendAsync(MessageToSendToARegisteredContact messageToSendToARegisteredContact, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    $"{ApiRoute}")
        {
            Content = JsonContent.Create(messageToSendToARegisteredContact, MediaTypeHeaderValue.Parse("application/json"))
        };

        var response = await ProcessRequestAsync<EnqueuedMessageToSend>(request, cancellationToken);
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

    public async Task<Result<EnqueuedMessageToSend?>> EnqueueToSendAsync(MessageToSendToANumber messageToSendToANumber, CancellationToken cancellationToken = default)
    {
        using var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    $"{ApiRoute}")
        {
            Content = JsonContent.Create(messageToSendToANumber, MediaTypeHeaderValue.Parse("application/json"))
        };

        var response = await ProcessRequestAsync<EnqueuedMessageToSend>(request, cancellationToken);
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
}
