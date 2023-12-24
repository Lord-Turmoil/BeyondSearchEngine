// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Services;

public interface IAuthorQueryService
{
    Task<ApiResponse> GetWorks(string id, bool brief, int pageSize, int page);

    Task<ApiResponse> GetInstitution(string id, bool brief);

    Task<ApiResponse> GetTopAuthors(int pageSize, int page);
}