using AutoMapper;
using Beyond.SearchEngine.Modules.Search.Models;
using Beyond.Shared.Dtos;
using Nest;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Services;

public class SimpleSearchService : ElasticService<SimpleSearchService>, ISimpleSearchService
{
    public SimpleSearchService(IElasticClient client, IMapper mapper, ILogger<SimpleSearchService> logger)
        : base(client, mapper, logger)
    {
    }

    public async Task<ApiResponse> SearchSingleAsync(string type, string id)
    {
        return type switch {
            "authors" => await SearchSingleImpl<Author, AuthorDto>(type, id),
            "concepts" => await SearchSingleImpl<Concept, ConceptDto>(type, id),
            "funders" => await SearchSingleImpl<Funder, FunderDto>(type, id),
            "institutions" => await SearchSingleImpl<Institution, InstitutionDto>(type, id),
            "publishers" => await SearchSingleImpl<Publisher, PublisherDto>(type, id),
            "sources" => await SearchSingleImpl<Source, SourceDto>(type, id),
            "works" => await SearchSingleImpl<Work, WorkDto>(type, id),
            _ => new BadRequestResponse(new BadRequestDto($"Invalid type {type}"))
        };
    }

    public async Task<ApiResponse> SearchManyAsync(string type, IEnumerable<string> ids)
    {
        return type switch {
            "authors" => await SearchManyImpl<Author, AuthorDto>(type, ids),
            "concepts" => await SearchManyImpl<Concept, ConceptDto>(type, ids),
            "funders" => await SearchManyImpl<Funder, FunderDto>(type, ids),
            "institutions" => await SearchManyImpl<Institution, InstitutionDto>(type, ids),
            "publishers" => await SearchManyImpl<Publisher, PublisherDto>(type, ids),
            "sources" => await SearchManyImpl<Source, SourceDto>(type, ids),
            "works" => await SearchManyImpl<Work, WorkDto>(type, ids),
            _ => new BadRequestResponse(new BadRequestDto($"Invalid type {type}"))
        };
    }

    private async Task<ApiResponse> SearchSingleImpl<TModel, TDto>(string type, string id)
        where TModel : OpenAlexModel
        where TDto : OpenAlexDto
    {
        ISearchResponse<TModel> response = await _client.SearchAsync<TModel>(s => s
            .Index(type).Query(q => q.Match(m => m.Field(f => f.Id).Query(id))));
        if (!response.IsValid)
        {
            _logger.LogError(response.DebugInformation);
            return new InternalServerErrorResponse(new InternalServerErrorDto());
        }

        if (response.Documents.Count == 0)
        {
            return new NotFoundResponse(new NotFoundDto($"{type} with ID {id} not found."));
        }

        TDto dto = _mapper.Map<TModel, TDto>(response.Documents.First());
        return new OkResponse(new OkDto(data: dto));
    }

    private async Task<ApiResponse> SearchManyImpl<TModel, TDto>(string type, IEnumerable<string> ids)
        where TModel : OpenAlexModel
        where TDto : OpenAlexDto
    {
        ISearchResponse<TModel> response = await _client.SearchAsync<TModel>(s => s
            .Index(type)
            .Query(q => q.Bool(b => b.Should(ids.Select(
                id => new Func<QueryContainerDescriptor<TModel>, QueryContainer>(d =>
                    d.Match(m => m.Field(f => f.Id).Query(id))))))));

        if (!response.IsValid)
        {
            _logger.LogError(response.DebugInformation);
            return new InternalServerErrorResponse(new InternalServerErrorDto());
        }

        List<TDto> dtos = response.Documents.Select(_mapper.Map<TModel, TDto>).ToList();
        return new OkResponse(new OkDto(data: dtos));
    }
}