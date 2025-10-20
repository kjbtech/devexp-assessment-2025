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

public sealed class Result<TValueWhenSuccess, TWhenError>
{
    public TValueWhenSuccess? WhenSuccess { get; }
    public TWhenError? WhenError { get; }

    [MemberNotNullWhen(true, nameof(WhenSuccess))]
    [MemberNotNullWhen(false, nameof(WhenError))]
    public bool IsSuccess => WhenSuccess is not null;

    [MemberNotNullWhen(false, nameof(WhenSuccess))]
    [MemberNotNullWhen(true, nameof(WhenError))]
    public bool HasFailed => WhenError is not null;

    private Result(TWhenError error)
    { WhenError = error; }

    private Result(TValueWhenSuccess value)
    { WhenSuccess = value; }

    public static Result<TValueWhenSuccess, TWhenError> Success(TValueWhenSuccess value) => new(value);

    public static Result<TValueWhenSuccess, TWhenError> Failure(TWhenError error) => new(error);

    public static implicit operator Result<TValueWhenSuccess, TWhenError>(TValueWhenSuccess value) => new(value);

    public static implicit operator Result<TValueWhenSuccess, TWhenError>(TWhenError error) => new(error);
}

public record Error(string Message)
{ }
