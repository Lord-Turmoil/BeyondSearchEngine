// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Statistics.Services;

public interface IWorkStatisticsService
{
    Task<ApiResponse> GetStatistics(string id);

    Task<ApiResponse> LikeWork(string id);
    Task<ApiResponse> UnLikeWork(string id);

    Task<ApiResponse> QuestionWork(string id);
    Task<ApiResponse> UnQuestionWork(string id);

    Task<ApiResponse> ViewWork(string id);
}