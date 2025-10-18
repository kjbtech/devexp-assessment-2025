using System.Diagnostics.CodeAnalysis;

namespace KjbTech.Messaging.Sdk;

public sealed class Result<TValueWhenSuccess>
{
    public TValueWhenSuccess? Value { get; }
    public Error? Error { get; }

    [MemberNotNullWhen(true, nameof(Value))]
    [MemberNotNullWhen(false, nameof(Error))]
    public bool IsSuccess => Value is not null;

    [MemberNotNullWhen(false, nameof(Value))]
    [MemberNotNullWhen(true, nameof(Error))]
    public bool HasFailed => Error is not null;

    private Result(Error error)
    { Error = error; }

    private Result(TValueWhenSuccess value)
    { Value = value; }

    public static Result<TValueWhenSuccess> Success(TValueWhenSuccess value) => new(value);

    public static Result<TValueWhenSuccess> Failure(Error error) => new(error);

    public static implicit operator Result<TValueWhenSuccess>(TValueWhenSuccess value) => new(value);

    public static implicit operator Result<TValueWhenSuccess>(Error error) => new(error);
}

public record Error(string Message)
{ }
