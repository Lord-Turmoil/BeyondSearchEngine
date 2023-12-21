// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

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
    private readonly IUnitOfWork _unitOfWork;

    protected BaseUpdater(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateTask> logger)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _logger = logger;
    }

    public abstract Task Update(string type, InitiateUpdateDto dto);

    /// <summary>
    ///     Create a new update history if not exists.
    /// </summary>
    /// <param name="type">Update type.</param>
    /// <param name="time">Update time of the data, not update action.</param>
    /// <param name="partId"></param>
    /// <returns>
    ///     Whether successfully added the history. Return false if already exists
    ///     and is completed.
    /// </returns>
    private async ValueTask<bool> AddUpdateHistory(string type, DateOnly time, int partId)
    {
        IRepository<UpdateHistory> repo = _unitOfWork.GetRepository<UpdateHistory>();
        string timeString = time.ToString("yyyy-MM-dd");
        UpdateHistory? history = await repo.GetFirstOrDefaultAsync(
            predicate: x => x.Type == type && x.UpdatedTime.Equals(timeString) && x.PartId == partId);
        if (history != null && history.IsCompleted)
        {
            return false;
        }

        if (history == null)
        {
            await repo.InsertAsync(new UpdateHistory {
                Type = type,
                UpdatedTime = timeString,
                PartId = partId,
                Created = DateTime.UtcNow,
                Completed = null
            });
        }
        else
        {
            history.Created = DateTime.UtcNow;
            repo.Update(history);
        }

        await _unitOfWork.SaveChangesAsync();

        return true;
    }

    /// <summary>
    ///     Complete update history.
    /// </summary>
    /// <param name="type">Update type.</param>
    /// <param name="time">Update time of the data.</param>
    /// <param name="partId">Part ID.</param>
    /// <param name="recordCount">How many record added.</param>
    /// <returns>Whether successfully saved the history or not.</returns>
    private async ValueTask<bool> CompleteUpdateHistory(string type, DateOnly time, int partId, int recordCount)
    {
        IRepository<UpdateHistory> repo = _unitOfWork.GetRepository<UpdateHistory>();
        string timeString = time.ToString("yyyy-MM-dd");
        UpdateHistory? history = await repo.GetFirstOrDefaultAsync(predicate:
            x => x.Type.Equals(type) && x.UpdatedTime.Equals(timeString) && x.PartId == partId,
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

        if (await AddUpdateHistory(type, entry.UpdatedDate, entry.PartId))
        {
            return 1;
        }

        _logger.LogWarning("{type} updated at {UpdatedDate}:{Id} is already updated", type, entry.UpdatedDate,
            entry.PartId);
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
        if (!await CompleteUpdateHistory(type, entry.UpdatedDate, entry.PartId, recordCount))
        {
            _logger.LogError("Failed to complete update of {type} at {UpdatedDate}", type, entry.UpdatedDate);
        }
        else
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }
}