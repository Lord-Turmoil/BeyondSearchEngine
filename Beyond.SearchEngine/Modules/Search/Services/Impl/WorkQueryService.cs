using AutoMapper;
using Beyond.SearchEngine.Modules.Search.Dtos;
using Beyond.SearchEngine.Modules.Search.Models;
using Beyond.Shared.Dtos;
using Nest;
using System.Collections.Generic;
using Tonisoft.AspExtensions.Response;

namespace Beyond.SearchEngine.Modules.Search.Services.Impl;

public class WorkQueryService : ElasticService<WorkQueryService>, IWorkQueryService
{
    private const string IndexName = "works";

    public WorkQueryService(IElasticClient client, IMapper mapper, ILogger<WorkQueryService> logger)
        : base(client, mapper, logger)
    {
    }

    public async Task<ApiResponse> GetRelatedWorks(string id, bool brief)
    {
        var impl = new SearchImpl<WorkQueryService>(_client, _mapper);

        WorkDto? dto = await impl.SearchSingleById<Work, WorkDto>(IndexName, id);
        if (dto == null)
        {
            return new NotFoundResponse(new NotFoundDto());
        }

        IEnumerable<object> result;
        if (brief)
        {
            result = await impl.SearchManyById<Work, BriefWorkDto>(
                IndexName, dto.RelatedWorkList);
        }
        else
        {
            result = await impl.SearchManyById<Work, WorkDto>(
                IndexName, dto.RelatedWorkList);
        }

        return new OkResponse(new OkDto(data: result));
    }

    public async Task<ApiResponse> GetReferencedWorks(string id, bool brief)
    {
        var impl = new SearchImpl<WorkQueryService>(_client, _mapper);

        WorkDto? dto = await impl.SearchSingleById<Work, WorkDto>(IndexName, id);
        if (dto == null)
        {
            return new NotFoundResponse(new NotFoundDto());
        }

        IEnumerable<object> result;
        if (brief)
        {
            result = await impl.SearchManyById<Work, BriefWorkDto>(
                IndexName, dto.ReferencedWorkList);
        }
        else
        {
            result = await impl.SearchManyById<Work, WorkDto>(
                IndexName, dto.ReferencedWorkList);
        }

        return new OkResponse(new OkDto(data: result));
    }
}