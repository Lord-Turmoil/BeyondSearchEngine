// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using System.Linq.Expressions;
using System.Text;
using AutoMapper;
using Beyond.SearchEngine.Extensions.Cache;
using Beyond.SearchEngine.Extensions.Module;
using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Search.Models;
using Beyond.SearchEngine.Modules.Search.Services.Exceptions;
using Beyond.SearchEngine.Modules.Utils;
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
        string key = $"{IndexName}:related:{id}:{brief}";
        var value = await _cache.GetAsync<IEnumerable<object>>(key);
        if (value != null)
        {
            return new OkResponse(new OkDto(data: value));
        }

        var agent = new SearchAgent(_client, _mapper, _cache);

        WorkDto? dto = await agent.GetSingleById<Work, WorkDto>(IndexName, id, brief);
        if (dto == null)
        {
            return new NotFoundResponse(new NotFoundDto());
        }

        if (brief)
        {
            value = await agent.GetManyById<Work, BriefWorkDto>(
                IndexName, dto.RelatedWorkList, brief);
        }
        else
        {
            value = await agent.GetManyById<Work, WorkDto>(
                IndexName, dto.RelatedWorkList, brief);
        }

        await _cache.SetAsync(key, value);

        return new OkResponse(new OkDto(data: value));
    }

    public async Task<ApiResponse> GetReferencedWorks(string id, bool brief)
    {
        string key = $"{IndexName}:ref:{id}:{brief}";
        var value = await _cache.GetAsync<IEnumerable<object>>(key);
        if (value != null)
        {
            return new OkResponse(new OkDto(data: value));
        }

        var agent = new SearchAgent(_client, _mapper, _cache);

        WorkDto? dto = await agent.GetSingleById<Work, WorkDto>(IndexName, id, brief);
        if (dto == null)
        {
            return new NotFoundResponse(new NotFoundDto());
        }

        if (brief)
        {
            value = await agent.GetManyById<Work, BriefWorkDto>(
                IndexName, dto.ReferencedWorkList, brief);
        }
        else
        {
            value = await agent.GetManyById<Work, WorkDto>(
                IndexName, dto.ReferencedWorkList, brief);
        }

        await _cache.SetAsync(key, value);

        return new OkResponse(new OkDto(data: value));
    }

    public async Task<ApiResponse> QueryWorksBasic(QueryWorkBasicDto dto)
    {
        string key = $"{IndexName}:query:{JsonConvert.SerializeObject(dto)}";
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
        string key = $"{IndexName}:query:{JsonConvert.SerializeObject(dto)}";
        var value = await _cache.GetAsync<PagedDto>(key);
        if (value != null)
        {
            return new OkResponse(new OkDto(data: value));
        }

        value = await QueryWorksAdvancedImpl(dto);
        await _cache.SetAsync(key, value);

        return new OkResponse(new OkDto(data: value));
    }

    public async Task<ApiResponse> QueryWorksWithAllFields(QueryWorkAllFieldsDto dto)
    {
        string key = $"{IndexName}:query:{JsonConvert.SerializeObject(dto)}";
        var value = await _cache.GetAsync<PagedDto>(key);
        if (value != null)
        {
            return new OkResponse(new OkDto(data: value));
        }

        value = await QueryWorksAllFieldsImpl(dto);
        await _cache.SetAsync(key, value);

        return new OkResponse(new OkDto(data: value));
    }

    public async Task<ApiResponse> GetCitations(string type, IReadOnlyCollection<string> idList)
    {
        StringBuilder builder = new();
        var agent = new SearchAgent(_client, _mapper, _cache);
        bool first = true;

        foreach (string id in idList)
        {
            if (string.IsNullOrEmpty(id))
            {
                continue;
            }

            string key = $"{IndexName}:citation:{type}:{id}";
            string? citation = await _cache.GetStringAsync(key);
            if (citation != null)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    builder.AppendLine();
                }

                builder.AppendLine(citation);
                continue;
            }

            var work = await agent.GetModelById<Work>(IndexName, id);
            if (work == null)
            {
                // Warning: We swallow this error.
                continue;
            }

            // Each citation is guaranteed to have a line separator.
            citation = CitationGenerator.GenerateCitation(type, work);
            if (citation == null)
            {
                return new BadRequestResponse(new BadRequestDto("Invalid citation type"));
            }

            if (first)
            {
                first = false;
            }
            else
            {
                builder.AppendLine();
            }

            builder.Append(citation);
        }

        return new OkResponse(new OkDto(data: builder.ToString()));
    }

    public async Task<ApiResponse> GetTopWorks(DateTime? begin, DateTime? end, int pageSize, int page)
    {
        string key = $"{IndexName}:top:{pageSize}:{page}";
        var value = await _cache.GetAsync<PagedDto>(key);
        if (value != null)
        {
            return new OkResponse(new OkDto(data: value));
        }

        Func<QueryContainerDescriptor<Work>, QueryContainer> query = descriptor => {
            if (begin == null)
            {
                return end == null
                    ? descriptor.MatchAll()
                    : descriptor.DateRange(m => m.Field(f => f.PublicationDate).LessThanOrEquals(end));
            }

            return end == null
                ? descriptor.DateRange(m => m.Field(f => f.PublicationDate).GreaterThanOrEquals(begin))
                : descriptor.DateRange(m => m.Field(f => f.PublicationDate)
                    .GreaterThanOrEquals(begin)
                    .LessThanOrEquals(end));
        };

        ISearchResponse<Work> response = await _client.SearchAsync<Work>(s => s
            .Index(IndexName)
            .From(page * pageSize)
            .Size(pageSize)
            .Sort(ss => ss.Field(f => f.CitationCount, SortOrder.Descending))
            .Query(query));

        if (!response.IsValid)
        {
            _logger.LogError(response.DebugInformation);
            return new InternalServerErrorResponse(new SearchFailedDto());
        }

        value = new PagedDto(
            response.Total,
            pageSize,
            page,
            response.Documents.Select(_mapper.Map<Work, BriefWorkDto>).ToList()
        );

        await _cache.SetAsync(key, value);

        return new OkResponse(new OkDto(data: value));
    }

    private async Task<PagedDto> QueryWorksBasicImpl(QueryWorkBasicDto dto)
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
                .Query(q => q.Bool(b => ConstructBasicQueryDescriptor(b, dto))));
        }
        else
        {
            response = await _client.SearchAsync<Work>(s => s
                .Index(IndexName)
                .From(dto.Page * dto.PageSize)
                .Size(dto.PageSize)
                .Query(q => q.Bool(b => ConstructBasicQueryDescriptor(b, dto))));
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
            results = response.Documents.Select(m => _mapper.Map<Work, WorkSearchDto>(m).Mock()).ToList();
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
                .Query(q => q.Bool(b => ConstructAdvancedQueryDescriptor(b, dto))));
        }
        else
        {
            response = await _client.SearchAsync<Work>(s => s
                .Index(IndexName)
                .From(dto.Page * dto.PageSize)
                .Size(dto.PageSize)
                .Query(q => q.Bool(b => ConstructAdvancedQueryDescriptor(b, dto))));
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
            results = response.Documents.Select(m => _mapper.Map<Work, WorkSearchDto>(m).Mock()).ToList();
        }

        return new PagedDto(
            response.Total,
            dto.PageSize,
            dto.Page,
            results
        );
    }

    private async Task<PagedDto> QueryWorksAllFieldsImpl(QueryWorkAllFieldsDto dto)
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
                .Query(q => q.Bool(b => ConstructAllFieldsQueryDescriptor(b, dto))));
        }
        else
        {
            response = await _client.SearchAsync<Work>(s => s
                .Index(IndexName)
                .From(dto.Page * dto.PageSize)
                .Size(dto.PageSize)
                .Query(q => q.Bool(b => ConstructAllFieldsQueryDescriptor(b, dto))));
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
            results = response.Documents.Select(m => _mapper.Map<Work, WorkSearchDto>(m).Mock()).ToList();
        }

        return new PagedDto(
            response.Total,
            dto.PageSize,
            dto.Page,
            results
        );
    }

    private static BoolQueryDescriptor<Work> ConstructBasicQueryDescriptor(
        BoolQueryDescriptor<Work> descriptor,
        QueryWorkBasicDto dto)
    {
        var container = new QueryContainer();

        foreach (BasicCondition cond in dto.Conditions)
        {
            if (string.IsNullOrEmpty(cond.Value))
            {
                continue;
            }

            Expression<Func<Work, string>>? field = GetField(cond.Field);
            if (field == null)
            {
                throw new SearchException($"Invalid field: {cond.Field}");
            }

            container &= new QueryContainerDescriptor<Work>()
                .Match(m => m.Field(field).Query(cond.Value)
                    .Fuzziness(Globals.DefaultFuzziness));
        }

        descriptor.Must(container);

        if (dto.TimeRange != null)
        {
            descriptor.Filter(q => q
                .DateRange(r => r.Field(w => w.PublicationDate)
                    .GreaterThanOrEquals(dto.TimeRange.From)
                    .LessThanOrEquals(dto.TimeRange.To)));
        }

        if (dto.Concepts.Count > 0)
        {
            container = new QueryContainer();
            foreach (string concept in dto.Concepts)
            {
                container &= new QueryContainerDescriptor<Work>()
                    .Match(m => m.Field(w => w.Concepts).Query(concept));
            }

            descriptor.Filter(container);
        }

        return descriptor;
    }

    /// <summary>
    ///     Construct advanced query descriptor.
    /// </summary>
    /// <param name="descriptor">The original descriptor.</param>
    /// <param name="dto">Advanced query dto.</param>
    /// <returns>Advanced query descriptor. Null if anything bad happens.</returns>
    /// <exception cref="SearchException">If condition contains any error.</exception>
    private static BoolQueryDescriptor<Work> ConstructAdvancedQueryDescriptor(
        BoolQueryDescriptor<Work> descriptor,
        QueryWorkAdvancedDto dto)
    {
        foreach (AdvancedCondition cond in dto.Conditions)
        {
            if (string.IsNullOrEmpty(cond.Value))
            {
                continue;
            }

            Expression<Func<Work, string>>? field = GetField(cond.Field);
            if (field != null)
            {
                QueryContainer container = cond.Fuzzy
                    ? new QueryContainerDescriptor<Work>()
                        .Match(m => m.Field(field).Query(cond.Value)
                            .Fuzziness(Globals.DefaultFuzziness))
                    : new QueryContainerDescriptor<Work>()
                        .Match(m => m.Field(field).Query(cond.Value));

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
            descriptor.Filter(q => q.DateRange(r => r.Field(w => w.PublicationDate)
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

            descriptor.Filter(container);
        }

        return descriptor;
    }

    private static BoolQueryDescriptor<Work> ConstructAllFieldsQueryDescriptor(
        BoolQueryDescriptor<Work> descriptor,
        QueryWorkAllFieldsDto dto)
    {
        descriptor.Should(should => should.MultiMatch(m => m
            .Fields(f => f.Field(w => w.Title)
                .Field(w => w.Authors)
                .Field(w => w.Abstract))
            .Query(dto.Query)));

        if (dto.TimeRange != null)
        {
            descriptor.Filter(q => q.DateRange(r => r.Field(w => w.PublicationDate)
                .GreaterThanOrEquals(dto.TimeRange.From)
                .LessThanOrEquals(dto.TimeRange.To)));
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
            "source" => w => w.Source,
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