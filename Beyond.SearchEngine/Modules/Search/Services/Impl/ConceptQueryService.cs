// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using AutoMapper;
using Beyond.SearchEngine.Extensions.Cache;
using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Search.Models;
using Nest;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Services.Impl;

public class ConceptQueryService : ElasticService<ConceptQueryService>, IConceptQueryService
{
    private const string IndexName = "concepts";
    private readonly ICacheAdapter _cache;

    public ConceptQueryService(IElasticClient client, IMapper mapper, ILogger<ConceptQueryService> logger,
        ICacheAdapter cache)
        : base(client, mapper, logger)
    {
        _cache = cache;
    }

    public async Task<ApiResponse> GetAllWithPrefix(string prefix, int pageSize, int page)
    {
        string key = $"source:prefix:{prefix}:{pageSize}:{page}";

        var value = await _cache.GetAsync<PagedDto>(key);
        if (value != null)
        {
            return new OkResponse(new OkDto(data: value));
        }

        ISearchResponse<Concept> response = await _client.SearchAsync<Concept>(s => s
            .Index(IndexName)
            .From(page * pageSize)
            .Size(pageSize)
            .Query(q => q.MatchPhrasePrefix(m => m.Field(f => f.Name).Query(prefix))));
        if (!response.IsValid)
        {
            _logger.LogError(response.DebugInformation);
            return new InternalServerErrorResponse(new SearchFailedDto());
        }

        value = new PagedDto(
            response.Total,
            pageSize,
            page,
            response.Documents.Select(_mapper.Map<Concept, DehydratedStatisticsModelDto>).ToList()
        );
        await _cache.SetAsync(key, value);

        return new OkResponse(new OkDto(data: value));
    }

    public async Task<ApiResponse> GetTopConcepts(int pageSize, int page)
    {
        string key = $"source:top:{pageSize}:{page}";
        var value = await _cache.GetAsync<PagedDto>(key);
        if (value != null)
        {
            return new OkResponse(new OkDto(data: value));
        }

        ISearchResponse<Concept> response = await _client.SearchAsync<Concept>(s => s
            .Index(IndexName)
            .From(page * pageSize)
            .Size(pageSize)
            .Sort(ss => ss.Field(f => f.HIndex, SortOrder.Descending))
            .Query(q => q.MatchAll()));

        if (!response.IsValid)
        {
            _logger.LogError(response.DebugInformation);
            return new InternalServerErrorResponse(new SearchFailedDto());
        }

        value = new PagedDto(
            response.Total,
            pageSize,
            page,
            response.Documents.Select(_mapper.Map<Concept, DehydratedStatisticsModelDto>).ToList()
        );

        await _cache.SetAsync(key, value);

        return new OkResponse(new OkDto(data: value));
    }
}