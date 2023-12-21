// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Search.Services;
using Beyond.SearchEngine.Modules.Utils;
using Microsoft.AspNetCore.Mvc;
using Tonisoft.AspExtensions.Module;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Controllers;

[ApiController]
[Route("api/search/concepts")]
public class ConceptQueryController : BaseController<ConceptQueryController>
{
    private readonly IConceptQueryService _service;

    public ConceptQueryController(ILogger<ConceptQueryController> logger, IConceptQueryService service)
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
            _logger.LogError(ex, "Error while getting all concepts with prefix {prefix}", prefix);
            return new InternalServerErrorResponse(new InternalServerErrorDto(ex.Message));
        }
    }
}