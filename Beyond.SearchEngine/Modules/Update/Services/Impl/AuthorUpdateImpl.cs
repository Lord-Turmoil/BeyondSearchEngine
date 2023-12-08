using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Beyond.SearchEngine.Modules.Search.Models;
using Beyond.SearchEngine.Modules.Update.Dtos;
using Beyond.SearchEngine.Modules.Update.Services.Utils;
using Beyond.Shared.Dtos;
using Beyond.Shared.Indexer;
using Beyond.Shared.Indexer.Impl;
using SharpCompress.Common;

namespace Beyond.SearchEngine.Modules.Update.Services.Impl;

public class AuthorUpdateImpl : BaseUpdateImpl
{
    public AuthorUpdateImpl(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateTask> logger)
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
            AuthorIndexer indexer = new(
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

    private async ValueTask UpdateImpl(string type, AuthorIndexer indexer)
    {
        IRepository<Author> repo = _unitOfWork.GetRepository<Author>();
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

            List<AuthorDto>? chunk = indexer.NextDataChunk();
            if (chunk != null)
            {
                foreach (AuthorDto dto in chunk)
                {
                    try
                    {
                        Author author = _mapper.Map<AuthorDto, Author>(dto);
                        await repo.InsertAsync(author);
                    }
                    catch (Exception e)
                    {
                        _logger.LogError("Failed to insert author: {exception}", e);
                    }
                }
            }
            else
            {
                _logger.LogWarning("Empty chunk for {entry}", entry);
            }

            await _unitOfWork.SaveChangesAsync();
        }

        await CompleteUpdateHistory(type, indexer.EndDate);
    }
}