using System.Text.Json.Serialization;

namespace KjbTech.Messaging.Sdk.Contacts;

public sealed class ContactList : PagedList
{
    [JsonPropertyName("contacts")]
    public List<ContactDetails> Items { get; set; } = [];
}
