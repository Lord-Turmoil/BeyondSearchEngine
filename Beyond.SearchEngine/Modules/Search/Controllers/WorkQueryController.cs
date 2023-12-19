using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Search.Services;
using Beyond.SearchEngine.Modules.Utils;
using Microsoft.AspNetCore.Mvc;
using Tonisoft.AspExtensions.Module;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Controllers;

[Route("api/search/query/works")]
[ApiController]
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
    public Task<ApiResponse> GetRelatedWorks([FromQuery] string id, [FromQuery] bool brief = true)
    {
        return _service.GetRelatedWorks(id, brief);
    }

    [HttpGet]
    [Route("referenced")]
    public Task<ApiResponse> GetReferencedWorks([FromQuery] string id, [FromQuery] bool brief = true)
    {
        return _service.GetReferencedWorks(id, brief);
    }
}