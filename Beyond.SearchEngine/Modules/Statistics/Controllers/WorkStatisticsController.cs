using Beyond.SearchEngine.Modules.Statistics.Services;
using Microsoft.AspNetCore.Mvc;
using Tonisoft.AspExtensions.Module;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Statistics.Controllers;

[ApiController]
[Route("v1/search/statistics/works")]
public class WorkStatisticsController : BaseController<WorkStatisticsController>
{
    private readonly IWorkStatisticsService _service;

    public WorkStatisticsController(ILogger<WorkStatisticsController> logger, IWorkStatisticsService service) :
        base(logger)
    {
        _service = service;
    }

    [HttpGet]
    [Route("")]
    public async Task<ApiResponse> GetWorkStatistics([FromQuery] string id)
    {
        try
        {
            return await _service.GetStatistics(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while getting work statistics of {id}", id);
            return new InternalServerErrorResponse(new InternalServerErrorDto());
        }
    }

    [HttpPost]
    [Route("like")]
    public async Task<ApiResponse> LikeWork([FromQuery] string id)
    {
        try
        {
            return await _service.LikeWork(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while liking work {id}", id);
            return new InternalServerErrorResponse(new InternalServerErrorDto());
        }
    }

    [HttpPost]
    [Route("unlike")]
    public async Task<ApiResponse> UnlikeWork([FromQuery] string id)
    {
        try
        {
            return await _service.UnLikeWork(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while unliking work {id}", id);
            return new InternalServerErrorResponse(new InternalServerErrorDto());
        }
    }

    [HttpPost]
    [Route("question")]
    public async Task<ApiResponse> QuestionWork([FromQuery] string id)
    {
        try
        {
            return await _service.QuestionWork(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while questioning work {id}", id);
            return new InternalServerErrorResponse(new InternalServerErrorDto());
        }
    }

    [HttpPost]
    [Route("unquestion")]
    public async Task<ApiResponse> UnQuestionWork([FromQuery] string id)
    {
        try
        {
            return await _service.UnQuestionWork(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while unquestioning work {id}", id);
            return new InternalServerErrorResponse(new InternalServerErrorDto());
        }
    }

    [HttpPost]
    [Route("view")]
    public async Task<ApiResponse> ViewWork([FromQuery] string id)
    {
        try
        {
            return await _service.ViewWork(id);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error while viewing work {id}", id);
            return new InternalServerErrorResponse(new InternalServerErrorDto());
        }
    }
}