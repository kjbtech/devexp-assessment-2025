using System.Text.Json.Serialization;

namespace KjbTech.Messaging.Sdk.Contacts;

/// <summary>
/// List of existing contacts.
/// </summary>
public sealed class ContactList
{
    /// <summary>
    /// <see cref="ExistingContact"/>
    /// </summary>
    [JsonPropertyName("contacts")]
    public List<ExistingContact> Items { get; set; } = [];

    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }
}
