﻿using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Search.Services;
using Beyond.SearchEngine.Modules.Utils;
using Microsoft.AspNetCore.Mvc;
using Tonisoft.AspExtensions.Module;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Controllers;

[Route("api/search/concepts")]
[ApiController]
public class ConceptSearchController : BaseController<ConceptSearchController>
{
    private readonly IConceptQueryService _service;

    public ConceptSearchController(ILogger<ConceptSearchController> logger, IConceptQueryService service)
        : base(logger)
    {
        _service = service;
    }

    [HttpGet]
    [Route("prefix")]
    public async Task<ApiResponse> GetAllWithPrefix(
        [FromQuery] string prefix,
        [FromQuery(Name = "ps")] int pageSize = Globals.DefaultPageSize,
        [FromQuery(Name = "p")] int page = Globals.DefaultPage)
    {
        if (PaginationValidator.IsInvalid(pageSize, page))
        {
            return new BadRequestResponse(new InvalidPaginationDto());
        }

        try
        {
            return await _service.GetAllWithPrefix(prefix, pageSize, page);
        }
        catch (Exception ex)
        {
            return new InternalServerErrorResponse(new InternalServerErrorDto(ex.Message));
        }
    }
}