using Beyond.SearchEngine.Modules.Utils;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Dtos;

public class PagedRequestDto : ApiRequestDto
{
    public int PageSize { get; set; } = Globals.DefaultPageSize;
    public int Page { get; set; } = Globals.DefaultPage;

    public override bool Verify()
    {
        return !PaginationValidator.IsInvalid(PageSize, Page);
    }
}