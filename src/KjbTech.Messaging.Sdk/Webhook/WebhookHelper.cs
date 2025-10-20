using KjbTech.Messaging.Sdk.Messages;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace KjbTech.Messaging.Sdk.Webhook;

public static class WebhookHelper
{
    public const string Scheme = "Signature";

    private static readonly JsonSerializerOptions _defaultJsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        Converters = { new System.Text.Json.Serialization.JsonStringEnumConverter(JsonNamingPolicy.CamelCase) }
    };

    /// <summary>
    /// Verifies the Authorization signature and deserializes the JSON body
    /// into a <see cref="TEvent"/> instance.
    /// </summary>
    /// <param name="body">The body of the incoming request.</param>
    /// <param name="signature">The signature sent.</param>
    /// <param name="secret">The webhook signing secret.</param>
    /// <param name="event">The parsed event if validation succeeds.</param>
    /// <returns>True if signature and JSON are valid; otherwise false.</returns>
    public static bool TryParse<TEvent>(string body, string secret, string signature, [NotNullWhen(true)] out TEvent? @event)
    {
        @event = default;

        if (!Verify(body, secret, signature))
            return false;

        try
        {
            @event = JsonSerializer.Deserialize<TEvent>(body, _defaultJsonSerializerOptions);
            return @event != null;
        }
        catch (JsonException)
        {
            return false;
        }
    }

    public static bool Verify(string rawBody, string secret, string authorizationHeader)
    {
        var parts = authorizationHeader.Split(' ', 2, StringSplitOptions.RemoveEmptyEntries);
        if (parts.Length != 2 || !parts[0].Equals(Scheme, StringComparison.OrdinalIgnoreCase))
            return false;

        var provided = parts[1].Trim();
        if (provided.Length == 0) return false;

        var computed = ComputeHex(rawBody, secret);
        return FixedTimeEqualsHex(provided, computed);
    }

    public static string ComputeHex(string rawBody, string secret)
    {
        var key = Encoding.UTF8.GetBytes(secret);
        var data = Encoding.UTF8.GetBytes(rawBody);
        using var hmac = new HMACSHA256(key);
        var hash = hmac.ComputeHash(data);
        return Convert.ToHexString(hash).ToLowerInvariant();
    }

    private static bool FixedTimeEqualsHex(string a, string b)
    {
        if (a.Length != b.Length) return false;

        var result = 0;
        for (int i = 0; i < a.Length; i++)
        {
            result |= a[i] ^ b[i];
        }
        return result == 0;
    }
}
