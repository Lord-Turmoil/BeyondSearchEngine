// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using AutoMapper;
using Beyond.SearchEngine.Extensions.Cache;
using Beyond.SearchEngine.Extensions.Module;
using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Search.Models;
using Beyond.SearchEngine.Modules.Utils;
using Beyond.Shared.Dtos;
using Nest;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Services.Impl;

public class SourceQueryService : ElasticService<SourceQueryService>, ISourceQueryService
{
    private const string IndexName = "sources";
    private readonly ICacheAdapter _cache;

    public SourceQueryService(IElasticClient client, IMapper mapper, ILogger<SourceQueryService> logger,
        ICacheAdapter cache)
        : base(client, mapper, logger)
    {
        _cache = cache;
    }

    public async Task<ApiResponse> GetAll(int pageSize, int page)
    {
        string key = $"{IndexName}:all:{pageSize}:{page}";
        var value = await _cache.GetAsync<PagedDto>(key);
        if (value != null)
        {
            return new OkResponse(new OkDto(data: value));
        }

        ISearchResponse<Source> response = await _client.SearchAsync<Source>(s => s.Index(IndexName)
            .From(page * pageSize)
            .Size(pageSize)
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
            response.Documents.Select(_mapper.Map<Source, SourceDto>));
        await _cache.SetAsync(key, value);

        return new OkResponse(new OkDto(data: value));
    }

    /// <summary>
    ///     Since it may return two types of classes, we don't use
    ///     direct cache for it.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="brief"></param>
    /// <returns></returns>
    public async Task<ApiResponse> GetHost(string id, bool brief)
    {
        var agent = new SearchAgent(_client, _mapper, _cache);

        SourceDto? dto = await agent.GetSingleById<Source, SourceDto>(IndexName, id, brief);
        if (dto == null || string.IsNullOrEmpty(dto.HostId))
        {
            return new NotFoundResponse(new NotFoundDto());
        }

        OpenAlexDto? data;
        if (dto.HostId.StartsWith("I"))
        {
            data = await agent.GetSingleById<Institution, InstitutionDto>("institutions", dto.HostId, brief);
        }
        else
        {
            data = await agent.GetSingleById<Publisher, PublisherDto>("publishers", dto.HostId, brief);
        }

        if (data == null)
        {
            return new NotFoundResponse(new NotFoundDto());
        }

        return new OkResponse(new OkDto(data: data));
    }

    public async Task<ApiResponse> GetRandomHot(bool brief, int pageSize)
    {
        int page = DataMock.RandomInt(0, Globals.MaxPagePressure / pageSize);

        string key = $"{IndexName}:random:{brief}:{pageSize}:{page}";
        var value = await _cache.GetAsync<PagedDto>(key);
        if (value != null)
        {
            return new OkResponse(new OkDto(data: value));
        }

        ISearchResponse<Source> response = await _client.SearchAsync<Source>(s => s
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

        if (brief)
        {
            value = new PagedDto(
                response.Total,
                pageSize,
                page,
                response.Documents.Select(_mapper.Map<Source, DehydratedStatisticsModelDto>));
        }
        else
        {
            value = new PagedDto(
                response.Total,
                pageSize,
                page,
                response.Documents.Select(_mapper.Map<Source, SourceDto>));
        }

        await _cache.SetAsync(key, value);

        return new OkResponse(new OkDto(data: value));
    }

    public async Task<ApiResponse> GetTopSourceStatisticsByWorksCount(int pageSize, int page)
    {
        string key = $"{IndexName}:top:{pageSize}:{page}";
        var value = await _cache.GetAsync<PagedDto>(key);
        if (value != null)
        {
            return new OkResponse(new OkDto(data: value));
        }

        ISearchResponse<Source> response = await _client.SearchAsync<Source>(s => s
            .Index(IndexName)
            .From(page * pageSize)
            .Size(pageSize)
            .Sort(ss => ss.Field(f => f.WorksCount, SortOrder.Descending))
            .Query(q => q.MatchAll()));

        if (!response.IsValid)
        {
            _logger.LogError(response.DebugInformation);
            return new InternalServerErrorResponse(new SearchFailedDto());
        }

        var agent = new SearchAgent(_client, _mapper, _cache);
        long count = await agent.GetCount<Work>(IndexName);
        value = new PagedDto(
            response.Total,
            pageSize,
            page,
            response.Documents.Select(source =>
                new TopSourcesDto {
                    Source = _mapper.Map<Source, DehydratedStatisticsModelDto>(source),
                    Percent = double.Round((double)source.WorksCount / count * 100.0, 2)
                }).ToList());

        await _cache.SetAsync(key, value);

        return new OkResponse(new OkDto(data: value));
    }

    public async Task<ApiResponse> SearchSource(string query, int pageSize, int page)
    {
        string key = $"{IndexName}:search:{query}:{pageSize}:{page}";
        var value = await _cache.GetAsync<PagedDto>(key);
        if (value != null)
        {
            return new OkResponse(new OkDto(data: value));
        }

        ISearchResponse<Source> response = await _client.SearchAsync<Source>(s => s.Index(IndexName)
            .From(page * pageSize)
            .Size(pageSize)
            .Sort(ss => ss.Field(f => f.HIndex, SortOrder.Descending))
            .Query(q => q.Match(m => m.Field(f => f.Name)
                .Query(query).Fuzziness(Globals.DefaultFuzziness))));
        if (!response.IsValid)
        {
            _logger.LogError(response.DebugInformation);
            return new InternalServerErrorResponse(new SearchFailedDto());
        }

        value = new PagedDto(
            response.Total,
            pageSize,
            page,
            response.Documents.Select(_mapper.Map<Source, DehydratedStatisticsModelDto>));
        await _cache.SetAsync(key, value);

        return new OkResponse(new OkDto(data: value));
    }
}