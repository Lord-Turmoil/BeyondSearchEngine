using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Beyond.SearchEngine.Extensions.Update;
using Beyond.SearchEngine.Modules.Update.Dtos;
using Beyond.SearchEngine.Modules.Update.Models.Elastic;
using Beyond.Shared.Indexer;
using Beyond.Shared.Indexer.Builder;
using Beyond.Shared.Indexer.Impl;
using Nest;

namespace Beyond.SearchEngine.Modules.Update.Services.Updater;

public class GenericUpdater<TIndexer, TModel, TDtoBuilder, TDto> : BaseUpdater
    where TIndexer : GenericIndexer<TDtoBuilder, TDto>
    where TDtoBuilder : IDtoBuilder<TDto>, new()
    where TModel : ElasticModel
    where TDto : class
{
    private readonly IElasticClient _client;
    private readonly UpdateOptions _options = new();

    private readonly Task[] _updateTasks;

    protected GenericUpdater(
        IUnitOfWork unitOfWork,
        IMapper mapper,
        ILogger<UpdateTask> logger,
        IConfiguration configuration,
        IElasticClient client)
        : base(unitOfWork, mapper, logger)
    {
        _client = client;
        configuration.GetRequiredSection(UpdateOptions.UpdateSection).Bind(_options);
        _updateTasks = new Task[_options.ConcurrentUpdate];
    }

    public override async Task Update(string type, InitiateUpdateDto dto)
    {
        if (!UpdateMutex.BeginUpdate(type))
        {
            _logger.LogError("Already updating {type}, failed to initiate new update", type);
            return;
        }

        try
        {
            var indexer = (TIndexer?)Activator.CreateInstance(
                typeof(TIndexer),
                Path.Join(_options.DataPath, type),
                Path.Join(_options.TempPath, type),
                dto.Begin,
                dto.End);
            if (indexer == null)
            {
                throw new Exception($"Failed to create indexer for {type}");
            }

            await UpdateImpl(type, indexer);
        }
        catch (Exception e)
        {
            _logger.LogError("Failed to update: {exception}", e);
        }
        finally
        {
            // Ensure to release the mutex.
            UpdateMutex.EndUpdate(type);
        }
    }

    /// <summary>
    ///     This one will update the database with yield return, which
    ///     reduces the memory usage and improves the performance.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="indexer"></param>
    /// <returns></returns>
    private async Task UpdateImpl(string type, TIndexer indexer)
    {
        // Initialize the update tasks.
        for (int i = 0; i < _options.ConcurrentUpdate; i++)
        {
            _updateTasks[i] = Task.CompletedTask;
        }
        int currentTask = 0;

        int result = await UpdatePreamble(type, indexer.CurrentManifestEntry());
        while (result != -1)
        {
            if (result == 0)
            {
                indexer.NextManifestEntry();
                result = await UpdatePreamble(type, indexer.CurrentManifestEntry());
                continue;
            }

            ManifestEntry entry = indexer.CurrentManifestEntry()!;
            _logger.LogInformation("Updating {type} at {UpdatedDate}", type, entry.UpdatedDate);

            int recordCount = 0;
            int bulkSaveSize = 0;
            var bulkDescriptor = new BulkDescriptor();
            foreach (TDto dto in indexer.AllDto())
            {
                try
                {
                    TModel model = _mapper.Map<TDto, TModel>(dto);
                    bulkDescriptor.Index<TModel>(op => op
                        .Document(model)
                        .Id(model.Id)
                        .Index(type)
                    );
                    recordCount++;
                    bulkSaveSize++;
                }
                catch (Exception e)
                {
                    _logger.LogError("Failed to build {name}: {exception}", typeof(TModel).Name, e);
                }

                if (bulkSaveSize < _options.BulkUpdateSize)
                {
                    continue;
                }

                // Update when reach bulk update size.
                await _updateTasks[currentTask];
                _updateTasks[currentTask] = BulkUpdate(bulkDescriptor);
                currentTask = (currentTask + 1) % _options.ConcurrentUpdate;

                // Reset bulk save size.
                bulkSaveSize = 0;
                bulkDescriptor = new BulkDescriptor();
            }

            // Update the rest of the records.
            if (bulkSaveSize > 0)
            {
                await _updateTasks[currentTask];
                _updateTasks[currentTask] = BulkUpdate(bulkDescriptor);
            }

            // Wait for all the tasks to finish.
            Task.WaitAll(_updateTasks);
            await PostUpdate(type, entry, recordCount);

            result = await UpdatePreamble(type, indexer.CurrentManifestEntry());
        }
    }

    private async Task BulkUpdate(IBulkRequest bulkDescriptor)
    {
        BulkResponse? response = await _client.BulkAsync(bulkDescriptor);
        if (response.IsValid)
        {
            return;
        }

        _logger.LogError("Failed to bulk update: {response}", response.DebugInformation);
        foreach (BulkResponseItemBase item in response.ItemsWithErrors)
        {
            _logger.LogError("Failed to index {type} with {id}", item.Index, item.Id);
        }
    }
}