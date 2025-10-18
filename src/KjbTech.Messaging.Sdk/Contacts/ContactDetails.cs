using System.Text.Json.Serialization;

namespace KjbTech.Messaging.Sdk.Contacts;

public sealed class ContactDetails
{
    /// <summary>
    /// The name of the contact.
    /// </summary>
    [JsonPropertyName("name")]
    public string Name { get; set; } = null!;

    /// <summary>
    /// The phone number of the contact.
    /// </summary>
    [JsonPropertyName("phone")]
    public string Phone { get; set; } = null!;

    /// <summary>
    /// The unique ID of the contact.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
}
