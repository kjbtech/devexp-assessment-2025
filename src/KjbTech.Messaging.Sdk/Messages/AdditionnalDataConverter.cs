using KjbTech.Messaging.Sdk.Contacts;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace KjbTech.Messaging.Sdk.Messages;

internal sealed class AdditionnalDataConverter : JsonConverter<AdditionnalData?>
{
    public override AdditionnalData? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        if (reader.TokenType == JsonTokenType.Null) return null;

        using var doc = JsonDocument.ParseValue(ref reader);
        var root = doc.RootElement;

        var contactsObj = root.TryGetProperty("contacts", out var c) && c.ValueKind == JsonValueKind.Object
            ? c
            : (root.ValueKind == JsonValueKind.Object ? root : default);

        var dict = new Dictionary<string, ContactDetails>(StringComparer.OrdinalIgnoreCase);

        if (contactsObj.ValueKind == JsonValueKind.Object)
        {
            foreach (var kv in contactsObj.EnumerateObject())
            {
                var obj = kv.Value;
                var name = obj.TryGetProperty("name", out var n) && n.ValueKind == JsonValueKind.String ? n.GetString() : null;
                var phone = obj.TryGetProperty("phone", out var p) && p.ValueKind == JsonValueKind.String ? p.GetString() : null;

                dict[kv.Name] = new ContactDetails { Name = name ?? string.Empty, Phone = phone ?? string.Empty };
            }
        }

        return new AdditionnalData(dict);
    }

    public override void Write(Utf8JsonWriter writer, AdditionnalData? value, JsonSerializerOptions options)
    {
        throw new NotImplementedException();
    }
}
