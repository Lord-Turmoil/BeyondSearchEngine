using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Beyond.SearchEngine.Modules.Update.Dtos;
using Beyond.SearchEngine.Modules.Update.Models;
using Beyond.Shared.Indexer;

namespace Beyond.SearchEngine.Modules.Update.Services.Updater;

public abstract class BaseUpdater : IUpdater
{
    protected readonly ILogger<UpdateTask> _logger;
    protected readonly IMapper _mapper;
    protected readonly IUnitOfWork _unitOfWork;

    protected BaseUpdater(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateTask> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public abstract Task Update(string type, InitiateUpdateDto dto, string dataPath, string tempPath);

    /// <summary>
    ///     Create a new update history if not exists.
    /// </summary>
    /// <param name="type">Update type.</param>
    /// <param name="time">Update time of the data, not update action.</param>
    /// <returns>
    ///     Whether successfully added the history. Return false if already exists
    ///     and is completed.
    /// </returns>
    private async ValueTask<bool> AddUpdateHistory(string type, DateOnly time)
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

    /// <summary>
    ///     Complete update history.
    /// </summary>
    /// <param name="type">Update type.</param>
    /// <param name="time">Update time of the data.</param>
    /// <param name="recordCount">How many record added.</param>
    /// <returns>Whether successfully saved the history or not.</returns>
    private async ValueTask<bool> CompleteUpdateHistory(string type, DateOnly time, int recordCount)
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

        history.RecordCount = recordCount;
        history.Completed = DateTime.UtcNow;
        history.ElapsedSeconds = (history.Completed.Value - history.Created).TotalSeconds;

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

        if (await AddUpdateHistory(type, entry.UpdatedDate))
        {
            return 1;
        }

        _logger.LogWarning("{type} updated at {UpdatedDate} is already updated", type, entry.UpdatedDate);
        return 0;
    }

    /// <summary>
    ///     Save history after update.
    /// </summary>
    /// <param name="type">See <see cref="CompleteUpdateHistory" /></param>
    /// <param name="entry">See <see cref="CompleteUpdateHistory" /></param>
    /// <param name="recordCount">See <see cref="CompleteUpdateHistory" /></param>
    /// <returns></returns>
    protected async ValueTask PostUpdate(string type, ManifestEntry entry, int recordCount)
    {
        if (!await CompleteUpdateHistory(type, entry.UpdatedDate, recordCount))
        {
            _logger.LogError("Failed to complete update of {type} at {UpdatedDate}", type, entry.UpdatedDate);
        }
        else
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }
}