﻿using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Services;

public interface IConceptSearchService
{
    static readonly string Index = "concepts";

    Task<ApiResponse> GetAllWithPrefix(string prefix, int pageSize, int page);
}