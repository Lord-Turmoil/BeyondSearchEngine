// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using AutoMapper;
using Beyond.SearchEngine.Extensions.Cache;
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
                "authors" => await impl.GetSingleById<Author, DehydratedStatisticsModelDto>(type, id),
                "concepts" => await impl.GetSingleById<Concept, DehydratedStatisticsModelDto>(type, id),
                "funders" => await impl.GetSingleById<Funder, DehydratedStatisticsModelDto>(type, id),
                "institutions" => await impl.GetSingleById<Institution, DehydratedStatisticsModelDto>(type, id),
                "publishers" => await impl.GetSingleById<Publisher, DehydratedStatisticsModelDto>(type, id),
                "sources" => await impl.GetSingleById<Source, DehydratedStatisticsModelDto>(type, id),
                "works" => await impl.GetSingleById<Work, DehydratedWorkDto>(type, id),
                _ => null
            };
        }
        else
        {
            dto = type switch {
                "authors" => await impl.GetSingleById<Author, AuthorDto>(type, id),
                "concepts" => await impl.GetSingleById<Concept, ConceptDto>(type, id),
                "funders" => await impl.GetSingleById<Funder, FunderDto>(type, id),
                "institutions" => await impl.GetSingleById<Institution, InstitutionDto>(type, id),
                "publishers" => await impl.GetSingleById<Publisher, PublisherDto>(type, id),
                "sources" => await impl.GetSingleById<Source, SourceDto>(type, id),
                "works" => await impl.GetSingleById<Work, WorkDto>(type, id),
                _ => null
            };
        }

        if (dto == null)
        {
            return new NotFoundResponse(new NotFoundDto());
        }

        return new OkResponse(new OkDto(data: dto));
    }

    public async Task<ApiResponse> SearchMany(string type, bool brief, IReadOnlyCollection<string> ids)
    {
        var impl = new SearchImpl(_client, _mapper, _cache);

        IEnumerable<object> dto;
        if (brief)
        {
            dto = type switch {
                "authors" => await impl.GetManyById<Author, DehydratedStatisticsModelDto>(type, ids),
                "concepts" => await impl.GetManyById<Concept, DehydratedStatisticsModelDto>(type, ids),
                "funders" => await impl.GetManyById<Funder, DehydratedStatisticsModelDto>(type, ids),
                "institutions" => await impl.GetManyById<Institution, DehydratedStatisticsModelDto>(type, ids),
                "publishers" => await impl.GetManyById<Publisher, DehydratedStatisticsModelDto>(type, ids),
                "sources" => await impl.GetManyById<Source, DehydratedStatisticsModelDto>(type, ids),
                "works" => await impl.GetManyById<Work, DehydratedWorkDto>(type, ids),
                _ => []
            };
        }
        else
        {
            dto = type switch {
                "authors" => await impl.GetManyById<Author, AuthorDto>(type, ids),
                "concepts" => await impl.GetManyById<Concept, ConceptDto>(type, ids),
                "funders" => await impl.GetManyById<Funder, FunderDto>(type, ids),
                "institutions" => await impl.GetManyById<Institution, InstitutionDto>(type, ids),
                "publishers" => await impl.GetManyById<Publisher, PublisherDto>(type, ids),
                "sources" => await impl.GetManyById<Source, SourceDto>(type, ids),
                "works" => await impl.GetManyById<Work, WorkDto>(type, ids),
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