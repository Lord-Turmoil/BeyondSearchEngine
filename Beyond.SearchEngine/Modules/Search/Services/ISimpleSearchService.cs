﻿// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Services;

/// <summary>
///     Simple search is for searching items with ID.
/// </summary>
public interface ISimpleSearchService
{
    public Task<ApiResponse> SearchSingle(string type, string id);
    public Task<ApiResponse> SearchMany(string type, IReadOnlyCollection<string> ids);
    public Task<ApiResponse> Preview(string type, string query, int pageSize, int page);
    public Task<ApiResponse> Search(string type, string query, int pageSize, int page);
}