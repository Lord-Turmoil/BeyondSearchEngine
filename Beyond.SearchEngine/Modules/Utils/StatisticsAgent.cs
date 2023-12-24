// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using AutoMapper;
using Beyond.SearchEngine.Extensions.Cache;
using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Search.Models;
using Beyond.SearchEngine.Modules.Search.Services.Exceptions;
using Beyond.Shared.Dtos;
using Nest;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Utils;

public class StatisticsAgent
{
    private readonly ICacheAdapter _cache;
    private readonly IElasticClient _client;
    private readonly IMapper _mapper;

    public StatisticsAgent(IElasticClient client, IMapper mapper, ICacheAdapter cache)
    {
        _client = client;
        _mapper = mapper;
        _cache = cache;
    }

    public async Task<PagedDto> GetTopModels<TModel, TDto>(string type, int pageSize, int page)
        where TModel : OpenAlexStatisticsModel
        where TDto : class
    {
        string key = $"{type}:top:{pageSize}:{page}";
        var value = await _cache.GetAsync<PagedDto>(key);
        if (value != null)
        {
            return value;
        }

        ISearchResponse<TModel> response = await _client.SearchAsync<TModel>(s => s
            .Index(type)
            .From(page * pageSize)
            .Size(pageSize)
            .Sort(ss => ss.Field(f => f.HIndex, SortOrder.Descending))
            .Query(q => q.MatchAll()));

        if (!response.IsValid)
        {
            throw new SearchException(response.DebugInformation);
        }

        value = new PagedDto(
            response.Total,
            pageSize,
            page,
            response.Documents.Select(_mapper.Map<TModel, TDto>).ToList()
        );

        await _cache.SetAsync(key, value);

        return value;
    }
}