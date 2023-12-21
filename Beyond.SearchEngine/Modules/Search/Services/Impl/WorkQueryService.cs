// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using System.Linq.Expressions;
using AutoMapper;
using Beyond.SearchEngine.Extensions.Cache;
using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Search.Models;
using Beyond.SearchEngine.Modules.Search.Services.Exceptions;
using Beyond.Shared.Dtos;
using Nest;
using Newtonsoft.Json;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Services.Impl;

public class WorkQueryService : ElasticService<WorkQueryService>, IWorkQueryService
{
    private const string IndexName = "works";
    private readonly ICacheAdapter _cache;

    public WorkQueryService(IElasticClient client, IMapper mapper, ILogger<WorkQueryService> logger,
        ICacheAdapter cache)
        : base(client, mapper, logger)
    {
        _cache = cache;
    }

    public async Task<ApiResponse> GetRelatedWorks(string id, bool brief)
    {
        string key = $"work:related:{id}:{brief}";
        var value = await _cache.GetAsync<IEnumerable<object>>(key);
        if (value != null)
        {
            return new OkResponse(new OkDto(data: value));
        }

        var impl = new SearchImpl(_client, _mapper, _cache);

        WorkDto? dto = await impl.SearchSingleById<Work, WorkDto>(IndexName, id);
        if (dto == null)
        {
            return new NotFoundResponse(new NotFoundDto());
        }

        if (brief)
        {
            value = await impl.SearchManyById<Work, BriefWorkDto>(
                IndexName, dto.RelatedWorkList);
        }
        else
        {
            value = await impl.SearchManyById<Work, WorkDto>(
                IndexName, dto.RelatedWorkList);
        }

        await _cache.SetAsync(key, value);

        return new OkResponse(new OkDto(data: value));
    }

    public async Task<ApiResponse> GetReferencedWorks(string id, bool brief)
    {
        string key = $"work:ref:{id}:{brief}";
        var value = await _cache.GetAsync<IEnumerable<object>>(key);
        if (value != null)
        {
            return new OkResponse(new OkDto(data: value));
        }

        var impl = new SearchImpl(_client, _mapper, _cache);

        WorkDto? dto = await impl.SearchSingleById<Work, WorkDto>(IndexName, id);
        if (dto == null)
        {
            return new NotFoundResponse(new NotFoundDto());
        }

        if (brief)
        {
            value = await impl.SearchManyById<Work, BriefWorkDto>(
                IndexName, dto.ReferencedWorkList);
        }
        else
        {
            value = await impl.SearchManyById<Work, WorkDto>(
                IndexName, dto.ReferencedWorkList);
        }

        await _cache.SetAsync(key, value);

        return new OkResponse(new OkDto(data: value));
    }

    public async Task<ApiResponse> QueryWorksBasic(QueryWorkBasicDto dto)
    {
        string key = $"work:query:{JsonConvert.SerializeObject(dto)}";
        var value = await _cache.GetAsync<PagedDto>(key);
        if (value != null)
        {
            return new OkResponse(new OkDto(data: value));
        }

        value = await QueryWorksBasicImpl(dto);
        await _cache.SetAsync(key, value);

        return new OkResponse(new OkDto(data: value));
    }

    public async Task<ApiResponse> QueryWorksAdvanced(QueryWorkAdvancedDto dto)
    {
        string key = $"work:query:{JsonConvert.SerializeObject(dto)}";
        var value = await _cache.GetAsync<PagedDto>(key);
        if (value != null)
        {
            return new OkResponse(new OkDto(data: value));
        }

        value = await QueryWorksAdvancedImpl(dto);
        await _cache.SetAsync(key, value);

        return new OkResponse(new OkDto(data: value));
    }

    private async Task<PagedDto> QueryWorksBasicImpl(QueryWorkBasicDto dto)
    {
        var container = new QueryContainer();

        foreach (BasicCondition cond in dto.Conditions)
        {
            Expression<Func<Work, string>>? field = GetField(cond.Field);
            if (field == null)
            {
                throw new SearchException($"Invalid field: {cond.Field}");
            }

            container &= new QueryContainerDescriptor<Work>()
                .Match(m => m.Field(field).Query(cond.Value)
                    .Fuzziness(Globals.DefaultFuzziness));
        }

        if (dto.TimeRange != null)
        {
            container &= new QueryContainerDescriptor<Work>()
                .DateRange(r => r.Field(w => w.PublicationDate)
                    .GreaterThanOrEquals(dto.TimeRange.From)
                    .LessThanOrEquals(dto.TimeRange.To));
        }

        foreach (string concept in dto.Concepts)
        {
            container &= new QueryContainerDescriptor<Work>()
                .Match(m => m.Field(w => w.Concepts).Query(concept));
        }

        ISearchResponse<Work> response;
        if (dto.OrderBy != null)
        {
            SortDescriptor<Work>? sortDescriptor = GetSortDescriptor(dto.OrderBy);
            if (sortDescriptor == null)
            {
                throw new SearchException($"Invalid sort field: {dto.OrderBy.Field}");
            }

            response = await _client.SearchAsync<Work>(s => s
                .Index(IndexName)
                .From(dto.Page * dto.PageSize)
                .Size(dto.PageSize)
                .Sort(_ => sortDescriptor)
                .Query(q => q.Bool(b => b.Must(container))));
        }
        else
        {
            response = await _client.SearchAsync<Work>(s => s
                .Index(IndexName)
                .From(dto.Page * dto.PageSize)
                .Size(dto.PageSize)
                .Query(q => q.Bool(b => b.Must(container))));
        }

        if (!response.IsValid)
        {
            throw new SearchException(response.DebugInformation);
        }

        IEnumerable<object> results;
        if (dto.Brief)
        {
            results = response.Documents.Select(_mapper.Map<Work, BriefWorkDto>).ToList();
        }
        else
        {
            results = response.Documents.Select(_mapper.Map<Work, WorkDto>).ToList();
        }

        return new PagedDto(
            response.Total,
            dto.PageSize,
            dto.Page,
            results
        );
    }

    private async Task<PagedDto> QueryWorksAdvancedImpl(QueryWorkAdvancedDto dto)
    {
        ISearchResponse<Work> response;
        if (dto.OrderBy != null)
        {
            SortDescriptor<Work>? sortDescriptor = GetSortDescriptor(dto.OrderBy);
            if (sortDescriptor == null)
            {
                throw new SearchException($"Invalid sort field: {dto.OrderBy.Field}");
            }

            response = await _client.SearchAsync<Work>(s => s
                .Index(IndexName)
                .From(dto.Page * dto.PageSize)
                .Size(dto.PageSize)
                .Sort(_ => sortDescriptor)
                .Query(q => q.Bool(b => ConstructQueryDescriptor(b, dto))));
        }
        else
        {
            response = await _client.SearchAsync<Work>(s => s
                .Index(IndexName)
                .From(dto.Page * dto.PageSize)
                .Size(dto.PageSize)
                .Query(q => q.Bool(b => ConstructQueryDescriptor(b, dto))));
        }

        if (!response.IsValid)
        {
            throw new SearchException(response.DebugInformation);
        }

        IEnumerable<object> results;
        if (dto.Brief)
        {
            results = response.Documents.Select(_mapper.Map<Work, BriefWorkDto>).ToList();
        }
        else
        {
            results = response.Documents.Select(_mapper.Map<Work, WorkDto>).ToList();
        }

        return new PagedDto(
            response.Total,
            dto.PageSize,
            dto.Page,
            results
        );
    }

    /// <summary>
    ///     Construct advanced query descriptor.
    /// </summary>
    /// <param name="descriptor">The original descriptor.</param>
    /// <param name="dto">Advanced query dto.</param>
    /// <returns>Advanced query descriptor. Null if anything bad happens.</returns>
    /// <exception cref="SearchException">If condition contains any error.</exception>
    private static BoolQueryDescriptor<Work> ConstructQueryDescriptor(
        BoolQueryDescriptor<Work> descriptor,
        QueryWorkAdvancedDto dto)
    {
        foreach (AdvancedCondition cond in dto.Conditions)
        {
            Expression<Func<Work, string>>? field = GetField(cond.Field);
            if (field != null)
            {
                QueryContainer container = new QueryContainerDescriptor<Work>()
                    .Match(m => m.Field(field).Query(cond.Value)
                        .Fuzziness(Globals.DefaultFuzziness));
                descriptor = cond.Op switch {
                    "and" => descriptor.Must(container),
                    "or" => descriptor.Should(container),
                    "not" => descriptor.MustNot(container),
                    _ => throw new SearchException($"Invalid operator: {cond.Op}")
                };
            }
            else
            {
                throw new SearchException($"Invalid field: {cond.Field}");
            }
        }

        if (dto.TimeRange != null)
        {
            descriptor.Must(q => q.DateRange(r => r.Field(w => w.PublicationDate)
                .GreaterThanOrEquals(dto.TimeRange.From)
                .LessThanOrEquals(dto.TimeRange.To)));
        }

        if (dto.Concepts.Count > 0)
        {
            var container = new QueryContainer();
            foreach (string concept in dto.Concepts)
            {
                container &= new QueryContainerDescriptor<Work>()
                    .Match(m => m.Field(w => w.Concepts).Query(concept));
            }

            descriptor.Must(container);
        }

        return descriptor;
    }

    private static Expression<Func<Work, string>>? GetField(string field)
    {
        return field switch {
            "title" => w => w.Title,
            "author" => w => w.Authors,
            "abstract" => w => w.Abstract,
            "keyword" => w => w.Keywords,
            "concept" => w => w.Concepts,
            _ => null
        };
    }

    private static SortDescriptor<Work>? GetSortDescriptor(OrderByData data)
    {
        SortOrder fieldSort = data.Ascending ? SortOrder.Ascending : SortOrder.Descending;
        return data.Field switch {
            "title" => new SortDescriptor<Work>().Field(w => w.Title, fieldSort),
            "citation" => new SortDescriptor<Work>().Field(w => w.CitationCount, fieldSort),
            "time" => new SortDescriptor<Work>().Field(w => w.PublicationDate, fieldSort),
            _ => null
        };
    }
}