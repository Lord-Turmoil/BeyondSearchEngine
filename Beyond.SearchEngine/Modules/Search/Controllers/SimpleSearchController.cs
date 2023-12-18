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
            return await _service.SearchSingleAsync(type, id);
        }
        catch (Exception ex)
        {
            return new InternalServerErrorResponse(new InternalServerErrorDto(ex.Message));
        }
    }

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
            return await _service.SearchManyAsync(type, ids);
        }
        catch (Exception ex)
        {
            return new InternalServerErrorResponse(new InternalServerErrorDto(ex.Message));
        }
    }
}