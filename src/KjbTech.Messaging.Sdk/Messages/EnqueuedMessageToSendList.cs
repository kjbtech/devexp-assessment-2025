using KjbTech.Messaging.Sdk.Contacts;
using System.Text.Json.Serialization;

namespace KjbTech.Messaging.Sdk.Messages;

/// <summary>
/// List of existing messages that have been enqueued.
/// </summary>
public sealed class EnqueuedMessageToSendList
{
    /// <summary>
    /// <see cref="EnqueuedMessageToSend"/>
    /// </summary>
    [JsonPropertyName("messages")]
    public List<EnqueuedMessageToSend> Items { get; set; } = [];

    /// <summary>
    /// Additionnal data related to the sens messages.
    /// </summary>
    [JsonPropertyName("data")]
    public AdditionnalData AdditionnalData { get; set; } = new AdditionnalData([]);

    [JsonPropertyName("page")]
    public int PageNumber { get; set; }

    [JsonPropertyName("quantityPerPage")]
    public int PageSize { get; set; }
}

[JsonConverter(typeof(AdditionnalDataConverter))]
public sealed record AdditionnalData(Dictionary<string, ContactDetails> Contacts);
