// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Search.Services;
using Beyond.SearchEngine.Modules.Utils;
using Microsoft.AspNetCore.Mvc;
using Tonisoft.AspExtensions.Module;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Controllers;

[ApiController]
[Route("v1/search/query/authors")]
public class AuthorQueryController : BaseController<AuthorQueryController>
{
    private readonly IAuthorQueryService _service;

    public AuthorQueryController(ILogger<AuthorQueryController> logger, IAuthorQueryService service) : base(logger)
    {
        _service = service;
    }


    [HttpGet]
    [Route("works")]
    public async Task<ApiResponse> GetWorks(
        [FromQuery] string id,
        [FromQuery] bool brief = true,
        [FromQuery(Name = "ps")] int pageSize = Globals.DefaultPageSize,
        [FromQuery(Name = "p")] int page = Globals.DefaultPage)
    {
        if (PaginationValidator.IsInvalid(pageSize, page))
        {
            return new BadRequestResponse(new InvalidPaginationDto());
        }

        try
        {
            return await _service.GetWorks(id, brief, pageSize, page);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting all works of author {id}", id);
            return new InternalServerErrorResponse(new InternalServerErrorDto(ex.Message));
        }
    }

    [HttpGet]
    [Route("institution")]
    public async Task<ApiResponse> GetInstitution([FromQuery] string id, [FromQuery] bool brief = true)
    {
        try
        {
            return await _service.GetInstitution(id, brief);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting institution of author {id}", id);
            return new InternalServerErrorResponse(new InternalServerErrorDto(ex.Message));
        }
    }

    [HttpGet]
    [Route("top")]
    public async Task<ApiResponse> GetTopAuthors(
        [FromQuery(Name = "ps")] int pageSize = Globals.DefaultPageSize,
        [FromQuery(Name = "p")] int page = Globals.DefaultPage)
    {
        if (PaginationValidator.IsInvalid(pageSize, page))
        {
            return new BadRequestResponse(new InvalidPaginationDto());
        }

        try
        {
            return await _service.GetTopAuthors(pageSize, page);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting top authors");
            return new InternalServerErrorResponse(new InternalServerErrorDto(ex.Message));
        }
    }
}