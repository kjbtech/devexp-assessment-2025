using System.Text.Json.Serialization;

namespace KjbTech.Messaging.Sdk.Messages;

public abstract class MessageToSend
{
    /// <summary>
    /// The sender's phone number.
    /// </summary>
    [JsonPropertyName("from")]
    public required string From { get; set; } = null!;

    /// <summary>
    /// The text content of the message.
    /// </summary>
    [JsonPropertyName("content")]
    public required string Content { get; set; } = null!;
}
