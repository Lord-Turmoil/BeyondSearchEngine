﻿using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Deprecated.Services;

public interface IDeprecatedService
{
    /// <summary>
    ///     This will return author's institution as string.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="brief"></param>
    /// <returns></returns>
    Task<ApiResponse> GetAuthorById(string id, bool brief);

    /// <summary>
    ///     This will return author's works.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="brief"></param>
    /// <param name="pageSize"></param>
    /// <param name="page"></param>
    /// <returns></returns>
    Task<ApiResponse> GetWorksOfAuthor(string id, bool brief, int pageSize, int page);
}