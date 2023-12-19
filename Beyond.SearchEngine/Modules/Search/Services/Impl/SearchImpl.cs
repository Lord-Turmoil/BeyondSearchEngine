using AutoMapper;
using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Search.Models;
using Beyond.Shared.Dtos;
using MySqlX.XDevAPI;
using Nest;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Services.Impl;

/// <summary>
///     Fundamental implementation for searching items.
/// </summary>
public class SearchImpl<TService>
{
    private readonly IElasticClient _client;
    private readonly IMapper _mapper;
    private readonly ILogger<TService> _logger;

    public SearchImpl(IElasticClient client, IMapper mapper, ILogger<TService> logger)
    {
        _client = client;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<ApiResponse> SearchSingleById<TModel, TDto>(string type, string id)
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

    public async Task<ApiResponse> SearchManyById<TModel, TDto>(string type, IEnumerable<string> ids)
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

    public async Task<ApiResponse> PreviewStatisticsModel<TModel>(string type, string query, int pageSize, int page)
        where TModel : OpenAlexStatisticsModel
    {
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
            .Query(q => q.Match(m => m.Field(f => f.Name).Query(query))));

        if (!response.IsValid)
        {
            _logger.LogError(response.DebugInformation);
            return new InternalServerErrorResponse(new SearchFailedDto());
        }

        var dto = new PagedDto(
            response.HitsMetadata.Total.Value,
            pageSize,
            page,
            response.Documents.Select(_mapper.Map<TModel, DehydratedStatisticsModelDto>).ToList());
        return new OkResponse(new OkDto(data: dto));
    }

    public async Task<ApiResponse> PreviewWork(string type, string query, int pageSize, int page)
    {
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
            .Query(q => q.Match(m => m.Field(f => f.Title).Query(query))));

        if (!response.IsValid)
        {
            _logger.LogError(response.DebugInformation);
            return new InternalServerErrorResponse(new SearchFailedDto());
        }

        var dto = new PagedDto(
            response.HitsMetadata.Total.Value,
            pageSize,
            page,
            response.Documents.Select(_mapper.Map<Work, DehydratedWorkDto>).ToList());
        return new OkResponse(new OkDto(data: dto));
    }
}