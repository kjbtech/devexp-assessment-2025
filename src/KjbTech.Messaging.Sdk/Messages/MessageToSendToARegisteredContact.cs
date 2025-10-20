using KjbTech.Messaging.Sdk.Contacts;
using System.Text.Json.Serialization;

namespace KjbTech.Messaging.Sdk.Messages;

public sealed class MessageToSendToARegisteredContact : MessageToSend
{
    [JsonPropertyName("to")]
    public required ExistingContactForAMessageToSend To { get; set; } = null!;
}

public sealed class ExistingContactForAMessageToSend
{
    /// <summary>
    /// The id of a registered contact.
    /// </summary>
    [JsonPropertyName("id")]
    public required string ContactId { get; set; } = null!;
}
