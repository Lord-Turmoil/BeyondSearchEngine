// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using AutoMapper;
using Beyond.SearchEngine.Extensions.Cache;
using Beyond.SearchEngine.Extensions.Module;
using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Search.Models;
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
        var impl = new SearchImpl(_client, _mapper, _cache);

        object? dto;
        if (brief)
        {
            dto = type switch {
                "authors" => await impl.GetSingleById<Author, DehydratedStatisticsModelDto>(type, id, brief),
                "concepts" => await impl.GetSingleById<Concept, DehydratedStatisticsModelDto>(type, id, brief),
                "funders" => await impl.GetSingleById<Funder, DehydratedStatisticsModelDto>(type, id, brief),
                "institutions" => await impl.GetSingleById<Institution, DehydratedStatisticsModelDto>(type, id, brief),
                "publishers" => await impl.GetSingleById<Publisher, DehydratedStatisticsModelDto>(type, id, brief),
                "sources" => await impl.GetSingleById<Source, DehydratedStatisticsModelDto>(type, id, brief),
                "works" => await impl.GetSingleById<Work, DehydratedWorkDto>(type, id, brief),
                _ => null
            };
        }
        else
        {
            dto = type switch {
                "authors" => await impl.GetSingleById<Author, AuthorDto>(type, id, brief),
                "concepts" => await impl.GetSingleById<Concept, ConceptDto>(type, id, brief),
                "funders" => await impl.GetSingleById<Funder, FunderDto>(type, id, brief),
                "institutions" => await impl.GetSingleById<Institution, InstitutionDto>(type, id, brief),
                "publishers" => await impl.GetSingleById<Publisher, PublisherDto>(type, id, brief),
                "sources" => await impl.GetSingleById<Source, SourceDto>(type, id, brief),
                "works" => await impl.GetSingleById<Work, WorkDto>(type, id, brief),
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
        var impl = new SearchImpl(_client, _mapper, _cache);

        IEnumerable<object> dto;
        if (brief)
        {
            dto = type switch {
                "authors" => await impl.GetManyById<Author, DehydratedStatisticsModelDto>(type, idList, brief),
                "concepts" => await impl.GetManyById<Concept, DehydratedStatisticsModelDto>(type, idList, brief),
                "funders" => await impl.GetManyById<Funder, DehydratedStatisticsModelDto>(type, idList, brief),
                "institutions" => await impl.GetManyById<Institution, DehydratedStatisticsModelDto>(type, idList, brief),
                "publishers" => await impl.GetManyById<Publisher, DehydratedStatisticsModelDto>(type, idList, brief),
                "sources" => await impl.GetManyById<Source, DehydratedStatisticsModelDto>(type, idList, brief),
                "works" => await impl.GetManyById<Work, DehydratedWorkDto>(type, idList, brief),
                _ => []
            };
        }
        else
        {
            dto = type switch {
                "authors" => await impl.GetManyById<Author, AuthorDto>(type, idList, brief),
                "concepts" => await impl.GetManyById<Concept, ConceptDto>(type, idList, brief),
                "funders" => await impl.GetManyById<Funder, FunderDto>(type, idList, brief),
                "institutions" => await impl.GetManyById<Institution, InstitutionDto>(type, idList, brief),
                "publishers" => await impl.GetManyById<Publisher, PublisherDto>(type, idList, brief),
                "sources" => await impl.GetManyById<Source, SourceDto>(type, idList, brief),
                "works" => await impl.GetManyById<Work, WorkDto>(type, idList, brief),
                _ => []
            };
        }

        return new OkResponse(new OkDto(data: dto));
    }

    public async Task<ApiResponse> Preview(string type, string query, int pageSize, int page)
    {
        var impl = new SearchImpl(_client, _mapper, _cache);

        PagedDto? dto = type switch {
            "authors" => await impl.PreviewStatisticsModel<Author>(type, query, pageSize, page),
            "concepts" => await impl.PreviewStatisticsModel<Concept>(type, query, pageSize, page),
            "funders" => await impl.PreviewStatisticsModel<Funder>(type, query, pageSize, page),
            "institutions" => await impl.PreviewStatisticsModel<Institution>(type, query, pageSize, page),
            "publishers" => await impl.PreviewStatisticsModel<Publisher>(type, query, pageSize, page),
            "sources" => await impl.PreviewStatisticsModel<Source>(type, query, pageSize, page),
            "works" => await impl.PreviewWork(type, query, pageSize, page),
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
        var impl = new SearchImpl(_client, _mapper, _cache);

        PagedDto? dto;
        if (brief)
        {
            dto = type switch {
                "authors" => await impl.SearchStatisticsModel<Author, DehydratedStatisticsModelDto>(type, query, pageSize, page),
                "concepts" => await impl.SearchStatisticsModel<Concept, DehydratedStatisticsModelDto>(type, query, pageSize, page),
                "funders" => await impl.SearchStatisticsModel<Funder, DehydratedStatisticsModelDto>(type, query, pageSize, page),
                "institutions" => await impl.SearchStatisticsModel<Institution, DehydratedStatisticsModelDto>(type, query, pageSize, page),
                "publishers" => await impl.SearchStatisticsModel<Publisher, DehydratedStatisticsModelDto>(type, query, pageSize, page),
                "sources" => await impl.SearchStatisticsModel<Source, DehydratedStatisticsModelDto>(type, query, pageSize, page),
                "works" => await impl.SearchWork<DehydratedWorkDto>(type, query, pageSize, page),
                _ => null
            };
        }
        else
        {
            dto = type switch {
                "authors" => await impl.SearchStatisticsModel<Author, AuthorDto>(type, query, pageSize, page),
                "concepts" => await impl.SearchStatisticsModel<Concept, ConceptDto>(type, query, pageSize, page),
                "funders" => await impl.SearchStatisticsModel<Funder, FunderDto>(type, query, pageSize, page),
                "institutions" => await impl.SearchStatisticsModel<Institution, InstitutionDto>(type, query, pageSize, page),
                "publishers" => await impl.SearchStatisticsModel<Publisher, PublisherDto>(type, query, pageSize, page),
                "sources" => await impl.SearchStatisticsModel<Source, SourceDto>(type, query, pageSize, page),
                "works" => await impl.SearchWork<WorkDto>(type, query, pageSize, page),
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