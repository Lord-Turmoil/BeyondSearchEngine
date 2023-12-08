using Arch.EntityFrameworkCore.UnitOfWork;
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
    private readonly ILogger<UpdateTask> _logger;

    public InstitutionUpdateImpl(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateTask> logger)
        : base(unitOfWork, mapper)
    {
        _logger = logger;
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
            _logger.LogError($"Failed to update: {e.Message}");
            UpdateMutex.EndUpdate(type);
        }
    }

    private async ValueTask UpdateImpl(string type, InstitutionIndexer indexer)
    {
        IRepository<Institution> repo = _unitOfWork.GetRepository<Institution>();
        int result = await UpdatePreamble(type, indexer);

        while (result != -1)
        {
            if (result == 0)
            {
                indexer.NextManifestEntry();
                result = await UpdatePreamble(type, indexer);
                continue;
            }

            ManifestEntry entry = indexer.CurrentManifestEntry()!;
            _logger.LogInformation($"Updating {type} at {entry.UpdatedDate}");

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
                        _logger.LogError($"Failed to insert Institution: {e.Message}");
                    }
                }

                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning($"Empty chunk for {entry}");
            }

            await PostUpdate(type, entry);

            result = await UpdatePreamble(type, indexer);
        }

        UpdateMutex.EndUpdate(type);
    }

    /// <summary>
    ///     -1: No more
    ///     0: Already updated
    ///     1: Can update
    /// </summary>
    /// <param name="entry"></param>
    /// <returns></returns>
    private async ValueTask<int> UpdatePreamble(string type, InstitutionIndexer indexer)
    {
        ManifestEntry? entry = indexer.CurrentManifestEntry();
        if (entry == null)
        {
            return -1;
        }

        if (!await AddUpdateHistory(type, entry.UpdatedDate))
        {
            _logger.LogWarning($"{type} updated at {entry.UpdatedDate} is already updated");
            return 0;
        }

        return 1;
    }

    private async ValueTask PostUpdate(string type, ManifestEntry entry)
    {
        if (!await CompleteUpdateHistory(type, entry.UpdatedDate))
        {
            _logger.LogError($"Failed to complete update of {type} at {entry.UpdatedDate}");
        }
        else
        {
            await _unitOfWork.SaveChangesAsync();
        }
    }
}