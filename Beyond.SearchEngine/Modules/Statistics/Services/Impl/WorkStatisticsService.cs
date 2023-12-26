// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Beyond.SearchEngine.Extensions.Cache;
using Beyond.SearchEngine.Modules.Statistics.Dtos;
using Beyond.SearchEngine.Modules.Statistics.Models;
using Tonisoft.AspExtensions.Module;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Statistics.Services.Impl;

public class WorkStatisticsService : BaseService<WorkStatisticsService>, IWorkStatisticsService
{
    private readonly ICacheAdapter _cache;

    private readonly Random _random = new();

    public WorkStatisticsService(IUnitOfWork unitOfWork, IMapper mapper, ILogger<WorkStatisticsService> logger,
        ICacheAdapter cache) : base(unitOfWork, mapper, logger)
    {
        _cache = cache;
    }

    public async Task<ApiResponse> GetStatistics(string id)
    {
        WorkStatistics? value = await GetOrCreateStatistics(id);
        if (value == null)
        {
            return new NotFoundResponse(new NotFoundDto());
        }

        return new OkResponse(new OkDto(data: _mapper.Map<WorkStatistics, WorkStatisticsDto>(value)));
    }

    public async Task<ApiResponse> LikeWork(int userId, string workId)
    {
        IRepository<UserLikeRecord> repo = _unitOfWork.GetRepository<UserLikeRecord>();
        UserLikeRecord? record = await repo.FindAsync(userId, workId);
        if (record != null)
        {
            return new OkResponse(new OkDto("Already liked the work"));
        }

        WorkStatistics? value = await GetOrCreateStatistics(workId);
        if (value == null)
        {
            return new NotFoundResponse(new NotFoundDto("Work not found"));
        }

        value.Likes++;

        await UpdateStatistics(value);
        await repo.InsertAsync(new UserLikeRecord {
            UserId = userId,
            WorkId = workId,
            Created = DateTime.Now
        });
        await _unitOfWork.SaveChangesAsync();

        return new OkResponse(new OkDto(data: value.Likes));
    }

    public async Task<ApiResponse> UnLikeWork(int userId, string workId)
    {
        IRepository<UserLikeRecord> repo = _unitOfWork.GetRepository<UserLikeRecord>();
        UserLikeRecord? record = await repo.FindAsync(userId, workId);
        if (record == null)
        {
            return new OkResponse(new OkDto("You didn't like the work"));
        }

        WorkStatistics? value = await GetOrCreateStatistics(workId);
        if (value == null)
        {
            return new NotFoundResponse(new NotFoundDto("Work not found"));
        }

        value.Likes--;
        await UpdateStatistics(value);
        repo.Delete(record);
        await _unitOfWork.SaveChangesAsync();

        return new OkResponse(new OkDto(data: value.Likes));
    }

    public async Task<ApiResponse> IsLiked(int userId, string workId)
    {
        IRepository<UserLikeRecord> repo = _unitOfWork.GetRepository<UserLikeRecord>();
        bool value = await repo.FindAsync(userId, workId) != null;

        return new OkResponse(new OkDto(data: value));
    }

    public async Task<ApiResponse> ViewWork(string id)
    {
        WorkStatistics? value = await GetOrCreateStatistics(id);
        if (value == null)
        {
            return new NotFoundResponse(new NotFoundDto());
        }

        value.Views++;

        await UpdateStatistics(value);
        await _unitOfWork.SaveChangesAsync();

        return new OkResponse(new OkDto(data: value.Views));
    }

    private static string GetKey(string id)
    {
        return $"work:statistics:{id}";
    }

    private async Task<WorkStatistics?> GetOrCreateStatistics(string id)
    {
        string key = GetKey(id);

        var value = await _cache.GetAsync<WorkStatistics>(key);
        if (value != null)
        {
            return value;
        }

        IRepository<WorkStatistics> repo = _unitOfWork.GetRepository<WorkStatistics>();
        value = await repo.FindAsync(id);
        if (value != null)
        {
            await _cache.SetAsync(key, value);
            return value;
        }

        // Then this value is not exist in the database.
        // Create a new statistics.
        value = new WorkStatistics {
            Id = id,
            Likes = _random.Next(100, 500),
            Views = _random.Next(1000, 5000)
        };

        await repo.InsertAsync(value);
        await _unitOfWork.SaveChangesAsync();
        await _cache.SetAsync(key, value);

        return value;
    }

    // Warning: It will not save!
    private async Task UpdateStatistics(WorkStatistics statistics)
    {
        string key = GetKey(statistics.Id);
        await _cache.SetAsync(key, statistics);

        IRepository<WorkStatistics> repo = _unitOfWork.GetRepository<WorkStatistics>();
        repo.Update(statistics);
    }
}