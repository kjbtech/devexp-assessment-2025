using System.Text.Json.Serialization;

namespace KjbTech.Messaging.Sdk.Errors;

/// <summary>
/// When 401, 404, 500...
/// </summary>
internal sealed class DefaultError
{
    /// <summary>
    /// The description of the error.
    /// </summary>
    [JsonPropertyName("message")]
    public string Message { get; set; } = null!;
}
