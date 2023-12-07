using Beyond.SearchEngine.Modules.Update.Dtos;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Update.Services;

public interface IUpdateService
{
    public Task<ApiResponse> InitiateUpdate(string type, InitiateUpdateDto dto);

    public ApiResponse QueryUpdateStatus(string type);
}