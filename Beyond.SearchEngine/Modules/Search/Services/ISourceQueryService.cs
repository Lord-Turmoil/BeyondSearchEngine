// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Services;

public interface ISourceQueryService
{
    Task<ApiResponse> GetAll(int pageSize, int page);

    Task<ApiResponse> GetHost(string id);

    Task<ApiResponse> GetRandom(bool brief, int pageSize);

    Task<ApiResponse> GetTopSourceStatisticsByWorksCount(int pageSize, int page);
}