namespace KjbTech.Messaging.Sdk.Contacts;

/// <summary>
/// A contact identifier.
/// </summary>
/// <param name="contactId"></param>
public struct ContactId(string contactId)
{
    /// <summary>
    /// The value of the identifier.
    /// </summary>
    public string Value { get; private set; } = contactId;
}
