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

public class SimpleSearchService : ElasticService<SimpleSearchService>, ISimpleSearchService
{
    private readonly ICacheAdapter _cache;

    public SimpleSearchService(IElasticClient client, IMapper mapper, ILogger<SimpleSearchService> logger,
        ICacheAdapter cache)
        : base(client, mapper, logger)
    {
        _cache = cache;
    }

    public async Task<ApiResponse> SearchSingle(string type, bool brief, string id)
    {
        var agent = new SearchAgent(_client, _mapper, _cache);

        object? dto;
        if (brief)
        {
            dto = type switch {
                "authors" => await agent.GetSingleById<Author, DehydratedStatisticsModelDto>(type, id, brief),
                "concepts" => await agent.GetSingleById<Concept, DehydratedStatisticsModelDto>(type, id, brief),
                "funders" => await agent.GetSingleById<Funder, DehydratedStatisticsModelDto>(type, id, brief),
                "institutions" => await agent.GetSingleById<Institution, DehydratedStatisticsModelDto>(type, id, brief),
                "publishers" => await agent.GetSingleById<Publisher, DehydratedStatisticsModelDto>(type, id, brief),
                "sources" => await agent.GetSingleById<Source, DehydratedStatisticsModelDto>(type, id, brief),
                "works" => await agent.GetSingleById<Work, DehydratedWorkDto>(type, id, brief),
                _ => null
            };
        }
        else
        {
            dto = type switch {
                "authors" => await agent.GetSingleById<Author, AuthorDto>(type, id, brief),
                "concepts" => await agent.GetSingleById<Concept, ConceptDto>(type, id, brief),
                "funders" => await agent.GetSingleById<Funder, FunderDto>(type, id, brief),
                "institutions" => await agent.GetSingleById<Institution, InstitutionDto>(type, id, brief),
                "publishers" => await agent.GetSingleById<Publisher, PublisherDto>(type, id, brief),
                "sources" => await agent.GetSingleById<Source, SourceDto>(type, id, brief),
                "works" => await agent.GetSingleById<Work, WorkDto>(type, id, brief),
                _ => null
            };
        }

        if (dto == null)
        {
            return new NotFoundResponse(new NotFoundDto());
        }

        return new OkResponse(new OkDto(data: dto));
    }

    public async Task<ApiResponse> SearchMany(string type, bool brief, IReadOnlyCollection<string> idList)
    {
        var agent = new SearchAgent(_client, _mapper, _cache);

        IEnumerable<object> dto;
        if (brief)
        {
            dto = type switch {
                "authors" => await agent.GetManyById<Author, DehydratedStatisticsModelDto>(type, idList, brief),
                "concepts" => await agent.GetManyById<Concept, DehydratedStatisticsModelDto>(type, idList, brief),
                "funders" => await agent.GetManyById<Funder, DehydratedStatisticsModelDto>(type, idList, brief),
                "institutions" => await agent.GetManyById<Institution, DehydratedStatisticsModelDto>(type, idList, brief),
                "publishers" => await agent.GetManyById<Publisher, DehydratedStatisticsModelDto>(type, idList, brief),
                "sources" => await agent.GetManyById<Source, DehydratedStatisticsModelDto>(type, idList, brief),
                "works" => await agent.GetManyById<Work, DehydratedWorkDto>(type, idList, brief),
                _ => []
            };
        }
        else
        {
            dto = type switch {
                "authors" => await agent.GetManyById<Author, AuthorDto>(type, idList, brief),
                "concepts" => await agent.GetManyById<Concept, ConceptDto>(type, idList, brief),
                "funders" => await agent.GetManyById<Funder, FunderDto>(type, idList, brief),
                "institutions" => await agent.GetManyById<Institution, InstitutionDto>(type, idList, brief),
                "publishers" => await agent.GetManyById<Publisher, PublisherDto>(type, idList, brief),
                "sources" => await agent.GetManyById<Source, SourceDto>(type, idList, brief),
                "works" => await agent.GetManyById<Work, WorkDto>(type, idList, brief),
                _ => []
            };
        }

        return new OkResponse(new OkDto(data: dto));
    }

    public async Task<ApiResponse> Preview(string type, string query, int pageSize, int page)
    {
        var agent = new SearchAgent(_client, _mapper, _cache);

        PagedDto? dto = type switch {
            "authors" => await agent.PreviewStatisticsModel<Author>(type, query, pageSize, page),
            "concepts" => await agent.PreviewStatisticsModel<Concept>(type, query, pageSize, page),
            "funders" => await agent.PreviewStatisticsModel<Funder>(type, query, pageSize, page),
            "institutions" => await agent.PreviewStatisticsModel<Institution>(type, query, pageSize, page),
            "publishers" => await agent.PreviewStatisticsModel<Publisher>(type, query, pageSize, page),
            "sources" => await agent.PreviewStatisticsModel<Source>(type, query, pageSize, page),
            "works" => await agent.PreviewWork(type, query, pageSize, page),
            _ => null
        };

        if (dto == null)
        {
            return new NotFoundResponse(new NotFoundDto());
        }

        return new OkResponse(new OkDto(data: dto));
    }

    public async Task<ApiResponse> Search(string type, string query, bool brief, int pageSize, int page)
    {
        var agent = new SearchAgent(_client, _mapper, _cache);

        PagedDto? dto;
        if (brief)
        {
            dto = type switch {
                "authors" => await agent.SearchStatisticsModel<Author, DehydratedStatisticsModelDto>(type, query, pageSize, page),
                "concepts" => await agent.SearchStatisticsModel<Concept, DehydratedStatisticsModelDto>(type, query, pageSize, page),
                "funders" => await agent.SearchStatisticsModel<Funder, DehydratedStatisticsModelDto>(type, query, pageSize, page),
                "institutions" => await agent.SearchStatisticsModel<Institution, DehydratedStatisticsModelDto>(type, query, pageSize, page),
                "publishers" => await agent.SearchStatisticsModel<Publisher, DehydratedStatisticsModelDto>(type, query, pageSize, page),
                "sources" => await agent.SearchStatisticsModel<Source, DehydratedStatisticsModelDto>(type, query, pageSize, page),
                "works" => await agent.SearchWork<DehydratedWorkDto>(type, query, pageSize, page),
                _ => null
            };
        }
        else
        {
            dto = type switch {
                "authors" => await agent.SearchStatisticsModel<Author, AuthorDto>(type, query, pageSize, page),
                "concepts" => await agent.SearchStatisticsModel<Concept, ConceptDto>(type, query, pageSize, page),
                "funders" => await agent.SearchStatisticsModel<Funder, FunderDto>(type, query, pageSize, page),
                "institutions" => await agent.SearchStatisticsModel<Institution, InstitutionDto>(type, query, pageSize, page),
                "publishers" => await agent.SearchStatisticsModel<Publisher, PublisherDto>(type, query, pageSize, page),
                "sources" => await agent.SearchStatisticsModel<Source, SourceDto>(type, query, pageSize, page),
                "works" => await agent.SearchWork<WorkDto>(type, query, pageSize, page),
                _ => null
            };
        }
        if (dto == null)
        {
            return new NotFoundResponse(new NotFoundDto());
        }

        return new OkResponse(new OkDto(data: dto));
    }
}