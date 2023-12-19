using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Dtos;

public class InvalidPaginationDto : BadRequestDto
{
    public InvalidPaginationDto(string message = "Invalid pagination") : base(message)
    {
    }
}