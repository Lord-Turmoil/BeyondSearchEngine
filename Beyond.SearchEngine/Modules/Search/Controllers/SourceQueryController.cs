// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Search.Services;
using Beyond.SearchEngine.Modules.Utils;
using Microsoft.AspNetCore.Mvc;
using Tonisoft.AspExtensions.Module;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Controllers;

[ApiController]
[Route("api/search/query/sources")]
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
    public async Task<ApiResponse> GetHost([FromQuery] string id)
    {
        try
        {
            return await _service.GetHost(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting host of source {id}", id);
            return new InternalServerErrorResponse(new InternalServerErrorDto(ex.Message));
        }
    }
}