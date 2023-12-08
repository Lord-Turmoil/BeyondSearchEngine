using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Beyond.SearchEngine.Modules.Search.Models;
using Beyond.Shared.Dtos;
using Beyond.Shared.Indexer.Builder;
using Beyond.Shared.Indexer.Impl;

namespace Beyond.SearchEngine.Modules.Update.Services.Updater.Impl;

/// <summary>
/// The Updater that update Author.
/// </summary>
public class AuthorUpdater
    : GenericUpdater<AuthorIndexer, Author, AuthorDtoBuilder, AuthorDto>
{
    public AuthorUpdater(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateTask> logger)
        : base(unitOfWork, mapper, logger)
    {
    }
}