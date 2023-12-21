// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using AutoMapper;
using Nest;

namespace Beyond.SearchEngine.Modules.Search.Services.Impl;

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