using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Services;

/// <summary>
/// Simple search is for searching items with ID.
/// </summary>
public interface ISimpleSearchService
{
    public Task<ApiResponse> SearchSingleAsync(string type, string id);
    public Task<ApiResponse> SearchManyAsync(string type, IEnumerable<string> ids);
}