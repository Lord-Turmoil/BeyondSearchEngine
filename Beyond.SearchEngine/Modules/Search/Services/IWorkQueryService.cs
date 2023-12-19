using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Services;

public interface IWorkQueryService
{
    Task<ApiResponse> GetRelatedWorks(string id, bool brief);

    Task<ApiResponse> GetReferencedWorks(string id, bool brief);

}