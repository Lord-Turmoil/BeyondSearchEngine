// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Search.Services;
using Beyond.SearchEngine.Modules.Utils;
using Microsoft.AspNetCore.Mvc;
using Tonisoft.AspExtensions.Module;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Controllers;

[ApiController]
[Route("v1/search/query/sources")]
public class SourceQueryController : BaseController<SourceQueryController>
{
    private readonly ISourceQueryService _service;

    public SourceQueryController(ILogger<SourceQueryController> logger, ISourceQueryService service)
        : base(logger)
    {
        _service = service;
    }

    [HttpGet]
    [Route("")]
    public async Task<ApiResponse> GetAll(
        [FromQuery(Name = "ps")] int pageSize = Globals.DefaultPageSize,
        [FromQuery(Name = "p")] int page = Globals.DefaultPage)
    {
        if (PaginationValidator.IsInvalid(pageSize, page))
        {
            return new BadRequestResponse(new InvalidPaginationDto());
        }

        try
        {
            return await _service.GetAll(pageSize, page);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting all sources");
            return new InternalServerErrorResponse(new InternalServerErrorDto(ex.Message));
        }
    }

    [HttpGet]
    [Route("host")]
    public async Task<ApiResponse> GetHost([FromQuery] string id, [FromQuery] bool brief)
    {
        try
        {
            return await _service.GetHost(id, brief);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting host of source {id}", id);
            return new InternalServerErrorResponse(new InternalServerErrorDto(ex.Message));
        }
    }

    [HttpGet]
    [Route("random")]
    public async Task<ApiResponse> GetRandomHot(
        [FromQuery] bool brief = true,
        [FromQuery(Name = "ps")] int pageSize = 3)
    {
        if (PaginationValidator.IsInvalid(pageSize, 100))
        {
            return new BadRequestResponse(new InvalidPaginationDto());
        }

        try
        {
            return await _service.GetRandomHot(brief, pageSize);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting random sources");
            return new InternalServerErrorResponse(new InternalServerErrorDto(ex.Message));
        }
    }

    [HttpGet]
    [Route("top")]
    public async Task<ApiResponse> GetTopSourceStatisticsByWorksCount(
        [FromQuery(Name = "ps")] int pageSize = Globals.DefaultPageSize,
        [FromQuery(Name = "p")] int page = Globals.DefaultPage)
    {
        if (PaginationValidator.IsInvalid(pageSize, page))
        {
            return new BadRequestResponse(new InvalidPaginationDto());
        }

        try
        {
            return await _service.GetTopSourceStatisticsByWorksCount(pageSize, page);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting top sources");
            return new InternalServerErrorResponse(new InternalServerErrorDto(ex.Message));
        }
    }

    [HttpGet]
    [Route("basic")]
    public async Task<ApiResponse> SearchBasic(
        [FromQuery] string query,
        [FromQuery(Name = "ps")] int pageSize = Globals.DefaultPageSize,
        [FromQuery(Name = "p")] int page = Globals.DefaultPage)
    {
        if (PaginationValidator.IsInvalid(pageSize, page))
        {
            return new BadRequestResponse(new InvalidPaginationDto());
        }

        try
        {
            return await _service.SearchSource(query, pageSize, page);
        }
        catch (Exception e)
        {
            _logger.LogError(e, "Error while searching sources with {query}", query);
            return new InternalServerErrorResponse(new InternalServerErrorDto(e.Message));
        }
    }
}