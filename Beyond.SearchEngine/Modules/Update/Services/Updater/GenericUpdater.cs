﻿using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Beyond.SearchEngine.Modules.Update.Dtos;
using Beyond.Shared.Indexer;
using Beyond.Shared.Indexer.Builder;
using Beyond.Shared.Indexer.Impl;

namespace Beyond.SearchEngine.Modules.Update.Services.Updater;

public class GenericUpdater<TIndexer, TModel, TBuilder, TDto> : BaseUpdater
    where TIndexer : GenericIndexer<TBuilder, TDto>
    where TBuilder : IDtoBuilder<TDto>, new()
    where TModel : class
{
    protected GenericUpdater(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateTask> logger)
        : base(unitOfWork, mapper, logger)
    {
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
                dto.DataPath,
                dto.TempPath,
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
    ///     Iterate through all manifest entries and update the database.
    ///     It bulks update the database, so the new data is either successfully
    ///     put into database, or completely not.
    /// </summary>
    /// <param name="type">Update type.</param>
    /// <param name="indexer">Specific indexer.</param>
    /// <returns></returns>
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
            bool success = true;
            if (chunk != null)
            {
                foreach (TDto dto in chunk)
                {
                    try
                    {
                        TModel model = _mapper.Map<TDto, TModel>(dto);
                        await repo.InsertAsync(model);
                        recordCount++;
                    }
                    catch (Exception e)
                    {
                        _logger.LogError("Failed to build {name}: {exception}", typeof(TModel).Name, e);
                    }
                }

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
}