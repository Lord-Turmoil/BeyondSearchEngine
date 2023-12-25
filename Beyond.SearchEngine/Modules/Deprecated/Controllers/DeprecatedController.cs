using Beyond.SearchEngine.Modules.Deprecated.Services;
using Microsoft.AspNetCore.Mvc;
using Tonisoft.AspExtensions.Module;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Deprecated.Controllers;

/// <summary>
///     This controller is for all deprecated APIs.
/// </summary>
[ApiController]
[Route("v1/search/deprecated")]
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
}