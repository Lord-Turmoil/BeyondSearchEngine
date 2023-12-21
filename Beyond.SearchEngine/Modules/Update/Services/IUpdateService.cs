// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Beyond.SearchEngine.Modules.Update.Dtos;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Update.Services;

public interface IUpdateService
{
    Task<ApiResponse> InitiateUpdate(string type, InitiateUpdateDto dto);

    Task<ApiResponse> ConfigureUpdate(ConfigureUpdateDto dto);

    ApiResponse QueryUpdateStatus(string type);
}