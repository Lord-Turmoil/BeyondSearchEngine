using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Deprecated.Services;

public interface IDeprecatedService
{
    /// <summary>
    /// This will return author's institution as string.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="brief"></param>
    /// <returns></returns>
    Task<ApiResponse> GetAuthorById(string id, bool brief);
}