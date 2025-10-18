using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace KjbTech.Messaging.Sdk.Contacts;

public sealed class ContactToCreate
{
    /// <summary>
    /// The name of the contact.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; set; } = null!;

    /// <summary>
    /// The phone number of the contact.
    /// </summary>
    [JsonPropertyName("phone")]
    public required string Phone { get; set; } = null!;
}
