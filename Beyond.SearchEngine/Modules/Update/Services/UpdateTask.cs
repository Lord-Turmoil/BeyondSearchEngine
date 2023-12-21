// Copyright (C) 2018 - 2023 Tony's Studio. All rights reserved.

using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Beyond.SearchEngine.Modules.Update.Dtos;
using Beyond.SearchEngine.Modules.Update.Services.Updater;
using Beyond.SearchEngine.Modules.Update.Services.Updater.Impl;
using Nest;

namespace Beyond.SearchEngine.Modules.Update.Services;

public class UpdateTask : IHostedService, IDisposable
{
    private readonly IConfiguration _configuration;
    private readonly IElasticClient _elasticClient;

    private readonly ILogger<UpdateTask> _logger;
    private readonly IMapper _mapper;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public UpdateTask(IServiceScopeFactory serviceScopeFactory, IMapper mapper, ILogger<UpdateTask> logger,
        IConfiguration configuration, IElasticClient elasticClient)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _mapper = mapper;
        _logger = logger;
        _configuration = configuration;
        _elasticClient = elasticClient;
    }


    public void Dispose()
    {
        // Nothing for now.
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Background update task online!");
        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Background update task offline.");
        return Task.CompletedTask;
    }

    public bool IsValidUpdateType(string type)
    {
        return Globals.AvailableTypes.Contains(type);
    }

    /// <summary>
    ///     A "pure" async method. It will not block the thread.
    /// </summary>
    /// <param name="type">Update type.</param>
    /// <param name="dto">What to update.</param>
    /// <returns></returns>
    public Task UpdateAsync(string type, InitiateUpdateDto dto)
    {
        _logger.LogInformation("Update of {type} begins", type);
        Update(type, dto);
        _logger.LogInformation("Update of {type} ends", type);

        return Task.CompletedTask;
    }

    private async Task Update(string type, InitiateUpdateDto dto)
    {
        if (!IsValidUpdateType(type))
        {
            return;
        }

        using IServiceScope scope = _serviceScopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

        IUpdater? updater = type switch {
            "institutions" => new InstitutionUpdater(unitOfWork, _mapper, _logger, _configuration, _elasticClient),
            "authors" => new AuthorUpdater(unitOfWork, _mapper, _logger, _configuration, _elasticClient),
            "works" => new WorkUpdater(unitOfWork, _mapper, _logger, _configuration, _elasticClient),
            "concepts" => new ConceptUpdater(unitOfWork, _mapper, _logger, _configuration, _elasticClient),
            "sources" => new SourceUpdater(unitOfWork, _mapper, _logger, _configuration, _elasticClient),
            "publishers" => new PublisherUpdater(unitOfWork, _mapper, _logger, _configuration, _elasticClient),
            "funders" => new FunderUpdater(unitOfWork, _mapper, _logger, _configuration, _elasticClient),
            _ => null
        };
        if (updater == null)
        {
            _logger.LogError("Unknown update type {type}", type);
            return;
        }

        await updater.Update(type, dto);
    }
}