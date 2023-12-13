﻿using Arch.EntityFrameworkCore.UnitOfWork;
using AutoMapper;
using Beyond.SearchEngine.Modules.Update.Models.Elastic;
using Beyond.Shared.Dtos;
using Beyond.Shared.Indexer.Builder;
using Beyond.Shared.Indexer.Impl;
using Nest;

namespace Beyond.SearchEngine.Modules.Update.Services.Updater.Impl;

/// <summary>
///     The Updater that update Author.
/// </summary>
public class AuthorUpdater
    : GenericUpdater<AuthorIndexer, ElasticAuthor, AuthorDtoBuilder, AuthorDto>
{
    public AuthorUpdater(IUnitOfWork unitOfWork, IMapper mapper, ILogger<UpdateTask> logger,
        IConfiguration configuration, IElasticClient client)
        : base(unitOfWork, mapper, logger, configuration, client)
    {
    }
}