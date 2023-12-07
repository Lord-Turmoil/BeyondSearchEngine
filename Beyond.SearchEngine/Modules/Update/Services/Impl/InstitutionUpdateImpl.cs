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
    private readonly ILogger<UpdateService> _logger;

    public InstitutionUpdateImpl(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateService> logger)
        : base(unitOfWork, mapper)
    {
        _logger = logger;
    }

    public void Update(string type, InitiateUpdateDto dto)
    {
        if (!UpdateMutex.BeginUpdate(type))
        {
            _logger.LogError($"Already updating {type}, failed to initiate new update");
            return;
        }

        InstitutionIndexer indexer = new(
            dto.DataPath,
            dto.TempPath,
            dto.Begin,
            dto.End);

        UpdateImpl(type, indexer);
    }

    private async Task UpdateImpl(string type, InstitutionIndexer indexer)
    {
        // Initiating update.
        ManifestEntry? entry = indexer.CurrentManifestEntry();
        if (entry == null)
        {
            return;
        }

        if (!await AddUpdateHistory(type, entry.UpdatedDate))
        {
            _logger.LogWarning($"{type} updated at {entry.UpdatedDate} is already updated");
            return;
        }

        // Do update, may take quite long.
        IRepository<Institution> repo = _unitOfWork.GetRepository<Institution>();
        List<InstitutionDto>? chunk = indexer.NextDataChunk();
        while (chunk != null)
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

            chunk = indexer.NextDataChunk();
        }

        // Finish up update.
        if (!await CompleteUpdateHistory(type, entry.UpdatedDate))
        {
            _logger.LogError($"Failed to complete update of {type} at {entry.UpdatedDate}");
        }
    }
}