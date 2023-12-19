using Beyond.SearchEngine.Modules.Search.Services;
using Microsoft.AspNetCore.Mvc;
using Tonisoft.AspExtensions.Module;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Controllers;

[Route("api/search/")]
[ApiController]
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
    [Route("single")]
    [HttpGet]
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
    [Route("many")]
    [HttpGet]
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
            return new InternalServerErrorResponse(new InternalServerErrorDto(ex.Message));
        }
    }

    [Route("preview")]
    [HttpGet]
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

        try
        {
            return await _service.Preview(type, query, pageSize, page);
        }
        catch (Exception ex)
        {
            return new InternalServerErrorResponse(new InternalServerErrorDto(ex.Message));
        }
    }
}