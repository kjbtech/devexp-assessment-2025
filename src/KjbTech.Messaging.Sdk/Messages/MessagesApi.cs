using KjbTech.Messaging.Sdk.Contacts;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;

namespace KjbTech.Messaging.Sdk.Messages;

public sealed class MessagesApi : MessagingApiBase
{
    public MessagesApi(HttpClient httpClient)
        : base(httpClient)
    { }

    public async Task<EnqueuedMessageToSend?> GetAsync(MessageId messageId)
    {
        var request = new HttpRequestMessage(
            HttpMethod.Get,
            $"messages/{messageId.Value}");

        var response = await ProcessRequestAsync(request);

        var detailedContact = await response.Content.ReadFromJsonAsync<EnqueuedMessageToSend>();

        return detailedContact;
    }

    public async Task<Result<EnqueuedMessageToSend?>> EnqueueToSendAsync(MessageToSendToARegisteredContact messageToSendToARegisteredContact)
    {
        var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    $"messages")
        {
            Content = JsonContent.Create(messageToSendToARegisteredContact, MediaTypeHeaderValue.Parse("application/json"))
        };

        var response = await ProcessRequestAsync(request);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<EnqueuedMessageToSend>();
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

    public async Task<Result<EnqueuedMessageToSend?>> EnqueueToSendAsync(MessageToSendToANumber messageToSendToANumber)
    {
        var request = new HttpRequestMessage(
                    HttpMethod.Post,
                    $"messages")
        {
            Content = JsonContent.Create(messageToSendToANumber, MediaTypeHeaderValue.Parse("application/json"))
        };

        var response = await ProcessRequestAsync(request);

        if (response.IsSuccessStatusCode)
        {
            return await response.Content.ReadFromJsonAsync<EnqueuedMessageToSend>();
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
}
