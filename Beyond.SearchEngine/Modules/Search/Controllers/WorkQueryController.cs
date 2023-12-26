// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Search.Services;
using Beyond.SearchEngine.Modules.Utils;
using Microsoft.AspNetCore.Mvc;
using Nest;
using Tonisoft.AspExtensions.Module;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Controllers;

[ApiController]
[Route("v1/search/query/works")]
public class WorkQueryController : BaseController<WorkQueryController>
{
    private readonly IWorkQueryService _service;

    public WorkQueryController(ILogger<WorkQueryController> logger, IWorkQueryService service)
        : base(logger)
    {
        _service = service;
    }

    [HttpGet]
    [Route("related")]
    public async Task<ApiResponse> GetRelatedWorks([FromQuery] string id, [FromQuery] bool brief = true)
    {
        try
        {
            return await _service.GetRelatedWorks(id, brief);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting related works of {id}", id);
            return new InternalServerErrorResponse(new InternalServerErrorDto(ex.Message));
        }
    }

    [HttpGet]
    [Route("referenced")]
    public async Task<ApiResponse> GetReferencedWorks([FromQuery] string id, [FromQuery] bool brief = true)
    {
        try
        {
            return await _service.GetReferencedWorks(id, brief);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting referenced works of {id}", id);
            return new InternalServerErrorResponse(new InternalServerErrorDto(ex.Message));
        }
    }

    [HttpPost]
    [HttpGet]
    [Route("basic")]
    public async Task<ApiResponse> QueryBasic([FromBody] QueryWorkBasicDto dto)
    {
        if (!dto.Verify())
        {
            return new BadRequestResponse(new BadRequestDto());
        }

        try
        {
            ApiResponse response = await _service.QueryWorksBasic(dto);
            Response.Headers.Append("Access-Control-Expose-Headers", "X-Response-Time");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while basic querying works with {dto}", dto);
            return new InternalServerErrorResponse(new InternalServerErrorDto(ex.Message));
        }
    }

    [HttpPost]
    [HttpGet]
    [Route("advanced")]
    public async Task<ApiResponse> QueryAdvanced([FromBody] QueryWorkAdvancedDto dto)
    {
        if (!dto.Verify())
        {
            return new BadRequestResponse(new BadRequestDto());
        }

        try
        {
            ApiResponse response = await _service.QueryWorksAdvanced(dto);
            Response.Headers.Append("Access-Control-Expose-Headers", "X-Response-Time");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while advanced querying works with {dto}", dto);
            return new InternalServerErrorResponse(new InternalServerErrorDto(ex.Message));
        }
    }

    [HttpPost]
    [HttpGet]
    [Route("all")]
    public async Task<ApiResponse> QueryAll([FromBody] QueryWorkAllFieldsDto dto)
    {
        if (!dto.Verify())
        {
            return new BadRequestResponse(new BadRequestDto());
        }

        try
        {
            ApiResponse response = await _service.QueryWorksWithAllFields(dto);
            Response.Headers.Append("Access-Control-Expose-Headers", "X-Response-Time");
            return response;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while querying works with {dto}", dto);
            return await Task.FromResult<ApiResponse>(new InternalServerErrorResponse(new InternalServerErrorDto(ex.Message)));
        }
    }

    [HttpGet]
    [Route("citation")]
    public async Task<ApiResponse> ExportCitation(
        [FromQuery] string type,
        [FromQuery(Name = "ids")] IReadOnlyCollection<string> idList)
    {
        if (ListValidator.IsInvalidIdList(idList))
        {
            return new BadRequestResponse(new InvalidIdListDto());
        }

        try
        {
            return await _service.GetCitations(type, idList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while exporting citations of {type} with {idList}", type, idList);
            return new InternalServerErrorResponse(new InternalServerErrorDto(ex.Message));
        }
    }

    [HttpGet]
    [Route("top")]
    public async Task<ApiResponse> GetTopWorks(
        [FromQuery] DateTime? begin = null,
        [FromQuery] DateTime? end = null,
        [FromQuery(Name = "ps")] int pageSize = Globals.DefaultPageSize,
        [FromQuery(Name = "p")] int page = Globals.DefaultPage)
    {
        if (PaginationValidator.IsInvalid(pageSize, page))
        {
            return new BadRequestResponse(new InvalidPaginationDto());
        }

        if (begin != null && end != null)
        {
            if (begin >= end)
            {
                return new BadRequestResponse(new BadRequestDto("Invalid date range"));
            }
        }

        try
        {
            return await _service.GetTopWorks(begin, end, pageSize, page);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting top works");
            return new InternalServerErrorResponse(new InternalServerErrorDto(ex.Message));
        }
    }
}