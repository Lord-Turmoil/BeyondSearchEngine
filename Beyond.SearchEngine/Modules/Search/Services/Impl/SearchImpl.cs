using AutoMapper;
using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Search.Models;
using Beyond.SearchEngine.Modules.Search.Services.Exceptions;
using Beyond.Shared.Dtos;
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

    public SearchImpl(IElasticClient client, IMapper mapper)
    {
        _client = client;
        _mapper = mapper;
    }

    public async Task<TDto?> SearchSingleById<TModel, TDto>(string type, string id)
        where TModel : OpenAlexModel
        where TDto : OpenAlexDto
    {
        ISearchResponse<TModel> response = await _client.SearchAsync<TModel>(s => s
            .Index(type).Query(q => q.Match(m => m.Field(f => f.Id).Query(id))));
        if (!response.IsValid)
        {
            throw new SearchException(response.DebugInformation);
        }

        if (response.Documents.Count == 0)
        {
            return null;
        }

        return _mapper.Map<TModel, TDto>(response.Documents.First());
    }

    public async Task<List<TDto>> SearchManyById<TModel, TDto>(string type, IEnumerable<string> ids)
        where TModel : OpenAlexModel
        where TDto : class
    {
        ISearchResponse<TModel> response = await _client.SearchAsync<TModel>(s => s
            .Index(type)
            .Query(q => q.Bool(b => b.Should(ids.Select(
                id => new Func<QueryContainerDescriptor<TModel>, QueryContainer>(d =>
                    d.Match(m => m.Field(f => f.Id).Query(id))))))));

        if (!response.IsValid)
        {
            throw new SearchException(response.DebugInformation);
        }

        return response.Documents.Select(_mapper.Map<TModel, TDto>).ToList();
    }

    public async Task<PagedDto> PreviewStatisticsModel<TModel>(string type, string query, int pageSize, int page)
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
            .Query(q => q.Match(m => m.Field(f => f.Name)
                .Query(query).Fuzziness(Fuzziness.EditDistance(2)))));

        if (!response.IsValid)
        {
            throw new SearchException(response.DebugInformation);
        }

        var dto = new PagedDto(
            response.Total,
            pageSize,
            page,
            response.Documents.Select(_mapper.Map<TModel, DehydratedStatisticsModelDto>).ToList());
        return dto;
    }

    public async Task<PagedDto> PreviewWork(string type, string query, int pageSize, int page)
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
            .Query(q => q.Match(m => m.Field(f => f.Title)
                .Query(query).Fuzziness(Fuzziness.EditDistance(2)))));

        if (!response.IsValid)
        {
            throw new SearchException(response.DebugInformation);
        }

        var dto = new PagedDto(
            response.Total,
            pageSize,
            page,
            response.Documents.Select(_mapper.Map<Work, DehydratedWorkDto>).ToList());
        return dto;
    }
}