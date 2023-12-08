﻿using Beyond.SearchEngine.Modules.Update.Dtos;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Update.Services;

public interface IUpdateService
{
    Task<ApiResponse> InitiateUpdate(string type, InitiateUpdateDto dto);

    ApiResponse QueryUpdateStatus(string type);
}