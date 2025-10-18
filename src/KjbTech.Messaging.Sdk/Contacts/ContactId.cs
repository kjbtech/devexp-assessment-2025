namespace KjbTech.Messaging.Sdk.Contacts;

public struct ContactId(string contactId)
{
    public string Value { get; private set; } = contactId;
}
