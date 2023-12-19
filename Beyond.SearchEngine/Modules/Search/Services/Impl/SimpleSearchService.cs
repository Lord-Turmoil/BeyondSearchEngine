using AutoMapper;
using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Search.Models;
using Beyond.Shared.Dtos;
using Nest;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Services.Impl;

public class SimpleSearchService : ElasticService<SimpleSearchService>, ISimpleSearchService
{
    public SimpleSearchService(IElasticClient client, IMapper mapper, ILogger<SimpleSearchService> logger)
        : base(client, mapper, logger)
    {
    }

    public async Task<ApiResponse> SearchSingle(string type, string id)
    {
        var impl = new SearchImpl<SimpleSearchService>(_client, _mapper);

        OpenAlexDto? dto = type switch {
            "authors" => await impl.SearchSingleById<Author, AuthorDto>(type, id),
            "concepts" => await impl.SearchSingleById<Concept, ConceptDto>(type, id),
            "funders" => await impl.SearchSingleById<Funder, FunderDto>(type, id),
            "institutions" => await impl.SearchSingleById<Institution, InstitutionDto>(type, id),
            "publishers" => await impl.SearchSingleById<Publisher, PublisherDto>(type, id),
            "sources" => await impl.SearchSingleById<Source, SourceDto>(type, id),
            "works" => await impl.SearchSingleById<Work, WorkDto>(type, id),
            _ => null
        };

        if (dto == null)
        {
            return new NotFoundResponse(new NotFoundDto());
        }

        return new OkResponse(new OkDto(data: dto));
    }

    public async Task<ApiResponse> SearchMany(string type, IEnumerable<string> ids)
    {
        var impl = new SearchImpl<SimpleSearchService>(_client, _mapper);

        IEnumerable<OpenAlexDto> dto = type switch {
            "authors" => await impl.SearchManyById<Author, AuthorDto>(type, ids),
            "concepts" => await impl.SearchManyById<Concept, ConceptDto>(type, ids),
            "funders" => await impl.SearchManyById<Funder, FunderDto>(type, ids),
            "institutions" => await impl.SearchManyById<Institution, InstitutionDto>(type, ids),
            "publishers" => await impl.SearchManyById<Publisher, PublisherDto>(type, ids),
            "sources" => await impl.SearchManyById<Source, SourceDto>(type, ids),
            "works" => await impl.SearchManyById<Work, WorkDto>(type, ids),
            _ => []
        };

        return new OkResponse(new OkDto(data: dto));
    }

    public async Task<ApiResponse> Preview(string type, string query, int pageSize, int page)
    {
        var impl = new SearchImpl<SimpleSearchService>(_client, _mapper);

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
}