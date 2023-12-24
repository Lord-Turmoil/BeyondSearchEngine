using AutoMapper;
using Beyond.SearchEngine.Extensions.Cache;
using Beyond.SearchEngine.Extensions.Module;
using Beyond.SearchEngine.Modules.Search.Models;
using Beyond.SearchEngine.Modules.Statistics.Models;
using Beyond.SearchEngine.Modules.Utils;
using Nest;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Statistics.Services.Impl;

public class WorkStatisticsService : ElasticService<WorkStatisticsService>, IWorkStatisticsService
{
    private const string IndexName = "work-statistics";

    private readonly ICacheAdapter _cache;

    public WorkStatisticsService(IElasticClient client, IMapper mapper, ILogger<WorkStatisticsService> logger,
        ICacheAdapter cache)
        : base(client, mapper, logger)
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

        return new OkResponse(new OkDto(data: value));
    }

    public async Task<ApiResponse> LikeWork(string id)
    {
        WorkStatistics? value = await GetOrCreateStatistics(id);
        if (value == null)
        {
            return new NotFoundResponse(new NotFoundDto());
        }

        value.Likes++;
        await UpdateStatistics(value);

        return new OkResponse(new OkDto(data: value.Likes));
    }

    public async Task<ApiResponse> UnLikeWork(string id)
    {
        WorkStatistics? value = await GetOrCreateStatistics(id);
        if (value == null)
        {
            return new NotFoundResponse(new NotFoundDto());
        }

        value.Likes--;
        if (value.Likes < 0)
        {
            value.Likes = 0;
        }

        await UpdateStatistics(value);

        return new OkResponse(new OkDto(data: value.Likes));
    }

    public async Task<ApiResponse> QuestionWork(string id)
    {
        WorkStatistics? value = await GetOrCreateStatistics(id);
        if (value == null)
        {
            return new NotFoundResponse(new NotFoundDto());
        }

        value.Questions++;

        await UpdateStatistics(value);

        return new OkResponse(new OkDto(data: value.Questions));
    }

    public async Task<ApiResponse> UnQuestionWork(string id)
    {
        WorkStatistics? value = await GetOrCreateStatistics(id);
        if (value == null)
        {
            return new NotFoundResponse(new NotFoundDto());
        }

        value.Questions--;
        if (value.Questions < 0)
        {
            value.Questions = 0;
        }

        await UpdateStatistics(value);

        return new OkResponse(new OkDto(data: value.Questions));
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

        var agent = new SearchAgent(_client, _mapper, _cache);
        value = await agent.GetModelById<WorkStatistics>(IndexName, id);
        if (value != null)
        {
            await _cache.SetAsync(key, value);
            return value;
        }

        // Then this value is not exist in the database.
        // First check if work exists.
        var work = await agent.GetModelById<Work>("works", id);
        if (work == null)
        {
            // No corresponding work.
            return null;
        }

        // Create a new statistics.
        value = new WorkStatistics {
            Id = id,
            Likes = 0,
            Questions = 0,
            Views = 0
        };
        IndexResponse response = await _client.IndexAsync(value, op => op
            .Index(IndexName)
            .Id(id));
        if (!response.IsValid)
        {
            _logger.LogError(response.DebugInformation);
            // Warning: We swallow this error, and return an instant value.
        }

        await _cache.SetAsync(key, value);

        return value;
    }

    private async Task UpdateStatistics(WorkStatistics statistics)
    {
        string key = GetKey(statistics.Id);
        await _cache.SetAsync(key, statistics);
        await _client.IndexAsync(statistics, op => op
            .Index(IndexName)
            .Id(statistics.Id));
    }
}