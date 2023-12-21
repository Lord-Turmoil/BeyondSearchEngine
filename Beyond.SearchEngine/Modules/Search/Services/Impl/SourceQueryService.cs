// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using AutoMapper;
using Beyond.SearchEngine.Extensions.Cache;
using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Search.Models;
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
        string key = $"source:all:{pageSize}:{page}";
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
    /// <returns></returns>
    public async Task<ApiResponse> GetHost(string id)
    {
        var impl = new SearchImpl(_client, _mapper, _cache);

        SourceDto? dto = await impl.SearchSingleById<Source, SourceDto>(IndexName, id);
        if (dto == null || string.IsNullOrEmpty(dto.HostId))
        {
            return new NotFoundResponse(new NotFoundDto());
        }

        OpenAlexDto? data;
        if (dto.HostId.StartsWith("I"))
        {
            data = await impl.SearchSingleById<Institution, InstitutionDto>("institutions", dto.HostId);
        }
        else
        {
            data = await impl.SearchSingleById<Publisher, PublisherDto>("publishers", dto.HostId);
        }

        if (data == null)
        {
            return new NotFoundResponse(new NotFoundDto());
        }

        return new OkResponse(new OkDto(data: data));
    }
}