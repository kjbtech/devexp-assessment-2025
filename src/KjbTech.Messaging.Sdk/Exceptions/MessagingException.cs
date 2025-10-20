namespace KjbTech.Messaging.Sdk.Exceptions;

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
