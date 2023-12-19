using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Services;

/// <summary>
///     Simple search is for searching items with ID.
/// </summary>
public interface ISimpleSearchService
{
    public Task<ApiResponse> SearchSingle(string type, string id);
    public Task<ApiResponse> SearchMany(string type, IEnumerable<string> ids);
    public Task<ApiResponse> Preview(string type, string query, int pageSize, int page);
}