using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Beyond.SearchEngine.Modules.Search.Models;
using Beyond.Shared.Dtos;
using Beyond.Shared.Indexer.Builder;
using Beyond.Shared.Indexer.Impl;

namespace Beyond.SearchEngine.Modules.Update.Services.Updater.Impl;

public class ConceptUpdater
    : GenericUpdater<ConceptIndexer, Concept, ConceptDtoBuilder, ConceptDto>
{
    public ConceptUpdater(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateTask> logger)
        : base(unitOfWork, mapper, logger)
    {
    }
}