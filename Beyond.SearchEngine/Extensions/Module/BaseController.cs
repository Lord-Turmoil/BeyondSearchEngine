// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.
// Licensed under the BSD 2-Clause License.

using Microsoft.AspNetCore.Mvc;
using Tonisoft.AspExtensions.Response;

namespace Tonisoft.AspExtensions.Module;

public class BaseController<TController> : Controller where TController : Controller
{
    protected readonly ILogger<TController> _logger;


    public BaseController(ILogger<TController> logger)
    {
        _logger = logger;
    }

    protected async Task<ApiResponse> ResponseWrapper(Task<ApiResponse> task)
    {
        try
        {
            return await task;
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error occurred when processing request.");
            return new InternalServerErrorResponse(new InternalServerErrorDto(e.Message));
        }
    }
}