using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Search.Services;
using Beyond.SearchEngine.Modules.Utils;
using Microsoft.AspNetCore.Mvc;
using Tonisoft.AspExtensions.Module;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Controllers;

[ApiController]
[Route("api/search/query/works")]
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
}