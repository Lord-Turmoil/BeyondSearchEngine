// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Search.Services;
using Beyond.SearchEngine.Modules.Utils;
using Microsoft.AspNetCore.Mvc;
using Tonisoft.AspExtensions.Module;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Controllers;

[ApiController]
[Route("search")]
public class SimpleSearchController : BaseController<SimpleSearchController>
{
    private readonly ISimpleSearchService _service;

    public SimpleSearchController(ILogger<SimpleSearchController> logger, ISimpleSearchService service)
        : base(logger)
    {
        _service = service;
    }

    /// <summary>
    ///     Get a single document by id.
    /// </summary>
    /// <param name="type">Index type.</param>
    /// <param name="id">ID</param>
    /// <returns></returns>
    [HttpGet]
    [Route("single")]
    public async Task<ApiResponse> SearchSingle([FromQuery] string type, [FromQuery] string id)
    {
        // if (!Globals.AvailableTypes.Contains(type))
        // {
        //     return new BadRequestResponse(new BadRequestDto($"Invalid type {type}"));
        // }

        try
        {
            return await _service.SearchSingle(type, id);
        }
        catch (Exception ex)
        {
            return new InternalServerErrorResponse(new InternalServerErrorDto(ex.Message));
        }
    }

    /// <summary>
    ///     Get many documents by ids.
    /// </summary>
    /// <param name="type">Index type.</param>
    /// <param name="ids">ID list.</param>
    /// <returns></returns>
    [HttpGet]
    [Route("many")]
    public async Task<ApiResponse> SearchMany([FromQuery] string type, [FromQuery] IEnumerable<string> ids)
    {
        if (!Globals.AvailableTypes.Contains(type))
        {
            return new BadRequestResponse(new BadRequestDto($"Invalid type {type}"));
        }

        try
        {
            return await _service.SearchMany(type, ids);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting many documents with type {type}", type);
            return new InternalServerErrorResponse(new InternalServerErrorDto(ex.Message));
        }
    }

    [HttpGet]
    [Route("preview")]
    public async Task<ApiResponse> Preview(
        [FromQuery] string type,
        [FromQuery] string query,
        [FromQuery(Name = "ps")] int pageSize = Globals.DefaultPageSize,
        [FromQuery(Name = "p")] int page = Globals.DefaultPage)
    {
        if (!Globals.AvailableTypes.Contains(type))
        {
            return new BadRequestResponse(new BadRequestDto($"Invalid type {type}"));
        }

        if (PaginationValidator.IsInvalid(pageSize, page))
        {
            return new BadRequestResponse(new InvalidPaginationDto());
        }

        try
        {
            return await _service.Preview(type, query, pageSize, page);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting preview with type {type}", type);
            return new InternalServerErrorResponse(new InternalServerErrorDto(ex.Message));
        }
    }

    [HttpGet]
    [Route("simple")]
    public async Task<ApiResponse> Search(
        [FromQuery] string type,
        [FromQuery] string query,
        [FromQuery(Name = "ps")] int pageSize = Globals.DefaultPageSize,
        [FromQuery(Name = "p")] int page = Globals.DefaultPage)
    {
        if (!Globals.AvailableTypes.Contains(type))
        {
            return new BadRequestResponse(new BadRequestDto($"Invalid type {type}"));
        }

        if (PaginationValidator.IsInvalid(pageSize, page))
        {
            return new BadRequestResponse(new InvalidPaginationDto());
        }

        try
        {
            return await _service.Search(type, query, pageSize, page);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting search with type {type}", type);
            return new InternalServerErrorResponse(new InternalServerErrorDto(ex.Message));
        }
    }
}