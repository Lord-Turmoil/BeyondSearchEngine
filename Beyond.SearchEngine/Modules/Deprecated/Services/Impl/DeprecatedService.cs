using AutoMapper;
using Beyond.SearchEngine.Extensions.Cache;
using Beyond.SearchEngine.Extensions.Module;
using Beyond.SearchEngine.Modules.Deprecated.Dtos;
using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Search.Models;
using Nest;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Deprecated.Services.Impl;

public class DeprecatedService : ElasticService<DeprecatedService>, IDeprecatedService
{
    private readonly ICacheAdapter _cache;

    public DeprecatedService(IElasticClient client, IMapper mapper, ILogger<DeprecatedService> logger,
        ICacheAdapter cache)
        : base(client, mapper, logger)
    {
        _cache = cache;
    }

    public async Task<ApiResponse> GetAuthorById(string id, bool brief)
    {
        string key = $"deprecated:authors:{id}:{brief}";
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
}