namespace KjbTech.Messaging.Sdk;

public record PaginationParameter(int PageNumber = 1, int PageSize = 10)
{
    private const int DefaultMaxElementByPage = 10;

    public int PageSize { get; init; } =
        PageSize > DefaultMaxElementByPage ? DefaultMaxElementByPage : PageSize;
}
