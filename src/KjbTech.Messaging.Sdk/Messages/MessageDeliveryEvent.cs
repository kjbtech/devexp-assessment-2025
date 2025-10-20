using KjbTech.Messaging.Sdk.Messages;
using System.Text.Json.Serialization;

namespace KjbTech.Messaging.Sdk.Messages;

public sealed class MessageDeliveryEvent
{
    /// <summary>
    /// Unique ID of the message.
    /// </summary>
    [JsonPropertyName("id")]
    public required string Id { get; set; } = null!;

    /// <summary>
    /// The delivery status of the message.
    /// </summary>
    [JsonPropertyName("status")]
    public required MessageStatus Status { get; set; }

    /// <summary>
    /// Time of delivery, if delivered.
    /// </summary>
    [JsonPropertyName("deliveredAt")]
    public DateTimeOffset? DeliveredAt { get; set; }

    /// <summary>
    /// Reason for failure, if applicable.
    /// </summary>
    [JsonPropertyName("failureReason")]
    public string? FailureReason { get; set; }
}
