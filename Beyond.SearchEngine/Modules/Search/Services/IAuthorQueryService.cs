using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Services;

public interface IAuthorQueryService
{
    Task<ApiResponse> GetWorks(string id, bool brief, int pageSize, int page);

    Task<ApiResponse> GetInstitution(string id, bool brief);
}