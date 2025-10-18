namespace KjbTech.Messaging.Sdk;

public sealed class MessagingEndpoint
{
    /// <summary>
    /// The API key used for the authentication.
    /// </summary>
    public string ApiKey { get; set; } = null!;

    /// <summary>
    /// The base URL of the API.
    /// </summary>
    /// <remarks>Must not contain a relative path!</remarks>
    public string ApiBaseUrl { get; set; } = null!;
}
