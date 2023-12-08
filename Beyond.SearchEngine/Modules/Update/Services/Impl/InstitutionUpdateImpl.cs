﻿using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Beyond.SearchEngine.Modules.Search.Models;
using Beyond.SearchEngine.Modules.Update.Dtos;
using Beyond.SearchEngine.Modules.Update.Services.Utils;
using Beyond.Shared.Dtos;
using Beyond.Shared.Indexer;
using Beyond.Shared.Indexer.Impl;

namespace Beyond.SearchEngine.Modules.Update.Services.Impl;

public class InstitutionUpdateImpl : BaseUpdateImpl
{
    public InstitutionUpdateImpl(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateTask> logger)
        : base(unitOfWork, mapper, logger)
    {
    }

    public async Task Update(string type, InitiateUpdateDto dto)
    {
        if (!UpdateMutex.BeginUpdate(type))
        {
            _logger.LogError($"Already updating {type}, failed to initiate new update");
            return;
        }

        try
        {
            InstitutionIndexer indexer = new(
                dto.DataPath,
                dto.TempPath,
                dto.Begin,
                dto.End);
            await UpdateImpl(type, indexer);
        }
        catch (Exception e)
        {
            // Make sure to release Mutex.
            _logger.LogError("Failed to update: {exception}", e);
        }
        finally
        {
            // Ensure to release the mutex.
            UpdateMutex.EndUpdate(type);
        }
    }

    private async ValueTask UpdateImpl(string type, InstitutionIndexer indexer)
    {
        IRepository<Institution> repo = _unitOfWork.GetRepository<Institution>();
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

            List<InstitutionDto>? chunk = indexer.NextDataChunk();
            if (chunk != null)
            {
                foreach (InstitutionDto dto in chunk)
                {
                    try
                    {
                        Institution institution = _mapper.Map<InstitutionDto, Institution>(dto);
                        await repo.InsertAsync(institution);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError("Failed to insert Institution: {exception}", e);
                    }
                }

                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning("Empty chunk for {entry}", entry);
            }

            await PostUpdate(type, entry);

            result = await UpdatePreamble(type, indexer.CurrentManifestEntry());
        }
    }
}