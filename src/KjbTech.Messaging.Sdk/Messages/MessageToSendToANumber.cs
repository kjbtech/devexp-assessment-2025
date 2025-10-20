using System.Text.Json.Serialization;

namespace KjbTech.Messaging.Sdk.Messages;

public sealed class MessageToSendToANumber : MessageToSend
{
    public MessageToSendToANumber()
    { }

    public MessageToSendToANumber(string name, string phone)
    {
        To = new ContactForAMessageToSend() { Name = name, Phone = phone };
    }

    [JsonPropertyName("to")]
    public ContactForAMessageToSend To { get; set; } = null!;
}

public sealed class ContactForAMessageToSend
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
