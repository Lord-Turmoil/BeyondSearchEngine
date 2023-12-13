using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Beyond.SearchEngine.Extensions.Update;
using Beyond.SearchEngine.Modules.Search.Models;
using Beyond.SearchEngine.Modules.Update.Dtos;
using Beyond.Shared.Indexer;
using Beyond.Shared.Indexer.Builder;
using Beyond.Shared.Indexer.Impl;

namespace Beyond.SearchEngine.Modules.Update.Services.Updater;

public class GenericUpdater<TIndexer, TModel, TDtoBuilder, TDto> : BaseUpdater
    where TIndexer : GenericIndexer<TDtoBuilder, TDto>
    where TDtoBuilder : IDtoBuilder<TDto>, new()
    where TModel : OpenAlexModel
    where TDto : class
{
    private readonly UpdateOptions _options = new();

    protected GenericUpdater(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateTask> logger, IConfiguration configuration)
        : base(unitOfWork, mapper, logger)
    {
        configuration.GetRequiredSection(UpdateOptions.UpdateSection).Bind(_options);
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

            // await UpdateImpl(type, indexer);
            await UpdateImplWithYield(type, indexer);
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
    ///     Iterate through all manifest entries and update the database.
    ///     It bulks update the database, so the new data is either successfully
    ///     put into database, or completely not.
    /// </summary>
    /// <param name="type">Update type.</param>
    /// <param name="indexer">Specific indexer.</param>
    /// <returns></returns>
    [Obsolete("This will cause memory and performance issues. Use UpdateImplWithYield() instead")]
    private async ValueTask UpdateImpl(string type, TIndexer indexer)
    {
        IRepository<TModel> repo = _unitOfWork.GetRepository<TModel>();
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

            List<TDto>? chunk = indexer.NextDataChunk();
            int recordCount = 0;
            int bulkSaveSize = 0;
            bool success = true;
            if (chunk != null)
            {
                foreach (TDto dto in chunk)
                {
                    try
                    {
                        TModel model = _mapper.Map<TDto, TModel>(dto);

                        //
                        // 2023/12/13 TS:
                        //   We don't need to check if the record exists in database.
                        //   If needed, uncomment this.
                        //   
                        // if (await repo.ExistsAsync(x => x.Id == model.Id))
                        // {
                        //     repo.Update(model);
                        // }
                        // else
                        // {
                        //     await repo.InsertAsync(model);
                        // }

                        await repo.InsertAsync(model);
                        recordCount++;
                        bulkSaveSize++;
                    }
                    catch (Exception e)
                    {
                        _logger.LogError("Failed to build {name}: {exception}", typeof(TModel).Name, e);
                    }

                    if (bulkSaveSize == _options.BulkUpdateSize)
                    {
                        try
                        {
                            await _unitOfWork.SaveChangesAsync();
                        }
                        catch (Exception e)
                        {
                            _logger.LogError("Failed to save changes: {exception}", e.Message);
                            success = false;
                        }

                        bulkSaveSize = 0;
                    }
                }

                if (bulkSaveSize > 0)
                {
                    try
                    {
                        await _unitOfWork.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError("Failed to save changes: {exception}", e.Message);
                        success = false;
                    }
                }
            }
            else
            {
                _logger.LogWarning("Empty chunk for {entry}", entry);
            }

            if (success)
            {
                await PostUpdate(type, entry, recordCount);
            }
            else
            {
                _logger.LogError("Failed to update {type} at {UpdatedDate}", type, entry.UpdatedDate);
            }

            result = await UpdatePreamble(type, indexer.CurrentManifestEntry());
        }
    }


    /// <summary>
    ///     This one will update the database with yield return, which
    ///     reduces the memory usage and improves the performance.
    /// </summary>
    /// <param name="type"></param>
    /// <param name="indexer"></param>
    /// <returns></returns>
    private async ValueTask UpdateImplWithYield(string type, TIndexer indexer)
    {
        IRepository<TModel> repo = _unitOfWork.GetRepository<TModel>();
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
            bool success = true;
            foreach (TDto dto in indexer.AllDto())
            {
                try
                {
                    TModel model = _mapper.Map<TDto, TModel>(dto);
                    await repo.InsertAsync(model);
                    recordCount++;
                    bulkSaveSize++;
                }
                catch (Exception e)
                {
                    _logger.LogError("Failed to build {name}: {exception}", typeof(TModel).Name, e);
                }

                if (bulkSaveSize == _options.BulkUpdateSize)
                {
                    bulkSaveSize = 0;
                    try
                    {
                        await _unitOfWork.SaveChangesAsync();
                    }
                    catch (Exception e)
                    {
                        _logger.LogError("Failed to save changes: {exception}", e.Message);
                        success = false;
                        break;
                    }
                }
            }

            if (bulkSaveSize > 0)
            {
                try
                {
                    await _unitOfWork.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    _logger.LogError("Failed to save changes: {exception}", e.Message);
                    success = false;
                }
            }

            if (success)
            {
                await PostUpdate(type, entry, recordCount);
            }
            else
            {
                _logger.LogError("Failed to update {type} at {UpdatedDate}", type, entry.UpdatedDate);
            }

            result = await UpdatePreamble(type, indexer.CurrentManifestEntry());
        }
    }
}