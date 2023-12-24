// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using AutoMapper;
using Beyond.SearchEngine.Extensions.Cache;
using Beyond.SearchEngine.Extensions.Module;
using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Search.Models;
using Beyond.SearchEngine.Modules.Utils;
using Beyond.Shared.Data;
using Beyond.Shared.Dtos;
using Nest;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Services.Impl;

public class AuthorQueryService : ElasticService<AuthorQueryService>, IAuthorQueryService
{
    private const string IndexName = "authors";
    private readonly ICacheAdapter _cache;

    public AuthorQueryService(IElasticClient client, IMapper mapper, ILogger<AuthorQueryService> logger,
        ICacheAdapter cache)
        : base(client, mapper, logger)
    {
        _cache = cache;
    }

    public async Task<ApiResponse> GetWorks(string id, bool brief, int pageSize, int page)
    {
        string key = $"{IndexName}:works:{id}:{brief}:{pageSize}:{page}";
        var value = await _cache.GetAsync<PagedDto>(key);
        if (value != null)
        {
            return new OkResponse(new OkDto(data: value));
        }

        ISearchResponse<Work> response = await _client.SearchAsync<Work>(s => s
            .Index("works")
            .From(page * pageSize)
            .Size(pageSize)
            .Query(q => q.Match(m => m.Field(f => f.Authors).Query(id))));
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
                response.Documents.Select(_mapper.Map<Work, BriefWorkDto>).ToList());
        }
        else
        {
            value = new PagedDto(
                response.Total,
                pageSize,
                page,
                response.Documents.Select(_mapper.Map<Work, WorkDto>).ToList());
        }

        await _cache.SetAsync(key, value);

        return new OkResponse(new OkDto(data: value));
    }

    public async Task<ApiResponse> GetInstitution(string id, bool brief)
    {
        string key = $"{IndexName}:institution:{id}:{brief}";
        object? value = await _cache.GetAsync<object>(key);
        if (value != null)
        {
            return new OkResponse(new OkDto(data: value));
        }

        var agent = new SearchAgent(_client, _mapper, _cache);
        AuthorDto? author = await agent.GetSingleById<Author, AuthorDto>(IndexName, id, brief);
        if (author == null)
        {
            return new NotFoundResponse(new NotFoundDto("No such author"));
        }

        InstitutionData? authorInstitutionData = author.InstitutionData;
        if (authorInstitutionData == null)
        {
            return new NotFoundResponse(new NotFoundDto("Author does not belong to any institution"));
        }


        value = brief
            ? await agent.GetSingleById<Institution, DehydratedStatisticsModelDto>("institutions",
                authorInstitutionData.Id, brief)
            : await agent.GetSingleById<Institution, InstitutionDto>("institutions", authorInstitutionData.Id, brief);
        if (value == null)
        {
            return new NotFoundResponse(new NotFoundDto("No such institution"));
        }

        await _cache.SetAsync(key, value);

        return new OkResponse(new OkDto(data: value));
    }

    public async Task<ApiResponse> GetTopAuthors(int pageSize, int page)
    {
        string key = $"{IndexName}:top:{pageSize}:{page}";
        var value = await _cache.GetAsync<PagedDto>(key);
        if (value != null)
        {
            return new OkResponse(new OkDto(data: value));
        }

        ISearchResponse<Author> response = await _client.SearchAsync<Author>(s => s
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
            response.Documents.Select(_mapper.Map<Author, DehydratedStatisticsModelDto>).ToList()
        );

        await _cache.SetAsync(key, value);

        return new OkResponse(new OkDto(data: value));
    }
}