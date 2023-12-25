using Beyond.SearchEngine.Modules.Deprecated.Services;
using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Utils;
using Microsoft.AspNetCore.Mvc;
using Tonisoft.AspExtensions.Module;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Deprecated.Controllers;

/// <summary>
///     This controller is for all deprecated APIs.
/// </summary>
[ApiController]
[Route("v1/search/d")]
public class DeprecatedController : BaseController<DeprecatedController>
{
    private readonly IDeprecatedService _service;

    public DeprecatedController(ILogger<DeprecatedController> logger, IDeprecatedService service)
        : base(logger)
    {
        _service = service;
    }

    [HttpGet]
    [Route("authors/single")]
    public async Task<ApiResponse> GetAuthorById([FromQuery] string id, [FromQuery] bool brief = true)
    {
        try
        {
            return await _service.GetAuthorById(id, brief);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting deprecated author by ID {id}", id);
            return new InternalServerErrorResponse(new InternalServerErrorDto());
        }
    }

    [HttpGet]
    [Route("query/authors/works")]
    public async Task<ApiResponse> GetWorksOfAuthor(
        [FromQuery] string id,
        [FromQuery] bool brief,
        [FromQuery(Name = "ps")] int pageSize = Globals.DefaultPageSize,
        [FromQuery(Name = "p")] int page = Globals.DefaultPage)
    {
        if (PaginationValidator.IsInvalid(pageSize, page))
        {
            return new BadRequestResponse(new InvalidPaginationDto());
        }

        try
        {
            return await _service.GetWorksOfAuthor(id, brief, pageSize, page);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting deprecated works of author {id}", id);
            return new InternalServerErrorResponse(new InternalServerErrorDto());
        }
    }

    [HttpGet]
    [Route("query/works/single")]
    public async Task<ApiResponse> GetWorkById([FromQuery] string id, [FromQuery] bool brief = true)
    {
        try
        {
            return await _service.GetWorkById(id, brief);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting deprecated work by ID {id}", id);
            return new InternalServerErrorResponse(new InternalServerErrorDto());
        }
    }

    [HttpGet]
    [Route("query/works/citation")]
    public async Task<ApiResponse> GetWorkCitations([FromQuery] IReadOnlyCollection<string> idList)
    {
        if (ListValidator.IsInvalidIdList(idList))
        {
            return new BadRequestResponse(new InvalidIdListDto());
        }

        try
        {
            return await _service.GetWorkCitations(idList);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting deprecated work citations by ID list {idList}", idList);
            return new InternalServerErrorResponse(new InternalServerErrorDto());
        }
    }
}