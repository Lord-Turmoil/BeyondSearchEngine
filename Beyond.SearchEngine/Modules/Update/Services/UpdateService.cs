using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Beyond.SearchEngine.Modules.Update.Dtos;
using Beyond.SearchEngine.Modules.Update.Models;
using Beyond.SearchEngine.Modules.Update.Services.Utils;
using Tonisoft.AspExtensions.Module;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Update.Services;

public class UpdateService : BaseService<UpdateService>, IUpdateService
{
    private readonly UpdateTask _task;

    public UpdateService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateService> logger, UpdateTask task)
        : base(unitOfWork, mapper, logger)
    {
        _task = task;
    }

    public async Task<ApiResponse> InitiateUpdate(string type, InitiateUpdateDto dto)
    {
        if (UpdateMutex.IsUpdating(type))
        {
            return new ForbiddenResponse(new ForbiddenDto($"Another update on {type} is running."));
        }

        // Verify user.
        IRepository<User> repo = _unitOfWork.GetRepository<User>();
        User? user = await repo.GetFirstOrDefaultAsync(predicate: x => x.Username.Equals(dto.Username));
        if (user == null)
        {
            return new UnauthorizedResponse(new UnauthorizedDto());
        }

        if (!user.Password.Equals(dto.Password))
        {
            return new UnauthorizedResponse(new UnauthorizedDto("Wrong password"));
        }

        switch (type)
        {
            case "institution":
                await _task.UpdateInstitutionAsync(type, dto);
                break;
            default:
                return new BadRequestResponse(new BadRequestDto("Invalid update type"));
        }

        return new OkResponse(new OkDto(data: new InitiateUpdateSuccessDto(type, dto.Begin, dto.End)));
    }

    public ApiResponse QueryUpdateStatus(string type)
    {
        bool status = UpdateMutex.IsUpdating(type);
        return new OkResponse(new OkDto(data: status));
    }
}