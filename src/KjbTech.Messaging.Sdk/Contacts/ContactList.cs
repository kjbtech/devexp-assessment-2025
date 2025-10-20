using System.Text.Json.Serialization;

namespace KjbTech.Messaging.Sdk.Contacts;

/// <summary>
/// List of existing contacts.
/// </summary>
public sealed class ContactList : PagedList
{
    /// <summary>
    /// <see cref="ExistingContact"/>
    /// </summary>
    [JsonPropertyName("contacts")]
    public List<ExistingContact> Items { get; set; } = [];
}
