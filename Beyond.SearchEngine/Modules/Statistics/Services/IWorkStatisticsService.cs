// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Statistics.Services;

public interface IWorkStatisticsService
{
    Task<ApiResponse> GetStatistics(string id);

    Task<ApiResponse> LikeWork(int userId, string workId);
    Task<ApiResponse> UnLikeWork(int userId, string workId);

    Task<ApiResponse> IsLiked(int userId, string workId);

    Task<ApiResponse> ViewWork(string id);
}