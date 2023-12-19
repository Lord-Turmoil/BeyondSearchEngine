using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Services;

public interface ISourceQueryService
{
    Task<ApiResponse> GetAll(int pageSize, int page);

    Task<ApiResponse> GetHost(string id);
}