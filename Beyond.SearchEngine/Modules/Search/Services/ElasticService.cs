using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Nest;

namespace Beyond.SearchEngine.Modules.Search.Services;

public class ElasticService<TService>
{
    protected readonly IElasticClient _client;
    protected readonly ILogger<TService> _logger;
    protected readonly IMapper _mapper;

    protected ElasticService(IElasticClient client, IMapper mapper, ILogger<TService> logger)
    {
        _client = client;
        _mapper = mapper;
        _logger = logger;
    }
}