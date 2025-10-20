using System.Text.Json.Serialization;

namespace KjbTech.Messaging.Sdk.Contacts;

/// <summary>
/// 400 error.
/// </summary>
internal sealed class ErrorWhenTryingCreation
{
    /// <summary>
    /// The description of the error.
    /// </summary>
    [JsonPropertyName("error")]
    public string Message { get; set; } = null!;
}
