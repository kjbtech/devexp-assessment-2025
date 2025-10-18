using System.Text.Json.Serialization;

namespace KjbTech.Messaging.Sdk;

public class PagedList
{
    [JsonPropertyName("pageNumber")]
    public int PageNumber { get; set; }

    [JsonPropertyName("pageSize")]
    public int PageSize { get; set; }
}
