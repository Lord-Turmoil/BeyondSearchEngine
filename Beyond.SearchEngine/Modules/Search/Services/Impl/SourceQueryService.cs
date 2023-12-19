using AutoMapper;
using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Search.Models;
using Beyond.Shared.Dtos;
using Nest;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Services.Impl;

public class SourceQueryService : ElasticService<SourceQueryService>, ISourceQueryService
{
    private const string IndexName = "sources";
    protected SourceQueryService(IElasticClient client, IMapper mapper, ILogger<SourceQueryService> logger)
        : base(client, mapper, logger)
    {
    }

    public async Task<ApiResponse> GetAll(int pageSize, int page)
    {
        ISearchResponse<Source> response = await _client.SearchAsync<Source>(s => s.Index(IndexName)
            .From(page * pageSize)
            .Size(pageSize)
            .Query(q => q.MatchAll()));

        if (!response.IsValid)
        {
            _logger.LogError(response.DebugInformation);
            return new InternalServerErrorResponse(new SearchFailedDto());
        }

        var dto = new PagedDto(
            response.Total,
            pageSize,
            page,
            response.Documents.Select(_mapper.Map<Source, SourceDto>));
        return new OkResponse(new OkDto(data: dto));
    }

    public async Task<ApiResponse> GetHost(string id)
    {
        var impl = new SearchImpl<SourceQueryService>(_client, _mapper);

        SourceDto? dto = await impl.SearchSingleById<Source, SourceDto>(IndexName, id);
        if (dto == null || string.IsNullOrEmpty(dto.HostId))
        {
            return new NotFoundResponse(new NotFoundDto());
        }

        OpenAlexDto? data;
        if (dto.HostId.StartsWith("I"))
        {
            data = await impl.SearchSingleById<Institution, InstitutionDto>("institutions", dto.HostId);
        }
        else
        {
            data = await impl.SearchSingleById<Publisher, PublisherDto>("publishers", dto.HostId);
        }
        if (data == null)
        {
            return new NotFoundResponse(new NotFoundDto());
        }

        return new OkResponse(new OkDto(data: data));
    }
}