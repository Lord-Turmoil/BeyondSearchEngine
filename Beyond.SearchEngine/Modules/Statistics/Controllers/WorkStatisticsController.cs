// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Beyond.SearchEngine.Modules.Statistics.Services;
using Microsoft.AspNetCore.Mvc;
using Tonisoft.AspExtensions.Module;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Statistics.Controllers;

[ApiController]
[Route("v1/search/statistics/works")]
public class WorkStatisticsController : BaseController<WorkStatisticsController>
{
    private readonly IWorkStatisticsService _service;

    public WorkStatisticsController(ILogger<WorkStatisticsController> logger, IWorkStatisticsService service) :
        base(logger)
    {
        _service = service;
    }

    [HttpGet]
    [Route("")]
    public async Task<ApiResponse> GetWorkStatistics([FromQuery] string id)
    {
        try
        {
            return await _service.GetStatistics(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting work statistics of {id}", id);
            return new InternalServerErrorResponse(new InternalServerErrorDto());
        }
    }

    [HttpPost]
    [Route("like")]
    public async Task<ApiResponse> LikeWork([FromQuery] int userId, [FromQuery] string workId)
    {
        try
        {
            return await _service.LikeWork(userId, workId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while {userId} liking work {workId}", userId, workId);
            return new InternalServerErrorResponse(new InternalServerErrorDto());
        }
    }

    [HttpPost]
    [Route("unlike")]
    public async Task<ApiResponse> UnlikeWork([FromQuery] int userId, [FromQuery] string workId)
    {
        try
        {
            return await _service.UnLikeWork(userId, workId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while {userId} unliking work {workId}", userId, workId);
            return new InternalServerErrorResponse(new InternalServerErrorDto());
        }
    }

    [HttpGet]
    [Route("islike")]
    public Task<ApiResponse> IsUserLikedWork([FromQuery] int userId, [FromQuery] string workId)
    {
        try
        {
            return _service.IsLiked(userId, workId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while checking whether {userId} liked work {workId}", userId, workId);
            return Task.FromResult<ApiResponse>(new InternalServerErrorResponse(new InternalServerErrorDto()));
        }
    }

    [HttpPost]
    [Route("view")]
    public async Task<ApiResponse> ViewWork([FromQuery] string id)
    {
        try
        {
            return await _service.ViewWork(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while viewing work {id}", id);
            return new InternalServerErrorResponse(new InternalServerErrorDto());
        }
    }
}