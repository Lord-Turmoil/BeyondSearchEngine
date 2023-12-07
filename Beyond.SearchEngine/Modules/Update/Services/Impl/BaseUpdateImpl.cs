using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Beyond.SearchEngine.Modules.Update.Models;

namespace Beyond.SearchEngine.Modules.Update.Services.Impl;

public class BaseUpdateImpl
{
    protected readonly IMapper _mapper;
    protected readonly IUnitOfWork _unitOfWork;

    protected BaseUpdateImpl(IUnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    protected async ValueTask<bool> AddUpdateHistory(string type, DateOnly time)
    {
        IRepository<UpdateHistory> repo = _unitOfWork.GetRepository<UpdateHistory>();
        if (await repo.ExistsAsync(x => x.Type.Equals(type) && x.UpdatedTime.Equals(time)))
        {
            return false;
        }

        await repo.InsertAsync(new UpdateHistory {
            Type = type,
            UpdatedTime = time,
            Created = DateTime.UtcNow
        });

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    protected async ValueTask<bool> CompleteUpdateHistory(string type, DateOnly time)
    {
        IRepository<UpdateHistory> repo = _unitOfWork.GetRepository<UpdateHistory>();
        UpdateHistory? history = await repo.GetFirstOrDefaultAsync(predicate:
            x => x.Type.Equals(type) && x.UpdatedTime.Equals(time));
        if (history == null)
        {
            return false;
        }

        if (history.IsCompleted)
        {
            return false;
        }

        history.Completed = DateTime.UtcNow;

        return true;
    }
}