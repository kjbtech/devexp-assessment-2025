using System.Text.Json.Serialization;

namespace KjbTech.Messaging.Sdk.Contacts;

public sealed class ErrorWhenContactCreation
{
    /// <summary>
    /// The description of the error.
    /// </summary>
    [JsonPropertyName("error")]
    public string Error { get; set; } = null!;
}
