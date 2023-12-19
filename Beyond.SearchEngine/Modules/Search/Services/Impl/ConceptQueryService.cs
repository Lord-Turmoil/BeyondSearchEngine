using AutoMapper;
using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Search.Models;
using Nest;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Services.Impl;

public class ConceptQueryService : ElasticService<ConceptQueryService>, IConceptQueryService
{
    private const string IndexName = "concepts";

    public ConceptQueryService(IElasticClient client, IMapper mapper, ILogger<ConceptQueryService> logger)
        : base(client, mapper, logger)
    {
    }

    public async Task<ApiResponse> GetAllWithPrefix(string prefix, int pageSize, int page)
    {
        ISearchResponse<Concept> response = await _client.SearchAsync<Concept>(s => s
            .Index(IndexName)
            .From(page * pageSize)
            .Size(pageSize)
            .Query(q => q.MatchPhrasePrefix(m => m.Field(f => f.Name).Query(prefix))));
        if (!response.IsValid)
        {
            _logger.LogError(response.DebugInformation);
            return new InternalServerErrorResponse(new SearchFailedDto());
        }

        var dto = new PagedDto(
            response.HitsMetadata.Total.Value,
            pageSize,
            page,
            response.Documents.Select(_mapper.Map<Concept, DehydratedStatisticsModelDto>).ToList()
        );

        return new OkResponse(new OkDto(data: dto));
    }
}