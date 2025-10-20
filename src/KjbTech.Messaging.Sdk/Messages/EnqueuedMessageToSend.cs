using System.Text.Json.Serialization;

namespace KjbTech.Messaging.Sdk.Messages;

public class EnqueuedMessageToSend
{
    /// <summary>
    /// Unique ID of the message.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; } = null!;

    /// <summary>
    /// The unique ID of the contact.
    /// </summary>
    [JsonPropertyName("to")]
    public required string To { get; set; } = null!;

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

    /// <summary>
    /// Initial status of the message.
    /// </summary>
    [JsonPropertyName("status")]
    public MessageStatus Status { get; set; }

    /// <summary>
    /// Time when the message was created.
    /// </summary>
    [JsonPropertyName("createdAt")]
    public required DateTime CreatedAt { get; set; }

    /// <summary>
    /// Time when the message was delivered.
    /// Could be null if the message is still in the queue.
    /// </summary>
    [JsonPropertyName("deliveredAt")]
    public DateTime? DeliveredAt { get; set; }
}

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum MessageStatus
{
    Queued,
    Sent,
    Delivered,
    Failed
}
