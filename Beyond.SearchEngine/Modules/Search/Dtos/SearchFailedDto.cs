using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Dtos;

public class SearchFailedDto : InternalServerErrorDto
{
    public SearchFailedDto(string message = "Search failed") : base(message)
    {
    }
}