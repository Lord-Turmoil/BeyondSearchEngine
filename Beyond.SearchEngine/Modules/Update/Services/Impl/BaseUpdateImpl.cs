using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Beyond.SearchEngine.Modules.Update.Models;
using Beyond.Shared.Indexer.Impl;
using Beyond.Shared.Indexer;

namespace Beyond.SearchEngine.Modules.Update.Services.Impl;

public class BaseUpdateImpl
{
    protected readonly IMapper _mapper;
    protected readonly IUnitOfWork _unitOfWork;
    protected readonly ILogger<UpdateTask> _logger;

    protected BaseUpdateImpl(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateTask> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    protected async ValueTask<bool> AddUpdateHistory(string type, DateOnly time)
    {
        IRepository<UpdateHistory> repo = _unitOfWork.GetRepository<UpdateHistory>();
        string timeString = time.ToString("yyyy-MM-dd");
        UpdateHistory? history = await repo.GetFirstOrDefaultAsync(
            predicate: x => x.Type == type && x.UpdatedTime.Equals(timeString));
        if (history != null && history.IsCompleted)
        {
            return false;
        }

        if (history == null)
        {
            await repo.InsertAsync(new UpdateHistory {
                Type = type,
                UpdatedTime = timeString,
                Created = DateTime.UtcNow,
                Completed = null
            });
        }

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    protected async ValueTask<bool> CompleteUpdateHistory(string type, DateOnly time)
    {
        IRepository<UpdateHistory> repo = _unitOfWork.GetRepository<UpdateHistory>();
        string timeString = time.ToString("yyyy-MM-dd");
        UpdateHistory? history = await repo.GetFirstOrDefaultAsync(predicate:
            x => x.Type.Equals(type) && x.UpdatedTime.Equals(timeString),
            disableTracking: false);
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

    /// <summary>
    ///     -1: No more
    ///     0: Already updated
    ///     1: Can update
    /// </summary>
    /// <param name="entry"></param>
    /// <returns></returns>
    protected async ValueTask<int> UpdatePreamble(string type, ManifestEntry? entry)
    {
        if (entry == null)
        {
            return -1;
        }

        if (!await AddUpdateHistory(type, entry.UpdatedDate))
        {
            _logger.LogWarning($"{type} updated at {entry.UpdatedDate} is already updated");
            return 0;
        }

        return 1;
    }

    protected async ValueTask PostUpdate(string type, ManifestEntry entry)
    {
        if (!await CompleteUpdateHistory(type, entry.UpdatedDate))
        {
            _logger.LogError($"Failed to complete update of {type} at {entry.UpdatedDate}");
        }
        else
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }
}