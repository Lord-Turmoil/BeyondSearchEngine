﻿// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using AutoMapper;
using Beyond.SearchEngine.Extensions.Cache;
using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Search.Models;
using Beyond.SearchEngine.Modules.Search.Services.Exceptions;
using Beyond.Shared.Dtos;
using Nest;

namespace Beyond.SearchEngine.Modules.Search.Services.Impl;

/// <summary>
///     Fundamental implementation for searching items.
/// </summary>
public class SearchImpl
{
    private readonly ICacheAdapter _cache;
    private readonly IElasticClient _client;
    private readonly IMapper _mapper;

    public SearchImpl(IElasticClient client, IMapper mapper, ICacheAdapter cache)
    {
        _client = client;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<TDto?> GetSingleById<TModel, TDto>(string type, string id)
        where TModel : OpenAlexModel
        where TDto : class
    {
        var value = await _cache.GetAsync<TDto>(id);
        if (value != null)
        {
            return value;
        }

        ISearchResponse<TModel> response = await _client.SearchAsync<TModel>(s => s
            .Index(type).Query(q => q.Ids(i => i.Values(id))));
        if (!response.IsValid)
        {
            throw new SearchException(response.DebugInformation);
        }

        if (response.Documents.Count == 0)
        {
            return null;
        }

        value = _mapper.Map<TModel, TDto>(response.Documents.First());
        await _cache.SetAsync(id, value);

        return value;
    }

    public async Task<List<TDto>> GetManyById<TModel, TDto>(string type, IReadOnlyCollection<string> ids)
        where TModel : OpenAlexModel
        where TDto : class
    {
        if (!ids.Any())
        {
            return [];
        }

        string key = string.Join(",", ids);
        var value = await _cache.GetAsync<List<TDto>>(key);
        if (value != null)
        {
            return value;
        }

        ISearchResponse<TModel> response = await _client.SearchAsync<TModel>(s => s
            .Index(type)
            .Query(q => q.Ids(i => i.Values(ids))));

        if (!response.IsValid)
        {
            throw new SearchException(response.DebugInformation);
        }


        value = response.Documents.Select(_mapper.Map<TModel, TDto>).ToList();
        await _cache.SetAsync(key, value);

        return value;
    }

    public async Task<PagedDto> PreviewStatisticsModel<TModel>(string type, string query, int pageSize, int page)
        where TModel : OpenAlexStatisticsModel
    {
        string key = $"preview:{type}:{query}:{pageSize}:{page}";

        var value = await _cache.GetAsync<PagedDto>(key);
        if (value != null)
        {
            return value;
        }

        ISearchResponse<TModel> response = await _client.SearchAsync<TModel>(s => s
            .Index(type)
            .From(page * pageSize)
            .Size(pageSize)
            .Source(doc => doc.Includes(i => i.Fields(
                f => f.Id,
                f => f.Name,
                f => f.WorksCount,
                f => f.CitationCount,
                f => f.HIndex)))
            .Query(q => q.Match(m => m.Field(f => f.Name)
                .Query(query).Fuzziness(Globals.DefaultFuzziness))));

        if (!response.IsValid)
        {
            throw new SearchException(response.DebugInformation);
        }

        value = new PagedDto(
            response.Total,
            pageSize,
            page,
            response.Documents.Select(_mapper.Map<TModel, DehydratedStatisticsModelDto>).ToList());
        await _cache.SetAsync(key, value);

        return value;
    }

    public async Task<PagedDto> PreviewWork(string type, string query, int pageSize, int page)
    {
        string key = $"preview:{type}:{query}:{pageSize}:{page}";

        var value = await _cache.GetAsync<PagedDto>(key);
        if (value != null)
        {
            return value;
        }

        ISearchResponse<Work> response = await _client.SearchAsync<Work>(s => s
            .Index(type)
            .From(page * pageSize)
            .Size(pageSize)
            .Source(doc => doc.Includes(i => i.Fields(
                f => f.Id,
                f => f.Title,
                f => f.Authors,
                f => f.CitationCount,
                f => f.PublicationYear)))
            .Query(q => q.Match(m => m.Field(f => f.Title)
                .Query(query).Fuzziness(Globals.DefaultFuzziness))));

        if (!response.IsValid)
        {
            throw new SearchException(response.DebugInformation);
        }

        value = new PagedDto(
            response.Total,
            pageSize,
            page,
            response.Documents.Select(_mapper.Map<Work, DehydratedWorkDto>).ToList());
        await _cache.SetAsync(key, value);

        return value;
    }

    public async Task<PagedDto> SearchStatisticsModel<TModel, TDto>(string type, string query, int pageSize, int page)
        where TModel : OpenAlexStatisticsModel
        where TDto : class
    {
        string key = $"search:{type}:{query}:{pageSize}:{page}";
        var value = await _cache.GetAsync<PagedDto>(key);
        if (value != null)
        {
            return value;
        }

        ISearchResponse<TModel> response = await _client.SearchAsync<TModel>(s => s
            .Index(type)
            .From(page * pageSize)
            .Size(pageSize)
            .Query(q => q.Match(m => m.Field(f => f.Name)
                .Query(query).Fuzziness(Globals.DefaultFuzziness))));

        if (!response.IsValid)
        {
            throw new SearchException(response.DebugInformation);
        }

        value = new PagedDto(
            response.Total,
            pageSize,
            page,
            response.Documents.Select(_mapper.Map<TModel, TDto>).ToList());
        await _cache.SetAsync(key, value);

        return value;
    }

    public async Task<PagedDto> SearchWork<TDto>(string type, string query, int pageSize, int page)
        where TDto : class
    {
        string key = $"search:{type}:{query}:{pageSize}:{page}";
        var value = await _cache.GetAsync<PagedDto>(key);
        if (value != null)
        {
            return value;
        }

        ISearchResponse<Work> response = await _client.SearchAsync<Work>(s => s
            .Index(type)
            .From(page * pageSize)
            .Size(pageSize)
            .Query(q => q.Match(m => m.Field(f => f.Title)
                .Query(query).Fuzziness(Globals.DefaultFuzziness))));

        if (!response.IsValid)
        {
            throw new SearchException(response.DebugInformation);
        }

        value = new PagedDto(
            response.Total,
            pageSize,
            page,
            response.Documents.Select(_mapper.Map<Work, TDto>).ToList());
        await _cache.SetAsync(key, value);

        return value;
    }
}