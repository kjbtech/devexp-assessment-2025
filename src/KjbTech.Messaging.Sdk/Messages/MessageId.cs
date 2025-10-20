namespace KjbTech.Messaging.Sdk.Messages;

public struct MessageId(string messageId)
{
    public string Value { get; private set; } = messageId;
}
