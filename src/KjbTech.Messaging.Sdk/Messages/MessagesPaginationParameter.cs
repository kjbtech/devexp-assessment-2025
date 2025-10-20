namespace KjbTech.Messaging.Sdk.Messages;

public record MessagesPaginationParameter(int PageNumber = 1, int PageSize = 100)
    : PaginationParameter(PageNumber, PageSize);
