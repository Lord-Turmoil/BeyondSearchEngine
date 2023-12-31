﻿// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using System.Text;
using AutoMapper;
using Beyond.SearchEngine.Extensions.Cache;
using Beyond.SearchEngine.Extensions.Module;
using Beyond.SearchEngine.Modules.Deprecated.Dtos;
using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Search.Models;
using Beyond.SearchEngine.Modules.Utils;
using Beyond.Shared.Dtos;
using Nest;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Deprecated.Services.Impl;

public class DeprecatedService : ElasticService<DeprecatedService>, IDeprecatedService
{
    private readonly ICacheAdapter _cache;

    private readonly string[] CitationTypes = ["bibtex", "endnote", "acm"];

    public DeprecatedService(IElasticClient client, IMapper mapper, ILogger<DeprecatedService> logger,
        ICacheAdapter cache)
        : base(client, mapper, logger)
    {
        _cache = cache;
    }

    public async Task<ApiResponse> GetAuthorById(string id, bool brief)
    {
        string key = $"d:authors:{id}:{brief}";
        object? value = await _cache.GetAsync<object>(key);
        if (value != null)
        {
            return new OkResponse(new OkDto(data: value));
        }

        ISearchResponse<Author> response = await _client.SearchAsync<Author>(s => s.Index("authors")
            .Query(q => q.Ids(i => i.Values(id))));
        if (!response.IsValid)
        {
            _logger.LogError(response.DebugInformation);
            return new InternalServerErrorResponse(new SearchFailedDto());
        }

        if (response.Documents.Count == 0)
        {
            return new NotFoundResponse(new NotFoundDto());
        }

        value = brief
            ? _mapper.Map<Author, DehydratedStatisticsModelDto>(response.Documents.First())
            : _mapper.Map<Author, DeprecatedAuthorDto>(response.Documents.First());

        await _cache.SetAsync(key, value);

        return new OkResponse(new OkDto(data: value));
    }

    public async Task<ApiResponse> GetWorksOfAuthor(string id, bool brief, int pageSize, int page)
    {
        string key = $"d:authors:works:{id}:{brief}:{pageSize}:{page}";
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
                response.Documents.Select(_mapper.Map<Work, DeprecatedBriefWorkDto>).ToList());
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

    public async Task<ApiResponse> GetWorkById(string id, bool brief)
    {
        string key = $"d:works:{id}:{brief}";
        object? value = await _cache.GetAsync<object>(key);
        if (value != null)
        {
            return new OkResponse(new OkDto(data: value));
        }

        ISearchResponse<Work> response = await _client.SearchAsync<Work>(s => s
            .Index("works")
            .Query(q => q.Ids(i => i.Values(id))));
        if (!response.IsValid)
        {
            _logger.LogError(response.DebugInformation);
            return new InternalServerErrorResponse(new SearchFailedDto());
        }

        if (response.Documents.Count == 0)
        {
            return new NotFoundResponse(new NotFoundDto());
        }

        if (brief)
        {
            value = _mapper.Map<Work, DeprecatedBriefWorkDto>(response.Documents.First());
        }
        else
        {
            DeprecatedWorkDto dto = _mapper.Map<Work, DeprecatedWorkDto>(response.Documents.First());
            dto.Mock();
            value = dto;
        }

        await _cache.SetAsync(key, value);

        return new OkResponse(new OkDto(data: value));
    }

    public async Task<ApiResponse> GetWorkCitations(IReadOnlyCollection<string> idList)
    {
        bool[] first = [true, true, true];
        StringBuilder[] builder = [new StringBuilder(), new StringBuilder(), new StringBuilder()];
        var agent = new SearchAgent(_client, _mapper, _cache);

        foreach (string id in idList)
        {
            if (string.IsNullOrEmpty(id))
            {
                continue;
            }

            var work = await agent.GetModelById<Work>("works", id);
            if (work == null)
            {
                // Warning: We swallow this error.
                continue;
            }

            for (int i = 0; i < 3; i++)
            {
                string type = CitationTypes[i];
                string key = $"d:works:citation:{type}:{id}";
                string? citation = await _cache.GetStringAsync(key);
                if (citation != null)
                {
                    builder[i].AppendLine(citation);
                    continue;
                }

                // Each citation is guaranteed to have a line separator.
                citation = CitationGenerator.GenerateCitation(type, work);
                if (citation == null)
                {
                    return new BadRequestResponse(new BadRequestDto("Invalid citation type"));
                }

                if (first[i])
                {
                    first[i] = false;
                }
                else
                {
                    builder[i].AppendLine();
                }

                builder[i].Append(citation);
            }
        }

        var dto = new DeprecatedCitationDto {
            Bibtex = builder[0].ToString(),
            Endnote = builder[1].ToString(),
            Acm = builder[2].ToString()
        };

        return new OkResponse(new OkDto(data: dto));
    }
}