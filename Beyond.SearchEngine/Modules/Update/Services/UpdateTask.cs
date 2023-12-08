using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Beyond.SearchEngine.Modules.Update.Dtos;
using Beyond.SearchEngine.Modules.Update.Services.Impl;

namespace Beyond.SearchEngine.Modules.Update.Services;

public class UpdateTask : IHostedService, IDisposable
{
    private readonly ILogger<UpdateTask> _logger;
    private readonly IMapper _mapper;
    private readonly IServiceScopeFactory _serviceScopeFactory;

    public UpdateTask(IServiceScopeFactory serviceScopeFactory, IMapper mapper, ILogger<UpdateTask> logger)
    {
        _serviceScopeFactory = serviceScopeFactory;
        _mapper = mapper;
        _logger = logger;
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

    public Task UpdateInstitutionAsync(string type, InitiateUpdateDto dto)
    {
        _logger.LogInformation($"Update of {type} begins");
        UpdateInstitution(type, dto);
        return Task.CompletedTask;
    }

    private async Task UpdateInstitution(string type, InitiateUpdateDto dto)
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var impl = new InstitutionUpdateImpl(unitOfWork, _mapper, _logger);

        await impl.Update(type, dto);
    }

    public Task UpdateAuthorAsync(string type, InitiateUpdateDto dto)
    {
        _logger.LogInformation($"Update of {type} begins");
        UpdateAuthor(type, dto);
        return Task.CompletedTask;
    }

    private async Task UpdateAuthor(string type, InitiateUpdateDto dto)
    {
        using IServiceScope scope = _serviceScopeFactory.CreateScope();
        var unitOfWork = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
        var impl = new AuthorUpdateImpl(unitOfWork, _mapper, _logger);

        await impl.Update(type, dto);
    }
}