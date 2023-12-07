using Beyond.SearchEngine.Modules.Update.Dtos;
using Beyond.SearchEngine.Modules.Update.Services;
using Microsoft.AspNetCore.Mvc;
using Tonisoft.AspExtensions.Module;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Update.Controllers;

[Route("api/search/update")]
[ApiController]
public class UpdateController : BaseController<UpdateController>
{
    private readonly IUpdateService _updateService;

    public UpdateController(ILogger<UpdateController> logger, IUpdateService updateService)
        : base(logger)
    {
        _updateService = updateService;
    }

    [HttpGet]
    [Route("status/{type}")]
    public ApiResponse QueryUpdateStatus([FromRoute] string type)
    {
        return _updateService.QueryUpdateStatus(type);
    }

    [HttpPost]
    [Route("{type}")]
    public async Task<IActionResult> InitiateUpdate([FromRoute] string type, [FromBody] InitiateUpdateDto dto)
    {
        if (!dto.Format().Verify())
        {
            return BadRequest();
        }

        try
        {
            return await _updateService.InitiateUpdate(type, dto);
        }
        catch (Exception e)
        {
            return new InternalServerErrorResponse(new InternalServerErrorDto(e.Message));
        }
    }
}