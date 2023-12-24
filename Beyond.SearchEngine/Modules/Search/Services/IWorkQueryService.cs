// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Beyond.SearchEngine.Modules.Search.Dtos;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Services;

public interface IWorkQueryService
{
    Task<ApiResponse> GetRelatedWorks(string id, bool brief);

    Task<ApiResponse> GetReferencedWorks(string id, bool brief);

    Task<ApiResponse> QueryWorksBasic(QueryWorkBasicDto dto);

    Task<ApiResponse> QueryWorksAdvanced(QueryWorkAdvancedDto dto);

    Task<ApiResponse> GetCitations(string type, IReadOnlyCollection<string> idList);

    Task<ApiResponse> GetTopWorks(DateTime? begin, DateTime? end, int pageSize, int page);
}