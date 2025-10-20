using System.Text.Json.Serialization;

namespace KjbTech.Messaging.Sdk.Contacts;

/// <summary>
/// Existing contact.
/// </summary>
public sealed class ExistingContact : ContactDetails
{
    /// <summary>
    /// The unique ID of the contact.
    /// </summary>
    [JsonPropertyName("id")]
    public string Id { get; set; } = null!;
}
