namespace KjbTech.Messaging.Sdk;

public sealed class MessagingException : Exception
{
    public MessagingException()
    {
    }

    public MessagingException(string message) : base(message)
    {
    }

    public MessagingException(string message, Exception innerException) : base(message, innerException)
    {
    }
}
