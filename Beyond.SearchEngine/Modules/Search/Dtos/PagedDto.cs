namespace Beyond.SearchEngine.Modules.Search.Dtos;

/// <summary>
///     Wrapper for paged response dto.
/// </summary>
public class PagedDto
{
    public PagedDto(long total, int pageSize, int page, IEnumerable<object> data)
    {
        Total = total;
        PageSize = pageSize;
        Page = page;
        Data = data;
    }

    public long Total { get; set; }
    public int PageSize { get; set; }
    public int Page { get; set; }
    public bool HasNext => Page * PageSize < Total;

    public IEnumerable<object> Data { get; set; }
}