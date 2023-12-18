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

    [HttpPost]
    [Route("configure")]
    public async Task<ApiResponse> ConfigureUpdate([FromBody] ConfigureUpdateDto dto)
    {
        if (!dto.Format().Verify())
        {
            return new BadRequestResponse(new BadRequestDto("Invalid format"));
        }

        return await _updateService.ConfigureUpdate(dto);
    }

    [HttpGet]
    [Route("status")]
    public ApiResponse QueryUpdateStatus([FromQuery] string type)
    {
        return _updateService.QueryUpdateStatus(type);
    }

    [HttpPost]
    [Route("initiate")]
    public async Task<ApiResponse> InitiateUpdate([FromBody] InitiateUpdateDto dto)
    {
        if (!dto.Format().Verify())
        {
            return new BadRequestResponse(new BadRequestDto("Invalid format"));
        }

        try
        {
            return await _updateService.InitiateUpdate(dto.Type, dto);
        }
        catch (Exception ex)
        {
            return new InternalServerErrorResponse(new InternalServerErrorDto(ex.Message));
        }
    }
}