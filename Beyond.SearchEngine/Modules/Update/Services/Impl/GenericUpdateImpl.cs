using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Beyond.SearchEngine.Modules.Update.Dtos;
using Beyond.SearchEngine.Modules.Update.Services.Utils;
using Beyond.Shared.Indexer;
using Beyond.Shared.Indexer.Builder;
using Beyond.Shared.Indexer.Impl;

namespace Beyond.SearchEngine.Modules.Update.Services.Impl;

public class GenericUpdateImpl<TIndexer, TModel, TBuilder, TDto> : BaseUpdateImpl
    where TIndexer : GenericIndexer<TBuilder, TDto>
    where TBuilder : IDtoBuilder<TDto>, new()
    where TModel : class
{
    protected GenericUpdateImpl(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateTask> logger)
        : base(unitOfWork, mapper, logger)
    {
    }

    public async Task Update(string type, InitiateUpdateDto dto)
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

                await _unitOfWork.SaveChangesAsync();
            }
            else
            {
                _logger.LogWarning("Empty chunk for {entry}", entry);
            }

            await PostUpdate(type, entry, recordCount);

            result = await UpdatePreamble(type, indexer.CurrentManifestEntry());
        }
    }
}