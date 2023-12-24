// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Services;

/// <summary>
///     Simple search is for searching items with ID.
/// </summary>
public interface ISimpleSearchService
{
    Task<ApiResponse> SearchSingle(string type, bool brief, string id);
    Task<ApiResponse> SearchMany(string type, bool brief, IReadOnlyCollection<string> idList);
    Task<ApiResponse> Preview(string type, string query, int pageSize, int page);
    Task<ApiResponse> Search(string type, string query, bool brief, int pageSize, int page);
}