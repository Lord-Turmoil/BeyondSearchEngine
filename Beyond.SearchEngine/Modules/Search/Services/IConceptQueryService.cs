// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Services;

public interface IConceptQueryService
{
    static readonly string Index = "concepts";

    Task<ApiResponse> GetAllWithPrefix(string prefix, int pageSize, int page);

    Task<ApiResponse> GetTopConcepts(int pageSize, int page);

    Task<ApiResponse> GetRandomHot(bool brief, int pageSize);
}